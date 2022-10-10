using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShips
{
    public class Destroyer : Ship
    {
        public int CellSpan = 2;
        public Direction Direction { get; set; }
    }
}
