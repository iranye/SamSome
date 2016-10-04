using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmazonBoxes
{
    public class Program
    {
        static void Main(string[] args)
        {
            var sb = new StringBuilder();
            int cityHorzDistance = 3;
            int cityVertDistance = 5;
            var grid = GetLockerDistanceGrid(cityHorzDistance, cityVertDistance, new int[] {1, 2}, new int[] {1, 3});
            for (int i = 0; i < cityVertDistance; i++)
            {
                for (int j = 0; j < cityHorzDistance; j++)
                {
                    sb.Append(grid[i][j]);
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb);
        }

        public static int[][] GetLockerDistanceGrid(int cityHorzDistance, int cityVertDistance, int[] lockerXCoordinates,
            int[] lockerYCoordinates)
        {
            int[][] grid = new int[cityVertDistance][];
            for (int i = 0; i < cityVertDistance; i++)
            {
                grid[i] = new int[cityHorzDistance];
            }

            for (int vert = 0; vert < cityVertDistance; vert++)
            {
                int vPos = vert + 1;
                for (int horz = 0; horz < cityHorzDistance; horz++)
                {
                    int hPos = horz + 1;
                    int moveMin = 10;

                    for (int i = 0; i < lockerXCoordinates.Length; i++)
                    {
                        int vMove = Math.Abs(vPos - lockerYCoordinates[i]);
                        int hMove = Math.Abs(hPos - lockerXCoordinates[i]);
                        int totalMove = vMove + hMove;
                        if (totalMove < moveMin)
                        {
                            moveMin = totalMove;
                        }
                        grid[vert][horz] = moveMin;
                    }
                }
            }
            return grid;
        }
    }
}
