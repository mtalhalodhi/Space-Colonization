using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TreeGenTest.SpaceColonization
{
    public class Leaf
    {
        public Vector2 Position { get; set; }
        public Branch Closest { get; set; }

        public Leaf(Vector2 position)
        {
            Position = position;
        }

        public Leaf(float x, float y)
        {
            Position = new Vector2(x, y);
        }
    }
}
