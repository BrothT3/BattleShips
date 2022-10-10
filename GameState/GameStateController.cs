using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips 
{ 
    public class GameStateController
    {
        public IState currentGameState;

        private static GameStateController instance;
        public static GameStateController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameStateController();
                }
                return instance;
            }

        }

       
        public void ChangeGameState(IState nextGameState)
        {
            if (currentGameState != null)
            {
                currentGameState.Exit();
            }
            currentGameState = nextGameState;

            currentGameState.Enter();


        }

        public void UpdateGameState()
        {
            if (currentGameState != null)
            {
                currentGameState.Execute();
            }

        }
    }
}
