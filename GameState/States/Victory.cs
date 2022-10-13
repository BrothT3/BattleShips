using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class Victory : IState
    {
        private static Victory instance;
        public static Victory Instance { get { if (instance == null) { instance = new Victory(); } return instance; } }
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
