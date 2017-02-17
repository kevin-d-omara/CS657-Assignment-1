using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class Action
    {
        public enum Type { Move, Rotate, Revert, Wait }

        public Type type;
        public Direction direction;
        public Action(Type type, Direction direction)
        {
            this.type = type;
            this.direction = direction;
        }
    }
}
