using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class Loss : IState
    {
        private static Loss instance;
        public static Loss Instance { get { if (instance == null) { instance = new Loss(); } return instance; } }
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
