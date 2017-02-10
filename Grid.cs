using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class Grid
    {
        // Dimensions discluding buffer.
        public readonly int width;
        public readonly int height;
        
        // Width of the 'wall' cell border.
        public const int buffer = 1;

        // Positions discluding buffer: top left @ [1,1]
        //                              bottom right @ [width, height]
        public readonly Vector2 startPos;
        public readonly Vector2 goalPos;

        // Percent of cells with an obstacle (0.0 to 1.0).
        public readonly float obstacleDensity;
        // Obstacle types to randomize between.
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

            // Create outer walls and floors.
            CreateEmptyBoard();

            // Create a list including valid board positions, excludes outer
            // wall, start position, and end position.
            InitializeValidMoves();

            // Add Rocks, Pits, etc. to the board.
            PlaceObstacles();
        }

        private void CreateEmptyBoard()
        {

        }

        private void InitializeValidMoves()
        {

        }

        private void PlaceObstacles()
        {

        }
    }
}
