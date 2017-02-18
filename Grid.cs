using System;
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
        public readonly List<Cell.Type> obstacleTypes = new List<Cell.Type>();

        // Discludes 'wall' border, 'startPos', and 'endPos'.
        private List<Vector2> validObstaclePositions = new List<Vector2>();

        public Grid(GridParameters gParms)
        {
            width = gParms.width;
            height = gParms.height;
            startPosition = new Vector2(gParms.startPosition.x, gParms.startPosition.y);
            goalPosition = new Vector2(gParms.goalPosition.x, gParms.goalPosition.y);
            obstacleDensity = gParms.obstacleDensity;
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
            string[] lines = System.IO.File.ReadAllLines(filename);

            width = lines[0].Length;
            height = lines.Length;
            // Check for end of file newline.
            if (lines[lines.Length - 1].Length == 1) { height -= 1; }
            CreateEmptyBoard();

            int obstacleCount = 0;
            var obstacleTypesFound = new Dictionary<Cell.Type, bool>();
            obstacleTypesFound.Add(Cell.Type.Wall, false);
            obstacleTypesFound.Add(Cell.Type.Pit, false);

            for (int y = 1; y <= height; ++y)
            {
                for (int x = 1; x <= width; ++x)
                {
                    char cell = lines[y - 1][x - 1];

                    // Wall
                    if (cell == '0' || cell == 'X')
                    {
                        ++obstacleCount;
                        Position[x, y] = new Cell(Cell.Type.Wall, x, y);
                        obstacleTypesFound[Cell.Type.Wall] = true;
                    }
                    // Pit
                    else if (cell == '2' || cell == '_')
                    {
                        ++obstacleCount;
                        Position[x, y] = new Cell(Cell.Type.Pit, x, y);
                        obstacleTypesFound[Cell.Type.Pit] = true;
                    }
                    // Rover
                    else if (cell == '8' || cell == 'R')
                    {
                        startPosition = new Vector2(x, y);
                    }
                    // Goal
                    else if (cell == '9' || cell == 'G')
                    {
                        goalPosition = new Vector2(x, y);
                    }
                    // Floor
                    else if (cell == '1' || cell == '.' || cell == ' ') {}
                }
            }

            obstacleDensity = (float)obstacleCount / (float)(width * height);
            foreach (KeyValuePair<Cell.Type, bool> type in obstacleTypesFound)
            {
                if (type.Value == true) { obstacleTypes.Add(type.Key); }
            }

            if (startPosition == null)
            {
                var x = width > 6 ? 6 : 1;
                var y = height > 6 ? 6 : 1;
                startPosition = new Vector2(x, y);
            }
            if (goalPosition == null)
            {
                goalPosition = new Vector2(width, height);
            }
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
