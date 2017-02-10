using System;
using System​.Numerics;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
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
        public readonly Vector2 startPos;
        public readonly Vector2 goalPos;

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

            // Add Rocks, Pits, etc. to the board.
            PlaceObstacles();
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
                        Position[x, y] = new Cell(Cell.Type.Wall);
                    }
                    else
                    {
                        Position[x, y] = new Cell(Cell.Type.Floor);
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
            validPositions.Remove(startPos);
            validPositions.Remove(goalPos);

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

                Position[(int)randPos.x, (int)randPos.y] = new Cell(randType);
            }
        }

        // Print a text version of the grid.
        public void Display()
        {
            {
                for (int y = 0; y <= height + 1; ++y)
                {
                    for (int x = 0; x <= width + 1; ++x)
                    {
                        Console.Write(Position[x, y].image);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
