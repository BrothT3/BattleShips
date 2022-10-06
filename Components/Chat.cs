using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;


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
        StringBuilder myTextBoxDisplayCharacters = new StringBuilder();


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
        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //GameWorld.Instance.Exit();
            }
            CheckEnterOnMyBox();





        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (myBoxHasFocus)
                spriteBatch.DrawString(font, "write something", new Vector2(50, 30), Color.Red);
            try
            {
                spriteBatch.DrawString(font, $"{myTextBoxDisplayCharacters}", new Vector2(30, 50), Color.Black);
            }
            catch (ArgumentException)
            {
                if (myTextBoxDisplayCharacters.Length > 1)
                    myTextBoxDisplayCharacters.Length -= 2;
                else
                    myTextBoxDisplayCharacters.Length--;
            }


        }

    }
}
