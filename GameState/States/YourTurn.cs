using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
        }

        public void Exit()
        {

        }
        public void Fire()
        {
            mstate = Mouse.GetState();
            if (mstate.LeftButton == ButtonState.Pressed && MReleased == true && GameWorld.Instance.UpperBoard.BoardSpace.Contains(new Vector2(mstate.X, mstate.Y)) && Fired == false)
            {
                Fired = true;
                MReleased = false;
                Cell selectedCell = GameWorld.Instance.UpperBoard.board.Find(x => x.isHovering == true);
                if (selectedCell.IsOccupied)
                {
                    HitCells++;
                    TargetHit = true;
                    selectedCell.isFiredOnAndShipHit = true;
                }
                else
                {
                    TargetHit = false;
                    selectedCell.isFiredOn = true;
                }
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
