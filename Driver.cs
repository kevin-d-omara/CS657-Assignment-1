using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class Driver
    {
        public static void Main(string[] args)
        {
            SimulationManager simulationManager = new SimulationManager();
            simulationManager.StartSimulation();
            
            Console.ReadKey();  // stop terminal from closing
        }
    }
}
