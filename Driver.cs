using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class Driver
    {
        // TODO: include switches:
        // -h, --help  -> display CL paramter options
        // -q, --quiet -> suppress CL output
        // -s, --step  -> press <enter> to step through the pathfinding
        public static void Main(string[] args)
        {
            SimulationManager simulationManager = new SimulationManager();

            // Read command line parameters.
            string envFile = "n/a";
            string outFile = "n/a";

            if (args.Length == 0)
            {
                Console.WriteLine("No environment filename given. Using " +
                    "default parameters.");
                Console.WriteLine("No output filename given. Outputting " +
                    "results to: 'results.txt'");
            }
            else if (args.Length == 1)
            {
                envFile = args[0];
                Console.WriteLine("Reading environment from: " + envFile);
                Console.WriteLine("No output filename given. Outputting " +
                    "results to: 'results.txt'");
                outFile = "results.txt";
            }
            else if (args.Length == 2)
            {
                envFile = args[0];
                outFile =  args[1];
                Console.WriteLine("Reading environment from: " + envFile);
                Console.WriteLine("Outputting results to: " + outFile);
                outFile = "results.txt";
            }
            else
            {
                Console.WriteLine("Too many arguments specified.");
                Console.WriteLine("Using default parameters.");
                Console.WriteLine("Outputting results to: 'results.txt'");
                outFile = "results.txt";
            }

            simulationManager.StartSimulation(envFile, outFile);
            
            Console.ReadKey();  // stop terminal from closing
        }
    }
}
