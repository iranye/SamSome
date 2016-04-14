using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathProblem
{
    class Program
    {
        private static int mMaxMoves;
        private static int mBoundary;
        private static int[,] mGrid;
        private static List<Tuple<int, int>> mVisited = new List<Tuple<int, int>>();
        private const int ILLEGAL_MOVE = -1;
        
        static void Main(string[] args)
        {
            initalizePuzzleSmall();
            //initalizePuzzleMedium();
            //initalizePuzzleMain();
            int maxSum = 0;

            for (int i = 0; i < mBoundary; i++)
            {
                for (int j = 0; j < mBoundary; j++)
                {
                    int sum = getPathSum(new Tuple<int, int>(i, j), 1);
                    if (maxSum < sum)
                    {
                        maxSum = sum;
                    }
                    mVisited.Clear();
                }
            }
            Console.WriteLine("maxSum: {0}", maxSum);
        }

        private static bool MoveIsLegal(Tuple<int, int> position)
        {
            return !(
                       (position.Item1 < 0) || (position.Item2 < 0)
                    || (position.Item1 >= mBoundary) || (position.Item2 >= mBoundary)
                    || (mVisited.Contains(position))
                    );
        }

        //private static int getPathSum(int i, int j, int moves)
        private static int getPathSum(Tuple<int, int> position, int moves)
        {
            mVisited.Add(position);

            if (moves >= mMaxMoves)
            {
                return mGrid[position.Item1, position.Item2];
            }
            List<Tuple<int, int>> legalMoves = new List<Tuple<int, int>>();
            var right = new Tuple<int, int>(position.Item1, position.Item2 + 1);
            var left  = new Tuple<int, int>(position.Item1, position.Item2 - 1);
            var up    = new Tuple<int, int>(position.Item1 - 1, position.Item2);
            var down  = new Tuple<int, int>(position.Item1 + 1, position.Item2);
            int rightSum = -1;
            if (MoveIsLegal(right))
            {
                legalMoves.Add(right);
                rightSum = getPathSum(right, moves + 1);
            }
            int leftSum = -1;
            if (MoveIsLegal(left))
            {
                legalMoves.Add(left);
                leftSum = getPathSum(left, moves + 1);
            }
            int upSum = -1;
            if (MoveIsLegal(up))
            {
                legalMoves.Add(up);
                upSum = getPathSum(up, moves + 1);
            }
            int downSum = -1;
            if (MoveIsLegal(down))
            {
                legalMoves.Add(down);
                downSum = getPathSum(down, moves + 1);
            }
            return mGrid[position.Item1, position.Item2] + MaxSum(rightSum, leftSum, upSum, downSum);
        }

        private static int MaxSum(int rightSum, int leftSum, int upSum, int downSum)
        {
            int lateralMax = Math.Max(rightSum, leftSum);
            int verticalMax = Math.Max(upSum, downSum);
            return Math.Max(lateralMax, verticalMax);
        }

        private static void initalizePuzzleSmall()
        {
            mMaxMoves = 3;
            mBoundary = 2;
            mGrid = new int[mBoundary, mBoundary];
            mGrid[0, 0] = 5;
            mGrid[0, 1] = 8;
            mGrid[1, 0] = 6;
            mGrid[1, 1] = 4;
        }

        private static void initalizePuzzleMedium()
        {
            mMaxMoves = 4;
            mBoundary = 3;
            mGrid = new int[mBoundary, mBoundary];
            mGrid[0, 0] = 1;
            mGrid[0, 1] = 2;
            mGrid[0, 2] = 5;
            mGrid[1, 0] = 1;
            mGrid[1, 1] = 3;
            mGrid[1, 2] = 4;
            mGrid[2, 0] = 1;
            mGrid[2, 1] = 1;
            mGrid[2, 2] = 1;
        }

        private static void initalizePuzzleMain()
        {
            mMaxMoves = 40;
            mBoundary = 12;
            mGrid = new int[mBoundary, mBoundary];
            mGrid[0, 0] = 45;
            mGrid[0, 1] = 16;
            mGrid[0, 2] = 49;
            mGrid[0, 3] = 17;
            mGrid[0, 4] = 40;
            mGrid[0, 5] = 18;
            mGrid[0, 6] = 51;
            mGrid[0, 7] = 18;
            mGrid[0, 8] = 41;
            mGrid[0, 9] = 10;
            mGrid[0, 10] = 53;
            mGrid[0, 11] = 10;

            mGrid[1, 0] = 16;
            mGrid[1, 1] = 46;
            mGrid[1, 2] = 16;
            mGrid[1, 3] = 42;
            mGrid[1, 4] = 19;
            mGrid[1, 5] = 50;
            mGrid[1, 6] = 12;
            mGrid[1, 7] = 42;
            mGrid[1, 8] = 17;
            mGrid[1, 9] = 45;
            mGrid[1, 10] = 10;
            mGrid[1, 11] = 37;

            mGrid[2, 0] = 46;
            mGrid[2, 1] = 13;
            mGrid[2, 2] = 42;
            mGrid[2, 3] = 19;
            mGrid[2, 4] = 43;
            mGrid[2, 5] = 12;
            mGrid[2, 6] = 51;
            mGrid[2, 7] = 18;
            mGrid[2, 8] = 47;
            mGrid[2, 9] = 13;
            mGrid[2, 10] = 43;
            mGrid[2, 11] = 19;

            mGrid[3, 0] = 14;
            mGrid[3, 1] = 43;
            mGrid[3, 2] = 19;
            mGrid[3, 3] = 40;
            mGrid[3, 4] = 18;
            mGrid[3, 5] = 48;
            mGrid[3, 6] = 10;
            mGrid[3, 7] = 42;
            mGrid[3, 8] = 13;
            mGrid[3, 9] = 42;
            mGrid[3, 10] = 12;
            mGrid[3, 11] = 47;

            mGrid[4, 0] = 47;
            mGrid[4, 1] = 16;
            mGrid[4, 2] = 40;
            mGrid[4, 3] = 20;
            mGrid[4, 4] = 42;
            mGrid[4, 5] = 17;
            mGrid[4, 6] = 53;
            mGrid[4, 7] = 19;
            mGrid[4, 8] = 49;
            mGrid[4, 9] = 17;
            mGrid[4, 10] = 41;
            mGrid[4, 11] = 19;

            mGrid[5, 0] = 13;
            mGrid[5, 1] = 47;
            mGrid[5, 2] = 12;
            mGrid[5, 3] = 41;
            mGrid[5, 4] = 19;
            mGrid[5, 5] = 42;
            mGrid[5, 6] = 10;
            mGrid[5, 7] = 49;
            mGrid[5, 8] = 10;
            mGrid[5, 9] = 46;
            mGrid[5, 10] = 13;
            mGrid[5, 11] = 52;

            mGrid[6, 0] = 48;
            mGrid[6, 1] = 12;
            mGrid[6, 2] = 47;
            mGrid[6, 3] = 23;
            mGrid[6, 4] = 46;
            mGrid[6, 5] = 18;
            mGrid[6, 6] = 42;
            mGrid[6, 7] = 18;
            mGrid[6, 8] = 49;
            mGrid[6, 9] = 19;
            mGrid[6, 10] = 43;
            mGrid[6, 11] = 18;

            mGrid[7, 0] = 12;
            mGrid[7, 1] = 43;
            mGrid[7, 2] = 19;
            mGrid[7, 3] = 39;
            mGrid[7, 4] = 20;
            mGrid[7, 5] = 42;
            mGrid[7, 6] = 11;
            mGrid[7, 7] = 40;
            mGrid[7, 8] = 23;
            mGrid[7, 9] = 48;
            mGrid[7, 10] = 11;
            mGrid[7, 11] = 53;

            mGrid[8, 0] = 49;
            mGrid[8, 1] = 17;
            mGrid[8, 2] = 40;
            mGrid[8, 3] = 18;
            mGrid[8, 4] = 48;
            mGrid[8, 5] = 17;
            mGrid[8, 6] = 42;
            mGrid[8, 7] = 21;
            mGrid[8, 8] = 52;
            mGrid[8, 9] = 12;
            mGrid[8, 10] = 44;
            mGrid[8, 11] = 11;

            mGrid[9, 0] = 11;
            mGrid[9, 1] = 42;
            mGrid[9, 2] = 18;
            mGrid[9, 3] = 46;
            mGrid[9, 4] = 21;
            mGrid[9, 5] = 46;
            mGrid[9, 6] = 12;
            mGrid[9, 7] = 39;
            mGrid[9, 8] = 13;
            mGrid[9, 9] = 47;
            mGrid[9, 10] = 10;
            mGrid[9, 11] = 53;

            mGrid[10, 0] = 50;
            mGrid[10, 1] = 13;
            mGrid[10, 2] = 42;
            mGrid[10, 3] = 11;
            mGrid[10, 4] = 43;
            mGrid[10, 5] = 19;
            mGrid[10, 6] = 43;
            mGrid[10, 7] = 17;
            mGrid[10, 8] = 43;
            mGrid[10, 9] = 11;
            mGrid[10, 10] = 46;
            mGrid[10, 11] = 10;

            mGrid[11, 0] = 10;
            mGrid[11, 1] = 48;
            mGrid[11, 2] = 13;
            mGrid[11, 3] = 48;
            mGrid[11, 4] = 19;
            mGrid[11, 5] = 51;
            mGrid[11, 6] = 18;
            mGrid[11, 7] = 42;
            mGrid[11, 8] = 10;
            mGrid[11, 9] = 54;
            mGrid[11, 10] = 19;
            mGrid[11, 11] = 41;
        }
    }
}
