using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TreeGenTest.SpaceColonization
{
    public class Branch
    {
        public Branch Parent { get; set; }
        
        public Vector2 Position { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 OriginalDirection { get; set; }
        public float Thickness = 1;

        public int GrowCount { get; set; }

        public Branch(Branch parent, Vector2 position, Vector2 direction)
        {
            Parent = parent;
            Position = position;
            Direction = direction;
        }

        public void Reset()
        {
            GrowCount = 0;
            Direction = OriginalDirection;
        }
    }
}
