namespace InterfacesVsAbstractClasses
{
    public class SquareFromConcrete: ConcreteRegularPolygon
    {
        public SquareFromConcrete(int length) : base(4, length)
        {}

        public override double GetArea()
        {
            return SideLength * SideLength;
        }
    }
}
