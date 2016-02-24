using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralPractice
{
    internal class Vector2
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }
    }
}
