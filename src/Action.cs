using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class Action
    {
        public enum Type { Move, Rotate, Revert, Wait, Start }

        public Type type;
        public Direction direction;
        public Action(Type type, Direction direction)
        {
            this.type = type;
            this.direction = direction;
        }

        public override string ToString()
        {
            switch (type)
            {
                case Action.Type.Move:
                    return "Move";
                case Action.Type.Rotate:
                    return "Rotate";
                case Action.Type.Revert:
                    return "Revert";
                case Action.Type.Wait:
                    return "Wait";
                case Action.Type.Start:
                    return "Start";
                default:
                    return "!Undefined!";
            }
        }
    }
}
