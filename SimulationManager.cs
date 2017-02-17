using System;
using System.Threading;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// Source of the main "simulation loop".
    /// Asks for user input of grid & rover parameters.
    /// </summary>
    public class SimulationManager
    {
        public const int MoveLimit = 400;

        private Grid grid;
        private Rover rover;
        private bool noPathFound = false;

        public SimulationManager()
        {
            Rover.OnUsedSonar += DisplayProgress;
            Rover.OnNoPathFound += FlagNoPathFound;
        }
        ~SimulationManager() { Rover.OnUsedSonar -= DisplayProgress; }

        public void StartSimulation(string environmentFile, string outputFile)
        {
            // Create Grid.
            if (environmentFile == "n/a")
            {
                // Default or Manual entry.
                var gridParameters = GetGridParameters();
                grid = new Grid(gridParameters);
            }
            else
            {
                // Create from File.
                grid = new Grid(environmentFile);
            }

            // Create Rover.
            RoverParameters roverParameters = GetRoverParameters();
            rover = new Rover(roverParameters, grid);

            EnterMainLoop();

            // TODO: print results to output file
        }

        private void EnterMainLoop()
        {
            // Simulation Loop
            while (true)
            {
                rover.Update();

                // Exit conditions.
                if (rover.Position.Equals(grid.goalPosition))
                {
                    DisplayProgress();
                    Console.WriteLine("-----> Goal Reached!");
                    break;
                }
                if (rover.MoveCount >= MoveLimit)
                {
                    DisplayProgress();
                    Console.WriteLine("-----> Move Limit Reached.");
                    break;
                }
                if (noPathFound)
                {
                    DisplayProgress();
                    Console.WriteLine("-----> No Path Found!");
                    break;
                }
            }
        }

        private GridParameters GetGridParameters()
        {
            GridParameters gridParams;

            Console.WriteLine("Enter Grid Parameters: ");
            Console.WriteLine("D) Default");
            Console.WriteLine("M) Manual");
            char choice = Convert.ToChar(Console.ReadLine());
            if (choice == 'm' || choice == 'M')
            {
                gridParams = ManuallyEnterGridParameters();
            }
            else
            {
                gridParams = new GridParameters();
            }

            return gridParams;
        }

        private GridParameters ManuallyEnterGridParameters()
        {
            var gridParams = new GridParameters();

            Console.WriteLine("Height: ");
            Console.WriteLine("Width: ");
            Console.WriteLine("Start Position (x y): ");
            Console.WriteLine("Goal  Position (x y): ");
            Console.WriteLine("Obstacle Density (0.0 to 1.0): ");
            Console.WriteLine("Obstacle Types (y or n):");
            Console.WriteLine("                Wall - ");
            Console.WriteLine("                Pit  - ");

            Console.SetCursorPosition(8, Console.CursorTop - 8);
            gridParams.height = Convert.ToInt32(Console.ReadLine());

            Console.SetCursorPosition(7, Console.CursorTop);
            gridParams.width = Convert.ToInt32(Console.ReadLine());

            Console.SetCursorPosition(22, Console.CursorTop);
            string[] position = Console.ReadLine().Split(null);
            gridParams.startPosition = new Vector2(
                (float)Convert.ToDouble(position[0]),
                (float)Convert.ToDouble(position[0]));

            Console.SetCursorPosition(22, Console.CursorTop);
            position = Console.ReadLine().Split(null);
            gridParams.goalPosition = new Vector2(
                (float)Convert.ToDouble(position[0]),
                (float)Convert.ToDouble(position[0]));

            Console.SetCursorPosition(31, Console.CursorTop);
            gridParams.obstacleDensity = 
                (float)Convert.ToDouble(Console.ReadLine());

            // Choose Obstacle Types
            gridParams.obstacleTypes = new List<Cell.Type>();
            Console.SetCursorPosition(23, Console.CursorTop + 1);
            char choice = Convert.ToChar(Console.ReadLine());
            if (choice == 'y' || choice == 'Y')
            {
                gridParams.obstacleTypes.Add(Cell.Type.Wall);
            }
            Console.SetCursorPosition(23, Console.CursorTop);
            choice = Convert.ToChar(Console.ReadLine());
            if (choice == 'y' || choice == 'Y')
            {
                gridParams.obstacleTypes.Add(Cell.Type.Pit);
            }

            return gridParams;
        }

        private GridParameters ReadCustomGrid()
        {
            Console.WriteLine("Expected format:");
            Console.WriteLine("    Floor - '1' or '.'");
            Console.WriteLine("    Wall  - '0' or 'X'");
            Console.WriteLine("    Pit   - '2' or '_'");
            Console.WriteLine("    Rover - 'R' or '8'");
            Console.WriteLine("    Goal  - 'G' or '9'");
            Console.WriteLine("Example file contents:");
            Console.WriteLine("R.X.._      810112      R.X.._");
            Console.WriteLine("...X.X  or  111010  or  11.0.X");
            Console.WriteLine("X_.X..      021011      X2.X..");
            Console.WriteLine(".X...G      101119      .01..9");

            Console.WriteLine("Enter the filename including extension");
            Console.WriteLine("(i.e. Test1.map or Test1.txt): ");
            string filename = Console.ReadLine();
            string[] map = System.IO.File.ReadAllLines(filename);
            


            return new GridParameters();
        }

        private RoverParameters GetRoverParameters()
        {
            // Prompt user for RoverParameters
            return new RoverParameters();
        }

        private void FlagNoPathFound() { noPathFound = true; }

        /// <summary>
        /// Display a text version of the grid, with markers for the Rover 'R',
        /// Goal 'G', Obstacles 'X', '_', etc..
        /// Also display status information (# moves/ limit, etc.)
        /// </summary>
        private void DisplayProgress()
        {
            int buffer = 3;

            Console.WriteLine();
            string title = "Rover";
            string space = new string(' ', Math.Max(grid.width + buffer - 3, 5));
            title += space + "Actual";
            Console.WriteLine(title);
            for (int y = 0; y <= grid.height + 1; ++y)
            {
                // Print Rover's knowledge of environment.
                for (int x = 0; x <= grid.width + 1; ++x)
                {
                    char image;
                    if (x == rover.Position.x && y == rover.Position.y)
                    {
                        image = 'R';
                    }
                    else if (x == grid.goalPosition.x && y == grid.goalPosition.y)
                    {
                        image = 'G';
                    }
                    else
                    {
                        image = rover.Grid.Position[x, y].image;
                    }
                    Console.Write(image);
                }

                Console.Write("   ");

                // Print actual environment.
                for (int x = 0; x <= grid.width + 1; ++x)
                {
                    char image;
                    if (x == rover.Position.x && y == rover.Position.y)
                    {
                        image = 'R';
                    }
                    else if (x == grid.goalPosition.x && y == grid.goalPosition.y)
                    {
                        image = 'G';
                    }
                    else
                    {
                        image = grid.Position[x, y].image;
                    }
                    Console.Write(image);
                }
                Console.WriteLine();
            }
            Console.WriteLine("Position: [{0},{1}]", rover.Position.x,
                rover.Position.y);
            Console.WriteLine("Facing: {0}", rover.Facing);
            Console.WriteLine("Move: {0}/{1}", rover.MoveCount, MoveLimit);
//            Console.WriteLine();
        }
    }
}
