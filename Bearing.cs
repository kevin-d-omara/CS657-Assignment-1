using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// Bearing is the absolute frame of reference. North is up (^) and East is
    /// right (>).
    /// </summary>
    public enum Bearing
    {
        North = 0, NorthEast = 45, East = 90, SouthEast = 135,
        South = 180, SouthWest = 225, West = 270, NorthWest = 315
    }

    /// <summary>
    /// Direction is relative to the Rover's Facing. I.e. is Facing is South,
    /// ForwardRight is SouthWest.
    /// </summary>
    public enum Direction
    {
        Forward = 0, ForwardRight = 45, SideRight = 90, BackwardRight = 135,
        Backward = 180, BackwardLeft = 225, SideLeft = 270, ForwardLeft = 315
    }

    public static class BearingMethods
    {
        /// <summary>
        /// Converts from Bearing Space (North is up, East is right) to Screen
        /// Space (+X is right, +Y is down).
        /// </summary>
        /// <param name="bearing">Bearing to convert to the coordinate offset.</param>
        /// <returns>Vector holding the relative offset.</returns>
        public static Vector2 ToCoordinateOffset(this Bearing bearing)
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

        /// <summary>
        /// Converts from the Rover's initial absolute Bearing and relative
        /// Direction to the final absolute Bearing. I.e. South & ForwardRight
        /// => SouthWest.
        /// </summary>
        /// <param name="bearing">Rover's initial bearing.</param>
        /// <param name="direction">Rover's desired relative direction.</param>
        /// <returns>Bearing to move towards.</returns>
        public static Bearing ToBearing(this Bearing bearing, Direction direction)
        {
            int finalFacing = ((int)bearing + (int)direction) % 360;
            return (Bearing)finalFacing;
        }
    }
}
