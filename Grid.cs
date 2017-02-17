﻿using System;
using System​.Numerics;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    // Grid uses Screen-Space coordinates:
    //   _1_2_3_ +X
    // 1|
    // 2| A <--[1,2]
    // 3|
    // +Y

    public class Grid
    {
        public Cell[,] Position { get; private set; }

        // Dimensions discluding buffer.
        public readonly int width;
        public readonly int height;
        
        // Width of the 'wall' cell border.
        public const int buffer = 1;

        // Start Position of Rover, and Goal Position of Rover:
        //      - top left @ [1,1]
        //      - bottom right @ [width, height]
        public readonly Vector2 startPosition;
        public readonly Vector2 goalPosition;

        // Percent of cells with an obstacle (0.0 to 1.0).
        public readonly float obstacleDensity;
        // Obstacle types to randomize between.
        public readonly List<Cell.Type> obstacleTypes;

        // Discludes 'wall' border, 'startPos', and 'endPos'.
        private List<Vector2> validObstaclePositions = new List<Vector2>();

        public Grid(GridParameters gParms)
        {
            width = gParms.width;
            height = gParms.height;
            startPosition = new Vector2(gParms.startPosition.x, gParms.startPosition.y);
            goalPosition = new Vector2(gParms.goalPosition.x, gParms.goalPosition.y);
            obstacleDensity = gParms.obstacleDensity;
            obstacleTypes = new List<Cell.Type>();
            foreach (Cell.Type type in gParms.obstacleTypes)
            {
                obstacleTypes.Add(type);
            }

            // Create outer walls and floors.
            CreateEmptyBoard();

            // Add Rocks, Pits, etc. to the board.
            PlaceObstacles();
        }

        /// <summary>
        /// Create Grid by reading from a file.
        /// </summary>
        /// <param name="filename">Filename, including extension (i.e. Map1.txt 
        /// or Map1.map)</param>
        public Grid(string filename)
        {

        }

        public List<Cell> RaycastFrom(Vector2 initialPosition,
            Bearing initialFacing, Sequence sequence)
        {
            List<Cell> hitCells = new List<Cell>();
            Vector2 pos = initialPosition;
            Bearing facing = initialFacing;
            bool rayBlocked = false;

            Bearing targetBearing = facing.ToBearing(sequence.direction);
            Vector2 offset = targetBearing.ToCoordinateOffset();
            do
            {
                // Calculate coordinates of next Cell to check.
                pos = new Vector2(pos.x + offset.x, pos.y + offset.y);

                hitCells.Add(Position[(int)pos.x, (int)pos.y]);

                // Check if current Cell blocks the sequence.
                switch(sequence.type)
                {
                    case Sequence.Type.Movement:
                        rayBlocked = Position[(int)pos.x, (int)pos.y].blocksMove;
                        break;
                    case Sequence.Type.Sonar:
                        rayBlocked = Position[(int)pos.x, (int)pos.y].blocksSonar;
                        break;
                }

                if (sequence.mode == Sequence.Mode.Terminate)
                {
                    rayBlocked = true;
                }
            } while (!rayBlocked);

            return hitCells;
        }

        // Create a 2D array of 'floor' cells w/ a 'wall' cell border.
        private void CreateEmptyBoard()
        {
            Position = new Cell[width + 2 * buffer, height + 2 * buffer];
            for (int y = 0; y <= height + 1; ++y)
            {
                for (int x = 0; x <= width + 1; ++x)
                {
                    if (x == 0 || x == width + 1 || y == 0 || y == height + 1)
                    {
                        Position[x, y] = new Cell(Cell.Type.Wall, x, y);
                    }
                    else
                    {
                        Position[x, y] = new Cell(Cell.Type.Floor, x, y);
                        validObstaclePositions.Add(new Vector2(x, y));
                    }
                }
            }
        }

        private void PlaceObstacles()
        {
            // Copy list of valid positions to place obstacles.
            List<Vector2> validPositions = new List<Vector2>();
            foreach (Vector2 pos in validObstaclePositions)
            {
                validPositions.Add(new Vector2(pos.x, pos.y));
            }
            validPositions.Remove(startPosition);
            validPositions.Remove(goalPosition);

            // Place obstacles on grid.
            int numObstacles = (int)(width * height * obstacleDensity);
            numObstacles = Math.Min(numObstacles, width * height - 2);
            Random random = new Random();
            for (int i = 0; i < numObstacles; ++i)
            {
                // Select random position on grid.
                int randomIndex = random.Next(0, validPositions.Count);
                Vector2 randPos = validPositions[randomIndex];

                // Prevent position from being used twice.
                validPositions.RemoveAt(randomIndex);

                // Select random obstacle type.
                randomIndex = random.Next(0, obstacleTypes.Count);
                Cell.Type randType = obstacleTypes[randomIndex];

                Position[(int)randPos.x, (int)randPos.y] = new Cell(randType,
                    (int)randPos.x, (int)randPos.y);
            }
        }
    }
}
