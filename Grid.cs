using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class Grid
    {
        // dimensions discluding buffer
        public readonly int width;
        public readonly int height;
        
        // width of the 'wall' cell border
        public const int buffer = 1;

        // positions discluding buffer
        public readonly Vector2 startPos;
        public readonly Vector2 goalPos;

        public readonly float obstacleDensity;
        public readonly List<Cell.Type> obstacleTypes;

        public Grid(GridParameters gParms)
        {
            width = gParms.width;
            height = gParms.height;
            startPos = new Vector2(gParms.startPos.x, gParms.startPos.y);
            goalPos = new Vector2(gParms.goalPos.x, gParms.goalPos.y);
            obstacleDensity = gParms.obstacleDensity;
            obstacleTypes = new List<Cell.Type>();
            foreach (Cell.Type type in gParms.obstacleTypes)
            {
                obstacleTypes.Add(type);
            }
        }
    }
}
