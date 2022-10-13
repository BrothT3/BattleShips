using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BattleShips;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.PortableExecutable;
using System.Text;

namespace BattleShips
{
    public class GameWorld : Game
    {
        public NetWorkHandler _networkHandler;
        public GameObject Player;
        public User User;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public SpriteFont Font;
        public Texture2D pixel;
        private StringBuilder chatLogBuilder = new StringBuilder();
        public Board UpperBoard;
        public Board LowerBoard;

        public List<GameObject> gameObjects = new List<GameObject>();

        private List<GameObject> newGameObjects = new List<GameObject>();
        private List<GameObject> destroyedGameObjects = new List<GameObject>();


        public GraphicsDeviceManager Graphics { get => _graphics; }
        public static float DeltaTime;
        private static GameWorld instance;
        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }

        }
        private GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        public Board lowerB;
        protected override void Initialize()
        {
            //GameObject chatWindow = new GameObject();
            ////SpriteRenderer cwsr = new SpriteRenderer();
            //Chat c = new Chat();
            //chatWindow.Transform.Position = new Vector2(30, 30);
            ////chatWindow.AddComponent(cwsr);
            //chatWindow.AddComponent(c);
            //Instantiate(chatWindow);

            Player = new GameObject();
            Player.AddComponent(new User());
            Instantiate(Player);

            GameObject upperBoard = new GameObject();
            Board upperB = new Board(9, 24, 24, 0);
            SpriteRenderer b1sr = new SpriteRenderer();

            //User user = Player.GetComponent<User>() as User;
            User = Player.GetComponent<User>() as User;




            upperBoard.AddComponent(upperB);
            Instantiate(upperBoard);
            UpperBoard = upperB;

            GameObject lowerBoard = new GameObject();
            lowerB = new Board(9, 24, 24, 10);
            SpriteRenderer b2sr = new SpriteRenderer();
            LowerBoard = lowerB;
            lowerBoard.AddComponent(lowerB);
            Instantiate(lowerBoard);
            User.Board = lowerB.cells;
            _networkHandler = new NetWorkHandler(new NetworkMessageBaseEventHandler());
            _networkHandler.AddListener<SetInitialPositionsMessage>(SetInitialPositionsMessage);
            _networkHandler.AddListener<UpdateChat>(HandleChatUpdate);
            _networkHandler.AddListener<ChangeGameState>(NewGameState);
            _networkHandler.AddListener<SendMousePos>(OpponentMouse);
            _networkHandler.AddListener<TurnUpdate>(HandleTurnUpdate);

            //_networkHandler.AddListener<SendBoard>(RegisterBoard);

            _networkHandler.AddListener<CheckConnection>(ConnectionCheck);

            GameObject chat = new GameObject();
            chat.AddComponent(new Chat() { Pos = new Vector2(300, 50) });
            Instantiate(chat);

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Awake();
            }

            base.Initialize();
        }

        private void HandleTurnUpdate(TurnUpdate turnUpdate)
        {
            if (turnUpdate.Name != null && turnUpdate.Name != User.Name && turnUpdate.HasWon)
            {
                User.HasLost = true;
            }
            if (turnUpdate.Name != null && turnUpdate.Name == User.Name)
            {
                User.YourTurn = turnUpdate.YourTurn;
                User.HasHit = turnUpdate.HasHit;
                GameStateController.Instance.ChangeGameState(YourTurn.Instance);
                User.HasWon = turnUpdate.HasWon;
                User.HasLost = turnUpdate.HasLost;
            }
            else
            {
                GameStateController.Instance.ChangeGameState(Waiting.Instance);
                User.YourTurn = false;
            }
        }
        private void OpponentMouse(SendMousePos receivedMousePos)
        {

            if (receivedMousePos.mousePos != null && receivedMousePos.Name != User.Name)
            {
                string[] split = receivedMousePos.mousePos.Split(' ');
                string tmpX = string.Empty;
                string tmpY = string.Empty;

                for (int i = 0; i < split[0].Length; i++)
                {
                    if (char.IsDigit(split[0][i]))
                    {
                        tmpX += split[0][i];
                    }
                    if (char.IsDigit(split[1][i]))
                    {
                        tmpY += split[1][i];
                    }
                }
                int x = int.Parse(tmpX);
                int y = int.Parse(tmpY) - 10;

                Point point = new Point(x, y);

                foreach (Point c in LowerBoard.cells.Keys)
                {
                    if (c == point)
                    {
                        LowerBoard.cells[c].isHovering = true;
                    }
                    else
                    {
                        LowerBoard.cells[c].isHovering = false;
                    }

                }
            }


        }
        private void NewGameState(ChangeGameState nextGameState)
        {
            User user = Player.GetComponent<User>() as User;
            if (nextGameState.Name == user.Name || nextGameState.Name == string.Empty || nextGameState.Name == null)
            {
                switch (nextGameState.nextGameState)
                {
                    case GameState.placeShips:
                        GameStateController.Instance.ChangeGameState(PlacingShips.Instance);
                        break;
                    case GameState.waitForOpponent:
                        break;
                    case GameState.yourTurn:
                        break;
                    default:
                        break;
                }
            }

        }

        private void SetInitialPositionsMessage(SetInitialPositionsMessage initialPositionsMessage)
        {
            //create board and load stuff

            _networkHandler.AddListener<SnapShot>(HandleSnapShotMessage);
        }

        private void HandleSnapShotMessage(SnapShot e)
        {
            //maybe where result of action is shown, animation or whatever

        }
        string prevMessage;
        /// <summary>
        /// Adds chat message from server to the chatLogBuilder if it's a new message
        /// </summary>
        /// <param name="updateChat"></param>
        private void HandleChatUpdate(UpdateChat updateChat)
        {

            string currentMessage = $"{updateChat.Name}: {updateChat.LastMessage}";

            if (currentMessage != prevMessage)
            {
                if (msgCount >= 10)
                {
                    chatLogBuilder.Clear();
                    msgCount = 0;
                }
                chatLogBuilder.AppendLine(currentMessage);
                msgCount++;
            }
            prevMessage = $"{updateChat.Name}: {updateChat.LastMessage}";

        }
        private void RegisterBoard(SendBoard sendBoard)
        {

        }

        private void ConnectionCheck(CheckConnection checkConnection)
        {
            Debug.WriteLine("Connection checked");
        }

        protected override void LoadContent()
        {

            Font = Content.Load<SpriteFont>("chatFont");

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = Content.Load<Texture2D>("Pixel");
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Start();
            }


        }
        float timer = 2f;
        private int msgCount;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
            }

            base.Update(gameTime);
            //adds and removes new objects
            CleanUp();

            timer -= DeltaTime;
            if (timer < 0)
            {
                User user = Player.GetComponent<User>() as User;

                if (user != null)
                {
                    _networkHandler.SendMessageToServer(new CheckConnection() { Name = user.Name }, MessageType.checkConnection);
                    _networkHandler.SendMessageToServer(new TurnUpdate(), MessageType.turnUpdate);
                }
                timer = 2;
            }
            GameStateController.Instance.UpdateGameState();
        }

        //draws all gameobjects
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            //Skal måske flyttes
            if (chatLogBuilder != null)
            {
                for (int i = 0; i < chatLogBuilder.Length; i++)
                {
                    _spriteBatch.DrawString(Font, chatLogBuilder, new Vector2(300, 100), Color.Black);
                }
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(_spriteBatch);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Adds GameObjects to the newGameObjects list
        /// </summary>
        /// <param name="go"></param>
        public void Instantiate(GameObject go)
        {
            newGameObjects.Add(go);
        }

        /// <summary>
        /// Adds GameObjects to the destroyedGameObjects list 
        /// </summary>
        /// <param name="go"></param>
        public void Destroy(GameObject go)
        {
            destroyedGameObjects.Add(go);
        }

        /// <summary>
        /// Adds new objects to gameObjects list, runs Awake and Start.
        /// while also removing all the objects that are destroyed before clearing both lists
        /// </summary>
        public void CleanUp()
        {
            for (int i = 0; i < newGameObjects.Count; i++)
            {
                gameObjects.Add(newGameObjects[i]);
                newGameObjects[i].Awake();
                newGameObjects[i].Start();

            }

            for (int i = 0; i < destroyedGameObjects.Count; i++)
            {
                gameObjects.Remove(destroyedGameObjects[i]);
            }

            destroyedGameObjects.Clear();
            newGameObjects.Clear();

        }

        /// <summary>
        /// Returns the specified object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Component FindObjectOfType<T>() where T : Component
        {
            foreach (GameObject gameObject in gameObjects)
            {
                Component c = gameObject.GetComponent<T>();

                if (c != null)
                {
                    return c;
                }
            }

            return null;


        }
    }
}
