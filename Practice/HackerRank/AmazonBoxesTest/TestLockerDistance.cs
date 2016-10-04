using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using AmazonBoxes;

namespace AmazonBoxesTest
{
    [TestClass]
    public class TestLockerDistance
    {
        [TestMethod]
        public void TestGetLockerDistanceGrid_SingeLocker()
        {
            var sb = new StringBuilder();
            int cityHorzDistance = 3;
            int cityVertDistance = 5;
            var grid = Program.GetLockerDistanceGrid(cityHorzDistance, cityVertDistance, new[] { 1 }, new[] { 1 });
            for (int i = 0; i < cityVertDistance; i++)
            {
                for (int j = 0; j < cityHorzDistance; j++)
                {
                    sb.Append(grid[i][j]);
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb);

            // TODO: Must match this grid:
            //grid[0][0] = 0;
            //grid[0][1] = 1;
            //grid[0][2] = 2;

            //grid[1][0] = 1;
            //grid[1][1] = 2;
            //grid[1][2] = 3;

            //grid[2][0] = 2;
            //grid[2][1] = 3;
            //grid[2][2] = 4;

            //grid[3][0] = 3;
            //grid[3][1] = 4;
            //grid[3][2] = 5;

            //grid[4][0] = 4;
            //grid[4][1] = 5;
            //grid[4][2] = 6;
        }

        [TestMethod]
        public void TestGetLockerDistanceGrid_MultipleLockers()
        {
            var sb = new StringBuilder();
            int cityHorzDistance = 3;
            int cityVertDistance = 5;
            var grid = Program.GetLockerDistanceGrid(cityHorzDistance, cityVertDistance, new[] { 1, 2 }, new[] { 1, 3 });
            for (int i = 0; i < cityVertDistance; i++)
            {
                for (int j = 0; j < cityHorzDistance; j++)
                {
                    sb.Append(grid[i][j]);
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb);

            // TODO: Must match this grid:
            //012
            //112
            //101
            //212
            //323
        }

        [TestMethod]
        public void TestGetLockerDistanceGrid_BigGrid_MultipleLockers()
        {
            var sb = new StringBuilder();
            int cityHorzDistance = 5;
            int cityVertDistance = 7;
            var grid = Program.GetLockerDistanceGrid(cityHorzDistance, cityVertDistance, new[] { 2, 4 }, new[] { 3, 7 });
            for (int i = 0; i < cityVertDistance; i++)
            {
                for (int j = 0; j < cityHorzDistance; j++)
                {
                    sb.Append(grid[i][j]);
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb);

            // TODO: Must match this grid:
            //32345
            //21234
            //10123
            //21234
            //32323
            //43212
            //32101
        }

    }
}
