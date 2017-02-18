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
        public const int MoveLimit = 1000;

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

            var result = EnterMainLoop();

            OutputResults(outputFile, result);
        }

        private void OutputResults(string outputFilename, string result)
        {
            var t = new List<string>();

            t.Add("Result: " + result);
            t.Add("");
            t.Add("Total Moves: " + rover.MoveCount);
            t.Add("");

            // Algorithm Comparison --------------------------------------------
            t.Add("Least Possible Moves:");
            t.Add("                   Rover");
            t.Add("Algo   | Moves | Efficiency");
            // Run A* search.
            var startFacing = rover.ActionHistory.Peek().Facing;
            var aStarSearch = new AStarSearch(grid, grid.startPosition,
                startFacing, grid.goalPosition, new Stack<Rover.MoveRecord>());
            var shortestPath = aStarSearch.GetShortestPath();
            var efficiency = (float)shortestPath.Count / (float)rover.MoveCount;
            t.Add(String.Format("A*+    | {0,4}  |  {1,6:0.00}%",
                shortestPath.Count, efficiency * 100f));
            t.Add("");

            // Action Breakdown ------------------------------------------------
            t.Add("Action Breakdown:");
            var moveActionCount = rover.MoveBreakdown["Move_Forward"]
                                + rover.MoveBreakdown["Move_ForwardLeft"]
                                + rover.MoveBreakdown["Move_ForwardRight"];
            // Move
            t.Add("    Move: " + moveActionCount);
            var len = Utils.digitsIn(moveActionCount);
            t.Add(String.Format("        Forward      {0," + len +"}/{1}",
                rover.MoveBreakdown["Move_Forward"], moveActionCount));
            t.Add(String.Format("        ForwardLeft  {0," + len + "}/{1}",
                rover.MoveBreakdown["Move_ForwardLeft"], moveActionCount));
            t.Add(String.Format("        ForwardRight {0," + len + "}/{1}",
                rover.MoveBreakdown["Move_ForwardRight"], moveActionCount));

            // Rotate
            var rotateActionCount = rover.MoveBreakdown["Rotate_Left"]
                                  + rover.MoveBreakdown["Rotate_Right"];
            t.Add("    Rotate: " + rotateActionCount);
            len = Utils.digitsIn(rotateActionCount);
            t.Add(String.Format("        Left  {0," + len + "}/{1}",
                rover.MoveBreakdown["Rotate_Left"], rotateActionCount));
            t.Add(String.Format("        Right {0," + len + "}/{1}",
                rover.MoveBreakdown["Rotate_Right"], rotateActionCount));

            // Revert
            var revertActionCount = rover.MoveBreakdown["Revert_Move"]
                                  + rover.MoveBreakdown["Revert_Rotate"];
            t.Add("    Revert: " + revertActionCount);
            len = Utils.digitsIn(revertActionCount);
            t.Add(String.Format("        Move   {0," + len + "}/{1}",
                rover.MoveBreakdown["Revert_Move"], revertActionCount));
            t.Add(String.Format("        Rotate {0," + len + "}/{1}",
                rover.MoveBreakdown["Revert_Rotate"], revertActionCount));

            // Environment Breakdown--------------------------------------------
            t.Add("");
            t.Add("Environment:");
            //WISE 0855−0714 
            var randomPlanet = Utils.randomString(Utils.upperChars, 4) + " "
                + Utils.randomString(Utils.alphaChars, 4) + "-"
                + Utils.randomString(Utils.alphaChars, 4);
            var randomSector = Utils.randomString(Utils.upperChars, 3) + " "
                + Utils.randomString(Utils.alphaChars, 2) + "."
                + Utils.randomString(Utils.alphaChars, 2) + "."
                + Utils.randomString(Utils.alphaChars, 3);

            t.Add("    Planet: " + randomPlanet);
            t.Add("    Sector: " + randomSector);
            t.Add("    Dimensions:");
            t.Add("        Width  - " + grid.width + "m");
            t.Add("        Height - " + grid.height + "m");
            t.Add("        Area   - " + grid.width * grid.height + "m^2");
            t.Add("    Obstructions:");
            t.Add("        Encountered     - ");
            t.Add("        Approx. Density - ");
            t.Add(String.Format("        Actual  Density - {0:0.00}%",
                grid.obstacleDensity * 100f));
            t.Add(String.Format("                          {0:0.00}/m^2",
                grid.obstacleDensity * 100f));

            // Environment Map -------------------------------------------------
            t.Add("");
            t.Add("Environment Map:");
            var environmentMap = grid.ToImage();
            environmentMap[(int)grid.startPosition.x, (int)grid.startPosition.y]
                = 'R';
            environmentMap[(int)grid.goalPosition.x, (int)grid.goalPosition.y]
                = 'G';

            for (int y = 0; y < environmentMap.GetLength(1); ++y)
            {
                var line = "";
                for (int x = 0; x < environmentMap.GetLength(0); ++x)
                {
                    line += environmentMap[x, y];
                }
                t.Add(line);
            }

            // Database Map ----------------------------------------------------
            t.Add("");
            t.Add("Rover Database Map:");
            var databaseMap = rover.Grid.ToImage();
            databaseMap[(int)grid.goalPosition.x, (int)grid.goalPosition.y]
                = 'G';

            for (int y = 0; y < databaseMap.GetLength(1); ++y)
            {
                var line = "";
                for (int x = 0; x < databaseMap.GetLength(0); ++x)
                {
                    line += databaseMap[x, y];
                }
                t.Add(line);
            }

            // Locations Visited Map -------------------------------------------
            t.Add("");
            t.Add("Locations Visited:");
            for (int y = 1; y < databaseMap.GetLength(1) -1 ; ++y)
            {
                for (int x = 1; x < databaseMap.GetLength(0) - 1; ++x)
                {
                    databaseMap[x, y] = '.';
                }
            }

            foreach (Rover.ActionRecord action in rover.ActionHistory)
            {
                databaseMap[(int)action.Position.x, (int)action.Position.y] = 'R';
            }

            for (int y = 0; y < databaseMap.GetLength(1); ++y)
            {
                var line = "";
                for (int x = 0; x < databaseMap.GetLength(0); ++x)
                {
                    line += databaseMap[x, y];
                }
                t.Add(line);
            }

            //  12345678901234567890
            // 1 R...
            // 2.RR.
            // 3.R..
            // 4...G
            // 5
            // 6
            // 7
            // 8
            // 9
            //10
            t.Add("");

            // Path Taken ------------------------------------------------------
            t.Add("Path Taken:");
            var lenM = Utils.digitsIn(rover.MoveCount);
            var lenX = Utils.digitsIn(grid.width);
            var lenY = Utils.digitsIn(grid.height);
            t.Add("Move Action    Path    Facing");
            var moveCount = 0;
            while (rover.ActionHistory.Count > 0)
            {
                var action = rover.ActionHistory.Dequeue();
                t.Add(String.Format("[{0," + lenM + "}] {1,-6} -> [{2," + lenX 
                    + "},{3," + lenY + "}] {4}", moveCount, 
                    action.Action.ToString(), action.Position.x,
                    action.Position.y, action.Facing));
                ++moveCount;
            }
            t.Add("-----> " + result);

            System.IO.File.WriteAllLines(outputFilename, t);
        }

        private string EnterMainLoop()
        {
            var result = "Undefined";
            // Simulation Loop
            while (true)
            {
                rover.Update();

                // Exit conditions.
                if (rover.Position.Equals(grid.goalPosition))
                {
                    DisplayProgress();
                    Console.WriteLine("-----> Goal Reached!");
                    result = "Goal Reached";
                    break;
                }
                if (rover.MoveCount >= MoveLimit)
                {
                    DisplayProgress();
                    Console.WriteLine("-----> Move Limit Reached.");
                    result = "Move Limit Reached";
                    break;
                }
                if (noPathFound)
                {
                    DisplayProgress();
                    Console.WriteLine("-----> No Path Found!");
                    result = "No Path Found";
                    break;
                }
            }
            return result;
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

            Console.WriteLine("Width: ");
            Console.WriteLine("Height: ");
            Console.WriteLine("Start Position (x y): ");
            Console.WriteLine("Goal  Position (x y): ");
            Console.WriteLine("Obstacle Density (0.0 to 1.0): ");
            Console.WriteLine("Obstacle Types (y or n):");
            Console.WriteLine("                Wall - ");
            Console.WriteLine("                Pit  - ");

            Console.SetCursorPosition(7, Console.CursorTop - 8);
            gridParams.width = Convert.ToInt32(Console.ReadLine());

            Console.SetCursorPosition(8, Console.CursorTop);
            gridParams.height = Convert.ToInt32(Console.ReadLine());

            Console.SetCursorPosition(22, Console.CursorTop);
            string[] position = Console.ReadLine().Split(null);
            gridParams.startPosition = new Vector2(
                (float)Convert.ToDouble(position[0]),
                (float)Convert.ToDouble(position[1]));

            Console.SetCursorPosition(22, Console.CursorTop);
            position = Console.ReadLine().Split(null);
            gridParams.goalPosition = new Vector2(
                (float)Convert.ToDouble(position[0]),
                (float)Convert.ToDouble(position[1]));

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
        }
    }
}
