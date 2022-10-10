using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class WaitingForOpponent : IState
    {
        private static WaitingForOpponent instance;

        public static WaitingForOpponent Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WaitingForOpponent();
                }
                return instance;
            }
        }

        public void Enter()
        {

        }

        public void Execute()
        {

        }

        public void Exit()
        {

        }

    }
}
