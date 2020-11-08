using PayParking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayParking
{
    public class MainMenu
    {

        readonly ParkingEngine parkingEngine = new ParkingEngine();

        public MainMenu()
        {
            
        }

        public bool ShowMenu()
        {
            if (!Console.IsOutputRedirected) Console.Clear();

            Console.WriteLine(@"
    ____                 ____             __   _            
   / __ \____ ___  __   / __ \____ ______/ /__(_)___  ____ _
  / /_/ / __ `/ / / /  / /_/ / __ `/ ___/ //_/ / __ \/ __ `/
 / ____/ /_/ / /_/ /  / ____/ /_/ / /  / ,< / / / / / /_/ / 
/_/    \__,_/\__, /  /_/    \__,_/_/  /_/|_/_/_/ /_/\__, /  
            /____/                                 /____/   

");
            Console.WriteLine("Available Parking Spots: " + parkingEngine.AvailableParkingSpots);
            Console.WriteLine();
            Console.WriteLine("Choose an action and press enter:");
            Console.WriteLine("[1] Register a car entering the parking lot");
            Console.WriteLine("[2] Register a car leaving the parking lot");
            Console.WriteLine("[3] Check the list of the parked cars");
            Console.WriteLine("[4] Exit");

            switch (Console.ReadLine())
            {
                case "1":
                    parkingEngine.RegisterCarEntry();
                        return true;
                case "2":
                    parkingEngine.RegisterCarExit();
                    return true;
                case "3":
                    parkingEngine.PrintList();
                    return true;
                case "4":
                    return false;
                default:
                    return true;
            }
  
        }

    }
}
