using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// Specifies a pattern of Directions for traversal on a grid.
    /// </summary>
    public class Sequence
    {
        public enum Mode { Repeat, Terminate }
        public enum Type { Movement, Sonar }

        public readonly List<Direction> directions;
        public readonly Mode mode;
        public readonly Type type;

        public Sequence(List<Direction> directions, Mode mode, Type type)
        {
            this.directions = directions;
            this.mode = mode;
            this.type = type;
        }
    }
}
