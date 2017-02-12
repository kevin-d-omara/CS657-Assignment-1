using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    // A Cell represents a single square on the grid
    public class Cell
    {
        public enum Type { Floor, Wall, Pit };

        public readonly Type type;
        public readonly bool blocksMove;
        public readonly bool blocksSonar;
        public readonly char image;

        public Cell(Type type)
        {
            this.type = type;
            switch (type)
            {
                case Type.Floor:
                    blocksMove = false;
                    blocksSonar = false;
                    image = '.';
                    break;
                case Type.Wall:
                    blocksMove = true;
                    blocksSonar = true;
                    image = 'X';
                    break;
                case Type.Pit:
                    blocksMove = true;
                    blocksSonar = false;
                    image = '_';
                    break;
                default:
                    throw new System.ArgumentException("Unsupported Cell.Type");
            }
        }
    }
}
