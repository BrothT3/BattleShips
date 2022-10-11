using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class Destroyer : Ship
    {
        

        public Destroyer()
        {
            CellSpan = 2;
            shipdir = SHIPDIRECTION.UP;
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("Destroyer");
            spriteString = "Destroyer";
            OffSetX = 12;
            OffSetY = 0;
        }
    }
}
