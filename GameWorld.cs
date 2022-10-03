using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong;
using System.Collections.Generic;
using System.Diagnostics;

namespace BattleShips
{
    public class GameWorld : Game
    {
        private NetWorkHandler _networkHandler;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private List<GameObject> gameObjects = new List<GameObject>();
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

        protected override void Initialize()
        {

            _networkHandler = new NetWorkHandler(new NetworkMessageBaseEventHandler());
            _networkHandler.AddListener<SetInitialPositionsMessage>(SetInitialPositionsMessage);
            _networkHandler.SendMessageToServer(new JoinMessage()
            {
                playerName = "Daniel",
                ResolutionX = _graphics.PreferredBackBufferWidth,
                ResolutionY = _graphics.PreferredBackBufferHeight,
                type = MessageType.initialJoin
            });


            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Awake();
            }

            base.Initialize();
        }

        private void SetInitialPositionsMessage(SetInitialPositionsMessage initialPositionsMessage)
        {
            //create board and load stuff

            //  _networkHandler.AddListener<SnapShot>(HandleSnapShotMessage);
        }

        private void HandleSnapShotMessage(SnapShot e)
        {
            //maybe where result of action is shown, animation or whatever

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Start();
            }


        }

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
        }

        //draws all gameobjects
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

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
