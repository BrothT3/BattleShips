using BattleShips;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static BattleShips.NetworkMessageBaseEventHandler;

namespace BattleShips
{
    public class NetWorkHandler
    {
        static int port = 11000;
        //IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse("20.216.185.74"), port);
        private IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
        private UdpClient client;
        private NetworkMessageBaseEventHandler messageHandler;

        public NetWorkHandler(NetworkMessageBaseEventHandler networkMessageBaseEventHandler)
        {
            messageHandler = networkMessageBaseEventHandler;
            client = new UdpClient();
            client.Connect(groupEP);
            Thread listeningThread = new Thread(Listening);
            listeningThread.IsBackground = true;
            listeningThread.Start();

        }

        public void SendMessageToServer(NetworkMessageBase networkMessage, MessageType messageType)
        {        
                var message = new NetworkMessage()
                {
                    type = messageType,
                    message = networkMessage
                };

                var serializedNetworkMessage = JsonConvert.SerializeObject(message);

                byte[] jsonAsBytes = Encoding.UTF8.GetBytes(serializedNetworkMessage);

                Debug.WriteLine($"Sending json message {serializedNetworkMessage} to server...");

                client.Send(jsonAsBytes, jsonAsBytes.Length);                    
        }

        public void AddListener<T>(EventDelegate<T> setInitialPositionsMessage) where T : NetworkMessageBase
        {
            messageHandler.AddListener<T>(setInitialPositionsMessage);
        }

        public void RemoveListener<T>(EventDelegate<T> setInitialPositionsMessage) where T : NetworkMessageBase
        {
            messageHandler.RemoveListener<T>(setInitialPositionsMessage);
        }




        public void Listening()
        {
            try
            {
                while (true)
                {
                    //listen on port
                    var data = client.Receive(ref groupEP);

                    var dataEncodedShouldBeJson = Encoding.UTF8.GetString(data);

                    JObject? complexMessage = JObject.Parse(dataEncodedShouldBeJson);
                    JToken? complexMessageType = complexMessage["type"];

                    Debug.WriteLine("Receiving message...");

                    if (complexMessage != null && complexMessageType?.Type is JTokenType.Integer)
                    {
                        //we have a message that is successfully serialized
                        //the message "Type" is int (enum)
                        //safe to cast
                        MessageType mesType = (MessageType)complexMessageType.Value<int>();
                        JToken? complexMessageMessage = complexMessage["message"];
                        if (complexMessageMessage == null)
                        {
                            return;
                        }

                        NetworkMessageBase networkMessage = null;
                        switch (mesType)
                        {
                            case MessageType.snapshot:
                                networkMessage = complexMessage["message"].ToObject<SnapShot>();
                                Debug.WriteLine("Got a snapshot " + complexMessage);
                                messageHandler.Raise(networkMessage);
                                break;
                            case MessageType.join:
                                networkMessage = complexMessage["message"].ToObject<JoinMessage>();
                                Debug.WriteLine("Got an JoinMessage " + complexMessage);
                                messageHandler.Raise(networkMessage);
                                break;
                            case MessageType.chatUpdate:
                                networkMessage = complexMessage["message"].ToObject<UpdateChat>();
                                messageHandler.Raise(networkMessage);
                                break;
                            case MessageType.sendBoard:
                                networkMessage = complexMessage["message"].ToObject<SendBoard>();
                                break;
                            case MessageType.checkConnection:
                                networkMessage = complexMessage["message"].ToObject<CheckConnection>();
                                messageHandler.Raise(networkMessage);
                                break;
                            case MessageType.changeState:
                                networkMessage = complexMessage["message"].ToObject<ChangeGameState>();
                                messageHandler.Raise(networkMessage);
                                break;
                            case MessageType.receiveOpponentMouse:
                                networkMessage = complexMessage["message"].ToObject<SendMousePos>();
                                messageHandler.Raise(networkMessage);
                                break;
                            case MessageType.turnUpdate:
                                networkMessage = complexMessage["message"].ToObject<TurnUpdate>();
                                messageHandler.Raise(networkMessage);
                                break;
                            default:
                                break;
                        }


                    }

                }
            }
            catch (SocketException e)
            {

                Debug.WriteLine("Socket exception " + e);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
