using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// A container class for generalized utility functions.
    /// </summary>
    public static class Utils
    {
        public const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
        public const string alphaChars = "0123456789";
        public const string hexChars = "0123456789abcdef";

        private static Random random = new Random();

        /// <summary>
        /// The Manhattan Distance on a grid is the defined by making cost=1
        /// steps between adjacent squares. Note: diagonal movement is
        /// restricted.
        /// </summary>
        public static int ManhattanDistance(Cell a, Cell b)
        {
            return Math.Abs((int)b.x - (int)a.x)
                 + Math.Abs((int)b.y - (int)a.y);
        }

        /// <summary>
        /// The Chebyshev Distance on a grid is defined by making cost=1 steps
        /// between adjacent AND diagonal squares. That is, movement allowed
        /// by a King in Chess.
        /// </summary>
        public static int ChebyshevDistance(Cell a, Cell b)
        {
            return Math.Max((Math.Abs((int)b.x - (int)a.x)),
                            (Math.Abs((int)b.y - (int)a.y)));
        }

        /// <summary>
        /// Always returns zero. To be used as a "null" search heuristic.
        /// </summary>
        public static int ZeroDistance(Cell a, Cell b) { return 0; }

        /// <summary>
        /// Finds the number of digits in a number.
        /// </summary>
        public static int digitsIn(int n)
        {
            return n == 0 ? 1 : (int)(Math.Log10(n)) + 1;
        }

        /// <summary>
        /// Creates an randomized string consisting of characters from the 
        /// provided character pool.
        /// </summary>
        /// <param name="charSet">Set of characters to randomize between.
        /// </param>
        public static string randomString(string charSet, int length)
        {
            string randStr = "";

            for (int i = 0; i < length; ++i)
            {
                randStr += charSet[random.Next(charSet.Length)];
            }

            return randStr;
        }
    }
}
