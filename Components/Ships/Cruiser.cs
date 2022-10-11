using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class Cruiser : Ship
    {
        public Cruiser()
        {
            CellSpan = 3;
            shipdir = SHIPDIRECTION.UP;
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("Cruiser");
            spriteString = "Cruiser";
            OffSetX = 24;
            OffSetY = -12;
        }
    }
}
