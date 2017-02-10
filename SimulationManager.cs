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
        public const int MoveLimit = 1;

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
            while (true)
            {
                rover.Update();
                grid.Display(rover.Position);

                // Exit conditions.
                if (rover.Position == grid.goalPosition)
                {
                    Console.WriteLine("Goal Reached!");
                    break;
                }
                if (rover.MoveCount >= MoveLimit)
                {
                    Console.WriteLine("Move Limit Reached.");
                    break;
                }
            }
        }

        private GridParameters GetGridParameters()
        {
            // Prompt user for GridParameters
            return new GridParameters();
        }
    }
}
