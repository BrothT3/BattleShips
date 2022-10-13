using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class Waiting : IState
    {
        private static Waiting instance;
        public static Waiting Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Waiting();
                }
                return instance;
            }
        }
        public void Enter()
        {

        }

        public void Execute()
        {
            ReceiveInfo();
        }

        public void Exit()
        {

        }

        public void ReceiveInfo()
        {
            GameWorld.Instance._networkHandler.SendMessageToServer(new SendMousePos(), MessageType.receiveOpponentMouse);
        }
    }
}
