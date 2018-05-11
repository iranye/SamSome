using System;
using System.Collections.Generic;
using System.Text;

namespace InterfacesVsAbstractClasses
{
    // Regular Polygon = shape w/ 3 or more sides where each side is the same length.
    public class ConcreteRegularPolygon
    {
        public int NumberOfSides { get; set; }
        public int SideLength { get; set; }

        public ConcreteRegularPolygon(int sides, int length)
        {
            NumberOfSides = sides;
            SideLength = length;
        }

        public double GetPerimeter()
        {
            return NumberOfSides * SideLength;
        }

        public virtual double GetArea()
        {
            throw new NotImplementedException();
        }
    }
}
