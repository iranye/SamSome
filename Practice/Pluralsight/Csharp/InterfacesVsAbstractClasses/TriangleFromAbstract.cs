using System;
using System.Collections.Generic;
using System.Text;

namespace InterfacesVsAbstractClasses
{
    public class TriangleFromAbstract : AbstractRegularPolygon
    {
        public TriangleFromAbstract(int length) : base(3, length)
        {
        }

        public override double GetArea()
        {
            return SideLength * SideLength * Math.Sqrt(3) / 4;
        }
    }
}
