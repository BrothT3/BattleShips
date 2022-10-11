using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class Carrier : Ship
    {
        public Carrier()
        {
            CellSpan = 5;
            shipdir = SHIPDIRECTION.UP;
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("Carrier");
            spriteString = "Carrier";
            OffSetX = 48;
            OffSetY = -36;
        }
    }
}
