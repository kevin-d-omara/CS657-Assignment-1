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
        public const int MoveLimit = 10;

        private Grid grid;
        private Rover rover;

        public SimulationManager() { }

        public void BeginSimulation()
        {
            // Setup
            GridParameters gParams = GetGridParameters();
            grid = new Grid(gParams);
            rover = new Rover();

            // Simulation Loop
            while (true)
            {
                // Exit condition.
                if (rover.Position == grid.goalPos)
                {
                    Console.WriteLine("Goal Reached!");
                    break;
                }
                if (rover.MoveCount > MoveLimit)
                {
                    Console.WriteLine("Move Limit Reached.");
                    break;
                }

                rover.Update();

            }
        }

        private GridParameters GetGridParameters()
        {
            // Prompt user for GridParameters
            return new GridParameters();
        }
    }
}
