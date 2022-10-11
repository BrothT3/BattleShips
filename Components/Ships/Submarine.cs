using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class Submarine : Ship
    {
        public Submarine()
        {
            CellSpan = 3;
            shipdir = SHIPDIRECTION.UP;
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("Submarine");
            spriteString = "Submarine";
            OffSetX = 24;
            OffSetY = -12;
        }
        
    }
}
