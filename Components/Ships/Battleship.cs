using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class Battleship : Ship
    {
        public Battleship()
        {
            CellSpan = 4;
            shipdir = SHIPDIRECTION.UP;
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("BattleShip");
            spriteString = "BattleShip";
            OffSetX = 36;
            OffSetY = -24;
        }
    }
}
