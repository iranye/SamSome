using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataStructures
{
    class Program
    {
        static void Main(String[] args)
        {
            //ReverseNumbers();
            
            const int gridSize = 6;
            Dictionary<Tuple<int, int>, int> nonZeroCoords = new Dictionary<Tuple<int, int>, int>();
            nonZeroCoords.Add(new Tuple<int, int>(0, 0), 1);
            nonZeroCoords.Add(new Tuple<int, int>(0, 1), 1);
            nonZeroCoords.Add(new Tuple<int, int>(0, 2), 1);

            nonZeroCoords.Add(new Tuple<int, int>(1, 1), 1);

            nonZeroCoords.Add(new Tuple<int, int>(2, 0), 1);
            nonZeroCoords.Add(new Tuple<int, int>(2, 1), 1);
            nonZeroCoords.Add(new Tuple<int, int>(2, 2), 1);

            nonZeroCoords.Add(new Tuple<int, int>(3, 2), 2);
            nonZeroCoords.Add(new Tuple<int, int>(3, 3), 4);
            nonZeroCoords.Add(new Tuple<int, int>(3, 4), 4);

            nonZeroCoords.Add(new Tuple<int, int>(4, 3), 2);

            nonZeroCoords.Add(new Tuple<int, int>(5, 2), 1);
            nonZeroCoords.Add(new Tuple<int, int>(5, 3), 2);
            nonZeroCoords.Add(new Tuple<int, int>(5, 4), 4);
            MaxHourGlassSum(gridSize, nonZeroCoords);
        }

        public static void MaxHourGlassSum(int gridSize, Dictionary<Tuple<int, int>, int> nonZeroCoords)
        {
            int[][] grid = GetGrid(gridSize, nonZeroCoords);
            PrintGrid(grid, gridSize);
            return;
            int[][] subGrid;

            const int subGridSize = 2;

            var sb = new StringBuilder();
            List<Tuple<int, int>> coords = new List<Tuple<int, int>>();
            coords.Add(new Tuple<int, int>(0, 0));
            coords.Add(new Tuple<int, int>(0, 1));
            coords.Add(new Tuple<int, int>(1, 0));
            coords.Add(new Tuple<int, int>(1, 1));

            subGrid = GetSubGrid(grid, subGridSize, coords);
            PrintGrid(subGrid, subGridSize);

            coords.Clear();
            coords.Add(new Tuple<int, int>(0, 1));
            coords.Add(new Tuple<int, int>(0, 2));
            coords.Add(new Tuple<int, int>(1, 1));
            coords.Add(new Tuple<int, int>(1, 2));

            subGrid = GetSubGrid(grid, subGridSize, coords);
            PrintGrid(subGrid, subGridSize);

            coords.Clear();
            coords.Add(new Tuple<int, int>(1, 0));
            coords.Add(new Tuple<int, int>(1, 1));
            coords.Add(new Tuple<int, int>(2, 0));
            coords.Add(new Tuple<int, int>(2, 1));

            subGrid = GetSubGrid(grid, subGridSize, coords);
            PrintGrid(subGrid, subGridSize);
        }

        private static int[][] GetGrid(int gridSize, Dictionary<Tuple<int, int>, int> nonZeroCoords)
        {
            int[][] grid = new int[gridSize][];
            for (int i = 0; i < gridSize; i++)
            {
                grid[i] = new int[gridSize];
            }
            foreach (var el in nonZeroCoords)
            {
                grid[el.Key.Item1][el.Key.Item2] = el.Value;
            }
            return grid;
        }

        private static int[][] GetSubGrid(int[][] grid, int subGridSize, List<Tuple<int, int>> coords)
        {
            int[][] subGrid = new int[subGridSize][];
            subGrid[0] = new int[subGridSize];
            subGrid[1] = new int[subGridSize];
            int i = 0;
            int j = 0;
            int row = 0;
            foreach (var el in coords)
            {
                //Console.Write("row: {3}: grid[{0}, {1}] = {2}\r\n", el.Item1, el.Item2, grid[el.Item1][el.Item2], i);
                subGrid[i][j++] = grid[el.Item1][el.Item2];
                if (++row >= subGridSize)
                {
                    i++;
                    j = 0;
                    row = 0;
                }
            }
            return subGrid;
        }

        private static void PrintGrid(int[][] grid, int gridSize)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    sb.Append(grid[i][j]);
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb);
        }

        public static void ReverseNumbers()
        {
            int[] arr;
            int n = GetInputs(out arr);
            foreach (var el in arr.Reverse())
            {
                Console.Write("{0} ", el);
            }
        }

        static int GetInputs(out int[] arr)
        {
            //Console.Write("Enter number of numbers you're entering: ");
            //int n = Convert.ToInt32(Console.ReadLine());
            //Console.WriteLine("Reading {0} numbers", n);
            string[] arrTemp;
            arr = new[] {0};

            try
            {
                arrTemp = Console.ReadLine().Split(' ');
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Failed to read numbers '{0}'", e.Message);
                return 0;
            }

            arr = Array.ConvertAll(arrTemp, Int32.Parse);
            return 0;
        }
    }
}