using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using BattleShips;
using System.Diagnostics;


namespace BattleShips
{
    public class Chat : Component
    {
        public SpriteFont font;
        public static GameWindow gw;
        public static MouseState mouseState;
        public bool enterReleased = true;
        bool myBoxHasFocus = false;
        public bool typing = true;

        public StringBuilder myTextBoxDisplayCharacters = new StringBuilder();
        public Vector2 Pos;

        private User _user;


        public Chat()
        {
            font = GameWorld.Instance.Content.Load<SpriteFont>("chatFont");
        }



        public static void RegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method)
        {
            GameWorld.Instance.Window.TextInput += method;
        }
        public static void UnRegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method)
        {
            GameWorld.Instance.Window.TextInput -= method;
        }
        public void CheckEnterOnMyBox()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && myBoxHasFocus != true && enterReleased)
            {
                enterReleased = false;
                myBoxHasFocus = true;

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Enter) && myBoxHasFocus == true && enterReleased)
            {
                enterReleased = false;
                myBoxHasFocus = false;
                if (_user == null)
                {
                    SetUserName(myTextBoxDisplayCharacters);
                    myTextBoxDisplayCharacters.Clear();
                }
                else
                {
                    SendMessage(myTextBoxDisplayCharacters.ToString());
                    myTextBoxDisplayCharacters.Clear();
                }


            }
            if (myBoxHasFocus)
            {
                RegisterFocusedButtonForTextInput(OnInput);
                typing = true;

            }

            else
                UnRegisterFocusedButtonForTextInput(OnInput);
            if (Keyboard.GetState().IsKeyUp(Keys.Enter))
            {
                enterReleased = true;
            }
        }




        public void OnInput(object sender, TextInputEventArgs e)
        {

            if (typing)
            {
                typing = false;
                var k = e.Key;
                var c = e.Character;

                myTextBoxDisplayCharacters.Append(c);

                myTextBoxDisplayCharacters.Replace("å", "aa");
                myTextBoxDisplayCharacters.Replace("ø", "oe");
                myTextBoxDisplayCharacters.Replace("æ", "ae");

                //if (Keyboard.GetState().IsKeyDown(Keys.Enter) && enterReleased)
                //{
                //    GameWorld.Instance._networkHandler.SendMessageToServer( message)
                //    {
                //        Message = myTextBoxDisplaycharacters
                //    }
                //    //ska flyttes et andet sted hen
                //}
            }


        }


        double updateTimer = 1;

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //GameWorld.Instance.Exit();
            }
            CheckEnterOnMyBox();


            //check for messages
            updateTimer -= GameWorld.DeltaTime;
            if (updateTimer < 0)
            {
                ReceiveMessage();
                updateTimer = 1;
            }



        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (myBoxHasFocus)

                spriteBatch.DrawString(font, "write something", Pos, Color.Red);
            try
            {
                spriteBatch.DrawString(font, $"{myTextBoxDisplayCharacters}", new Vector2(Pos.X - 20, Pos.X + 20), Color.Black);

            }
            catch (ArgumentException)
            {
                if (myTextBoxDisplayCharacters.Length > 1)
                    myTextBoxDisplayCharacters.Length -= 2;
                else
                    myTextBoxDisplayCharacters.Length--;
            }
            if (GameStateController.Instance.currentGameState == WaitingForOpponent.Instance)
            {
                spriteBatch.DrawString(GameWorld.Instance.Font, "Waiting For Opponent", new Vector2(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2, GameWorld.Instance.Graphics.PreferredBackBufferHeight / 2), Color.Black);
            }
                


        }


        /// <summary>
        /// Creats a new instance of the ChatMessage class, sending the string to our server
        /// via the NetworkHandler
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(string message)
        {
            if (_user != null && message.Length >= 2)
            {
                GameWorld.Instance._networkHandler.SendMessageToServer(new ChatMessage()
                {
                    Name = _user.Name,
                    chatMessage = message,

                }, MessageType.chatMessage);
            }


        }


        public void ReceiveMessage()
        {
            GameWorld.Instance._networkHandler.SendMessageToServer(new UpdateChat()
            {

            }, MessageType.chatUpdate);
        }

        public void SetUserName(StringBuilder name)
        {
            User user = GameWorld.Instance.Player.GetComponent<User>() as User;

            if (user != null && name.Length >= 2)
            {
                _user = user;
                _user.Name = name.ToString();

                GameWorld.Instance._networkHandler.SendMessageToServer(new JoinMessage()
                {
                    playerName = _user.Name,
                    ResolutionX = GameWorld.Instance.Graphics.PreferredBackBufferWidth,
                    ResolutionY = GameWorld.Instance.Graphics.PreferredBackBufferHeight,

                }, MessageType.join);
                GameStateController.Instance.ChangeGameState(WaitingForOpponent.Instance);
            }
        }
    }
}
