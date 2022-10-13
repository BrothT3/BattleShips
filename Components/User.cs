using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BattleShips
{
    [Serializable]
    public class User : Component
    {
        public string Name { get; set; }
        public bool YourTurn { get; set; }

        public Dictionary<Point, Cell> Board = new Dictionary<Point, Cell>();

        public bool isReady { get; set; }
        public bool HasFired { get;set; }
        public bool HasHit { get; set; }
        public bool HasWon { get; set; }
        public bool HasLost { get; set; }
    }
}

