using System;
using System.Collections.Generic;
using System.Text;
enum Direction { UP, RIGHT }
namespace BattleShips
{
    
    public class PlacingShips : IState
    {
        
        public void Enter()
        {

        }

        public void Execute()
        {

        }

        public void Exit()
        {
            //send board to server, not sure if it should be here
            User user = GameWorld.Instance.Player.GetComponent<User>() as User;
            if (user != null)
            {
                GameWorld.Instance._networkHandler.SendMessageToServer(new SendBoard()
                {
                    Name = user.Name,
                    Board = user.Board

                }, MessageType.sendBoard);;
            }        
        }

        public void PlaceShip()
        {

        }
    }
}
