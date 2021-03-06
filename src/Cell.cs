﻿using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    // A Cell represents a single square on the grid
    public class Cell
    {
        public enum Type { Floor, Wall, Pit, Shroud };

        public readonly Type type;
        public readonly bool blocksMove;
        public readonly bool blocksSonar;
        public readonly char image;
        public readonly int x;
        public readonly int y;

        public Cell(Type type, int x, int y)
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
                    image = 'O';
                    break;
                case Type.Shroud:
                    blocksMove = false;
                    blocksSonar = true;
                    image = 'S';
                    break;
                default:
                    throw new System.ArgumentException("Unsupported Cell.Type");
            }
            this.x = x;
            this.y = y;
        }
    }
}
