using System;
using static System.Console;

namespace InterfacesVsAbstractClasses
{
    // https://app.pluralsight.com/player?course=csharp-interfaces&author=jeremy-clark&name=csharp-interfaces-m1-introduction&clip=3
    public class Program
    {
        static void Main(string[] args)
        {
            //UseConcreteBase();
            //UseAbstractBase();
            UseInterfacedClass();
        }

        private static void UseInterfacedClass()
        {
            var octagon = new OctagonInterfaced(5);
            DisplayImplementedPolygon("Octagon", octagon);
        }

        private static void UseAbstractBase()
        {
            var triangle = new TriangleFromAbstract(5);
            DisplayAbstractPolygon("Triangle From Abstract", triangle);
        }

        private static void UseConcreteBase()
        {
            var square = new SquareFromConcrete(5);
            DisplayConcretePolygon("SquareFromConcrete", square);
        }

        private static void DisplayImplementedPolygon(string polygonType, IRegularPolygon regularPolygon)
        {
            WriteLine(polygonType + ":");
            WriteLine($"NumberofSides:\t{regularPolygon.NumberOfSides}");
            WriteLine($"SideLength:\t{regularPolygon.SideLength}");
            WriteLine($"Perimeter:\t{regularPolygon.GetPerimeter()}");
            WriteLine($"Area:\t{regularPolygon.GetArea()}");
        }

        private static void DisplayAbstractPolygon(string polygonType, AbstractRegularPolygon abstractRegularPolygon)
        {
            WriteLine(polygonType + ":");
            WriteLine($"NumberofSides:\t{abstractRegularPolygon.NumberOfSides}");
            WriteLine($"SideLength:\t{abstractRegularPolygon.SideLength}");
            WriteLine($"Perimeter:\t{abstractRegularPolygon.GetPerimeter()}");
            WriteLine($"Area:\t{abstractRegularPolygon.GetArea()}");
        }

        private static void DisplayConcretePolygon(string polygonType, ConcreteRegularPolygon concreteRegularPolygon)
        {
            WriteLine(polygonType + ":");
            WriteLine($"NumberofSides:\t{concreteRegularPolygon.NumberOfSides}");
            WriteLine($"SideLength:\t{concreteRegularPolygon.SideLength}");
            WriteLine($"Perimeter:\t{concreteRegularPolygon.GetPerimeter()}");
            WriteLine($"Area:\t{concreteRegularPolygon.GetArea()}");
        }
    }
}
