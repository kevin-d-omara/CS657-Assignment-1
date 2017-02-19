using System;
using System.Collections.Generic;

namespace KevinDOMara.SDSU.CS657.Assignment1
{
    public class Driver
    {
        
        /// <summary>
        /// For help, run this program with the -h or --help flag. Or see the
        /// function DisplayHelp() below.
        /// </summary>
        /// <param name="allArgs"></param>
        public static void Main(string[] allArgs)
        {
            SimulationManager simulationManager = new SimulationManager();
            var args = new List<string>(allArgs);

            // Activate CL Flags
            foreach (string arg in allArgs)
            {
                switch (arg)
                {
                    case "-a":
                    case "--automatic":
                        args.Remove(arg);
                        SimulationManager.flags["automatic"] = true;
                        break;
                    case "-d":
                    case "--display":
                        args.Remove(arg);
                        SimulationManager.flags["display"] = true;
                        break;
                    case "-f":
                    case "--final":
                        args.Remove(arg);
                        SimulationManager.flags["final"] = true;
                        break;
                    case "-h":
                    case "--help":
                    case "-m":
                    case "--man":
                        args.Remove(arg);
                        Driver.DisplayHelp();
                        return;
                    case "-n":
                    case "--no-map":
                        args.Remove(arg);
                        SimulationManager.flags["no-map"] = true;
                        break;
                    case "-u":
                    case "--unlimited":
                        args.Remove(arg);
                        SimulationManager.flags["unlimited"] = true;
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
        }

        private static void DisplayHelp()
        {
            var manPage = System.IO.File.ReadAllLines("manpage.txt");
            foreach (string line in manPage)
            {
                Console.WriteLine(line);
            }
        }
    }
}
