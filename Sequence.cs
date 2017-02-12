using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// Specifies a pattern of Direction for traversal on a grid.
    /// </summary>
    public class Sequence
    {
        public enum Mode { Repeat, Terminate }
        public enum Type { Movement, Sonar }

        public readonly Direction direction;
        public readonly Mode mode;
        public readonly Type type;

        public Sequence(Direction direction, Mode mode, Type type)
        {
            this.direction = direction;
            this.mode = mode;
            this.type = type;
        }
    }
}
