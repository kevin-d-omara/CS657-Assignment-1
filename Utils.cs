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
