using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BattleShips
{
    public class YourTurn : IState
    {
        private static YourTurn instance;
        private MouseState mstate;
        public int HitCells { get; set; } = 0;
        public bool TargetHit { get; set; } = false;
        public bool Fired { get; set; } = false;
        public bool MReleased { get; set; } = true;
        public float FireTimer { get; set; } = 2;
        public static YourTurn Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new YourTurn();
                }
                return instance;
            }
        }



        public void Enter()
        {

        }

        public void Execute()
        {
            Fire();
            VictoryCondition();
            SendMouseInfo();
            if (!GameWorld.Instance.User.YourTurn)
            {
                ReceiveInfo();
            }
          
        }

        public void Exit()
        {

        }

        public void SendMouseInfo()
        {
            try
            {
                Cell? selectedCell = GameWorld.Instance.UpperBoard.board.Find(x => x.isHovering == true);
                if (selectedCell != null)
                {
                    GameWorld.Instance._networkHandler.SendMessageToServer(new SendMousePos()
                    {
                        mousePos = new Point((int)(selectedCell.Position.X + 0.5f), (int)(selectedCell.Position.Y + 0.5f + 10)).ToString(),
                        Name = GameWorld.Instance.User.Name
                    }, MessageType.sendMouseInfo);
                }
            }
            catch (Exception)
            {

               
            }
                       
        }

        public void ReceiveInfo()
        {
            GameWorld.Instance._networkHandler.SendMessageToServer(new SendMousePos(), MessageType.receiveOpponentMouse);
        }

        public void Fire()
        {
            mstate = Mouse.GetState();
            if (mstate.LeftButton == ButtonState.Pressed && MReleased == true && GameWorld.Instance.UpperBoard.BoardSpace.Contains(new Vector2(mstate.X, mstate.Y)) && Fired == false)
            {
                Fired = true;
                MReleased = false;
                Cell selectedCell = GameWorld.Instance.UpperBoard.board.Find(x => x.isHovering == true);

                GameWorld.Instance._networkHandler.SendMessageToServer(new SendShotAttempt()
                {
                    Name = GameWorld.Instance.User.Name,
                    MousePos = new Point((int)(selectedCell.Position.X + 0.5f), (int)(selectedCell.Position.Y + 0.5f + 10)).ToString(),
                    HasFired = true

                }, MessageType.shoot);

                if (GameWorld.Instance.User.HasHit)
                {
                    HitCells++;
                    GameWorld.Instance.User.HasHit = false;
                    selectedCell.isFiredOnAndShipHit = true;
                }

                //if (selectedCell.IsOccupied)
                //{
                //    HitCells++;
                //    TargetHit = true;
                //    selectedCell.isFiredOnAndShipHit = true;
                //}
                //else
                //{
                //    TargetHit = false;
                //    selectedCell.isFiredOn = true;
                //}
            }
            if (mstate.LeftButton != ButtonState.Pressed)
            {
                MReleased = true;
            }
            if (Fired)
            {
                FireTimer -= GameWorld.DeltaTime;
                if (FireTimer <= 0)
                {
                    Fired = false;
                    FireTimer = 1.2f;
                }
            }
        }
        public void VictoryCondition()
        {
            if (HitCells == 17)
            {
                //victory state eller noget
                
            }
        }

    }
}
