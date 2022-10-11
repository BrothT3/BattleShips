using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

public enum SHIPDIRECTION { UP, RIGHT}
namespace BattleShips
{
    public class Ship : Component
    {
        public SHIPDIRECTION shipdir { get; set; }
        public int CellSpan { get; set; }
        public Texture2D Sprite { get; set; }
        public string spriteString { get; set; }
        public int OffSetY { get; set; } 
        public int OffSetX { get; set; } 
    }
}
