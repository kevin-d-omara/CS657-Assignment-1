using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    // Bearing is the absolute frame of reference.
    // North is up (^) and East is right (>).
    public enum Bearing
    {
        North = 0, NorthEast = 45, East = 90, SouthEast = 135,
        South = 180, SouthWest = 225, West = 270, NorthWest = 315
    }

    // Direction is relative to the Rover's Facing.
    // I.e. if Facing is South, FrontRight is SouthWest.
    public enum Direction
    {
        Front = 0, FrontRight = 45, Right = 90, BackRight = 135,
        Back = 180, BackLeft = 225, Left = 270, FrontLeft = 315
    }

    public static class Utility
    {
        public static Bearing TempName(Bearing bearing, Direction direction)
        {
            //int finalFacing = (bearing + direction);
            return Bearing.North;
        }

        // Convert from Bearing Space (North is up)
        //           to Screen Space  (-X-axis is up)
        public static Vector2 ConvertBearingToCoordinateOffset(Bearing bearing)
        {
            Vector2 coordinateOffset;
            switch (bearing)
            {
                case Bearing.North:
                    coordinateOffset = new Vector2(0, -1);
                    break;
                case Bearing.NorthEast:
                    coordinateOffset = new Vector2(1, -1);
                    break;
                case Bearing.East:
                    coordinateOffset = new Vector2(1, 0);
                    break;
                case Bearing.SouthEast:
                    coordinateOffset = new Vector2(1, 1);
                    break;
                case Bearing.South:
                    coordinateOffset = new Vector2(0, 1);
                    break;
                case Bearing.SouthWest:
                    coordinateOffset = new Vector2(-1, 1);
                    break;
                case Bearing.West:
                    coordinateOffset = new Vector2(-1, 0);
                    break;
                case Bearing.NorthWest:
                    coordinateOffset = new Vector2(-1, -1);
                    break;
                default:
                    Console.WriteLine("Bearing unsupported.");
                    coordinateOffset = null;
                    break;
            }
            return coordinateOffset;
        }
    }
}
