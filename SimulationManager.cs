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
        public SimulationManager() { }

        public void BeginSimulation()
        {
            GridParameters gParams = GetGridParameters();
            //RoverParameters rParams = GetRoverParameters();
        }

        private GridParameters GetGridParameters()
        {
            // Prompt user for GridParameters
            return new GridParameters();
        }

        private void GetRoverParameters()
        {
            // Prompt user for RoverParameters
            //return new RoverParameters();
        }
    }
}
