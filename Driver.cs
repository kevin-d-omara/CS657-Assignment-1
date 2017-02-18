using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class Driver
    {
        // ../../Maps/test_revert_2.map ../../Output/results.txt

        // TODO: include switches:
        // -d, --display -> prints each step of progress to the CL
        // -h, --help    -> display CL paramter options
        // -n, --no-map  -> suppress Cl output of map
        // -s, --step    -> press <enter> to step through the pathfinding
        public static void Main(string[] allArgs)
        {
            SimulationManager simulationManager = new SimulationManager();
            var args = new List<string>(allArgs);

            // Activate CL Flags
            foreach (string arg in allArgs)
            {
                switch (arg)
                {
                    case "-d":
                    case "--display":
                        args.Remove(arg);
                        SimulationManager.flags["display"] = true;
                        break;
                    case "-h":
                    case "--help":
                        args.Remove(arg);
                        Driver.DisplayHelp();
                        return;
                    case "-n":
                    case "--no-map":
                        args.Remove(arg);
                        SimulationManager.flags["no-map"] = true;
                        break;
                    case "-s":
                    case "--step":
                        args.Remove(arg);
                        SimulationManager.flags["step"] = true;
                        break;
                    default:
                        break;
                }
            }

            // Read command line parameters.
            string envFile = "n/a";
            string outFile = "n/a";

            if (args.Count == 0)
            {
                Console.WriteLine("No environment filename given. Select option from menu.");
                Console.WriteLine("No output filename given. Outputting results to: 'results.txt'");
                outFile = "results.txt";
            }
            else if (args.Count == 1)
            {
                envFile = args[0];
                Console.WriteLine("Reading environment from: " + envFile);
                Console.WriteLine("No output filename given. Outputting results to: 'results.txt'");
                outFile = "results.txt";
            }
            else if (args.Count == 2)
            {
                envFile = args[0];
                outFile = args[1];
                Console.WriteLine("Reading environment from: " + envFile);
                Console.WriteLine("Outputting results to: " + outFile);
            }
            else
            {
                Console.WriteLine("Too many arguments specified.");
                Console.WriteLine("Select option from menu.");
                Console.WriteLine("Outputting results to: 'results.txt'");
                outFile = "results.txt";
            }

            simulationManager.StartSimulation(envFile, outFile);
            
            Console.ReadKey();  // stop terminal from closing
        }

        private static void DisplayHelp()
        {
            // ../../Maps/test_revert_2.map ../../Output/results.txt

            // TODO: include switches:
            // -d, --display -> prints each step of progress to the CL
            // -h, --help    -> display CL paramter options
            // -n, --no-map  -> suppress Cl output of map
            // -s, --step    -> press <enter> to step through the pathfinding

            Console.WriteLine("NAME");
            Console.WriteLine("    Assignment1 - Watch expert AI guide a Rover back to base across the Martian surface.");
            Console.WriteLine("");
            Console.WriteLine("SYNOPSIS");
            Console.WriteLine("    Assignment1 [Environment-Filename] [Output-Filename] [OPTIONS]...");
            Console.WriteLine("");
            Console.WriteLine("DESCRIPTION");
            Console.WriteLine("    If no environment filename is specified, a menu will be offered.");
            Console.WriteLine("    The menu includes environment creation through default and custom options.");
            Console.WriteLine("");
            Console.WriteLine("    If no output filename is specified, a file with the name 'results.txt' will be created in the current directory.");
            Console.WriteLine("");
            Console.WriteLine("    Switches:");
            Console.WriteLine("        -d, --display -> prints each step of progress to the command line");
            Console.WriteLine("        -h, --help    -> display this menu");
            Console.WriteLine("        -n, --no-map  -> supress command line output of the map (note: only works when used alongside -d or --display");
            Console.WriteLine("        -s, --setp    -> press <enter> to advance after each step of the Rover's movement");
            Console.WriteLine("");
            Console.ReadKey();  // stop terminal from closing
        }
    }
}
