using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// A container class for generalized utility functions.
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Finds the number of digits in a number.
        /// </summary>
        public static int digitsIn(int n)
        {
            return n == 0 ? 1 : (int)(Math.Log10(n)) + 1;
        }
    }
}
