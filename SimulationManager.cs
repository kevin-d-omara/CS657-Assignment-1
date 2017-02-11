using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    /// <summary>
    /// Source of the main "simulation loop".
    /// Asks for user input of grid & rover parameters.
    /// </summary>
    public class SimulationManager
    {
        public const int MoveLimit = 30;

        private Grid grid;
        private Rover rover;

        public SimulationManager() { }

        public void StartSimulation()
        {
            // Setup
            GridParameters gridParameters = GetGridParameters();
            grid = new Grid(gridParameters);

            // Rover database starts with no obstacles.
            gridParameters.obstacleDensity = 0f;
            rover = new Rover(gridParameters);

            // Simulation Loop
            DisplayProgress();
            while (true)
            {
                rover.Update();
                DisplayProgress();

                // Exit conditions.
                if (rover.Position.Equals(grid.goalPosition))
                {
                    Console.WriteLine("-----> Goal Reached!");
                    break;
                }
                if (rover.MoveCount >= MoveLimit)
                {
                    Console.WriteLine("-----> Move Limit Reached.");
                    break;
                }
            }
        }

        private GridParameters GetGridParameters()
        {
            // Prompt user for GridParameters
            return new GridParameters();
        }

        /// <summary>
        /// Display a text version of the grid, with markers for the Rover 'R',
        /// Goal 'G', Obstacles 'X', '_', etc..
        /// Also display status information (# moves/ limit, etc.)
        /// </summary>
        private void DisplayProgress()
        {
            for (int y = 0; y <= grid.height + 1; ++y)
            {
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
            Console.WriteLine();
        }
    }
}
