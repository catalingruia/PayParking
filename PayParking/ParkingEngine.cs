using Newtonsoft.Json;
using PayParking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PayParking
{
    public class ParkingEngine
    {
        public int AvailableParkingSpots;

        private List<CarRegistration> CarRegistrations = new List<CarRegistration>();

        readonly Pricing pricing = JsonConvert.DeserializeObject<Pricing>(File.ReadAllText("./Pricing.json"));

        readonly Regex rg = new Regex(@"[A-Z]{1,2}[0-9]{2,3}[A-Z]{3,3}");
        public ParkingEngine()
        {
            //Mocking 5 data entries 
            for (int i = 0; i < 5; i++)
            {
                int counter = 10 + i;
                string placeholder = $"B{counter}FFF";
                CarRegistration carRegistration = new CarRegistration { RegistrationNumber = placeholder, EntryTime = DateTime.UtcNow };
                CarRegistrations.Add(carRegistration);
            }

            this.AvailableParkingSpots = 10 - CarRegistrations.Count();
        }

        public void PrintList()
        {
            if (!Console.IsOutputRedirected) Console.Clear();
            Console.WriteLine(@"
   ______                                              __  __                          __            __
  / ____/___ ___________   _______  __________  ____  / /_/ /_  __   ____  ____ ______/ /_____  ____/ /
 / /   / __ `/ ___/ ___/  / ___/ / / / ___/ _ \/ __ \/ __/ / / / /  / __ \/ __ `/ ___/ //_/ _ \/ __  / 
/ /___/ /_/ / /  (__  )  / /__/ /_/ / /  /  __/ / / / /_/ / /_/ /  / /_/ / /_/ / /  / ,< /  __/ /_/ /  
\____/\__,_/_/  /____/   \___/\__,_/_/   \___/_/ /_/\__/_/\__, /  / .___/\__,_/_/  /_/|_|\___/\__,_/   
                                                         /____/  /_/                                   
");
            Console.WriteLine();
            Console.WriteLine("Curent time: " + DateTime.UtcNow);
            Console.WriteLine();

            if (CarRegistrations.Count() > 0)
            {
                foreach (CarRegistration car in CarRegistrations)
                {
                    System.Threading.Thread.Sleep(250); // Added just for a render effect
                    Console.WriteLine("####################################");
                    Console.WriteLine("--------------" + car.RegistrationNumber + "--------------");
                    Console.WriteLine("Entry Time: " + car.EntryTime);
                    Console.WriteLine("Total Time Spent: " + (DateTime.UtcNow - car.EntryTime).ToString(@"hh\:mm\:ss"));
                    Console.WriteLine("####################################");
                    Console.WriteLine();
                }

            }
            else
            {
                Console.WriteLine("There are no cars parked right now.");
            }

            Console.WriteLine();
            Console.WriteLine("Press Any key to go back to the main menu..");
            Console.ReadLine();
        }

        public void RegisterCarExit()
        {


            bool tryAgain;
            do
            {
                if (!Console.IsOutputRedirected) Console.Clear();
                Console.WriteLine(@"
    ____             _      __                                                     _ __ 
   / __ \___  ____ _(_)____/ /____  _____   ____ _   _________ ______   ___  _  __(_) /_
  / /_/ / _ \/ __ `/ / ___/ __/ _ \/ ___/  / __ `/  / ___/ __ `/ ___/  / _ \| |/_/ / __/
 / _, _/  __/ /_/ / (__  ) /_/  __/ /     / /_/ /  / /__/ /_/ / /     /  __/>  </ / /_  
/_/ |_|\___/\__, /_/____/\__/\___/_/      \__,_/   \___/\__,_/_/      \___/_/|_/_/\__/  
           /____/                                                                       
");
                Console.Write("Enter the registration number of the leaving car: ");
                string registrationNumber = Console.ReadLine();

                var itemToRemove = CarRegistrations.SingleOrDefault(r => r.RegistrationNumber == registrationNumber);

                if (itemToRemove != null)
                {
                    CarRegistrations.Remove(itemToRemove);
                    AvailableParkingSpots = 10 - CarRegistrations.Count();

                    double numberOfSeconds = DateTime.UtcNow.Subtract(itemToRemove.EntryTime).TotalSeconds;

                    double totalPrice = 0;

                    if (Math.Ceiling((numberOfSeconds / 60) / 60) == 1)
                    {
                        totalPrice = 1 * pricing.FirstHourPrice;
                    }
                    else
                    {
                        totalPrice = pricing.FirstHourPrice + (Math.Ceiling((numberOfSeconds / 60) / 60) - 1) * pricing.ExtendedHoursPrice;
                    }

                    Console.WriteLine();
                    Console.WriteLine("####################################");
                    Console.WriteLine("--------------" + itemToRemove.RegistrationNumber + "--------------");
                    Console.WriteLine("Time of entry: " + itemToRemove.EntryTime);
                    Console.WriteLine("Time of exit: " + DateTime.UtcNow);
                    Console.WriteLine("Total time spent: " + DateTime.UtcNow.Subtract(itemToRemove.EntryTime).ToString(@"hh\:mm\:ss"));
                    Console.WriteLine("----------Total Price:" + totalPrice + "$-----------");
                    Console.WriteLine("####################################");

                    tryAgain = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(registrationNumber))
                    {
                        Console.WriteLine("Please enter a Registration Number");
                    }
                    if (!string.IsNullOrEmpty(registrationNumber) && !rg.IsMatch(registrationNumber))
                    {
                        Console.WriteLine("The Registration Number you entered has a wrong format");
                        Console.WriteLine("Please check the spelling and try again");
                    }
                    if (rg.IsMatch(registrationNumber) && CarRegistrations.SingleOrDefault(car => car.RegistrationNumber == registrationNumber) == null)
                    {
                        Console.WriteLine("There are no cars registered with that registration number");
                        Console.WriteLine("Please check the spelling and try again");
                    }

                    Console.WriteLine();
                    Console.Write("Try Again? (y/n): ");
                    string answer = Console.ReadLine();

                    if (answer.ToLower() == "y")
                    {
                        tryAgain = true;
                    }
                    else
                    {
                        tryAgain = false;
                    }
                }

            } while (tryAgain);

            Console.WriteLine();
            Console.WriteLine("Press Any key to go back to the main menu..");
            Console.ReadLine();
        }

        public void RegisterCarEntry()
        {
            if (AvailableParkingSpots > 0)
            {
                bool tryAgain;
                do
                {
                    if (!Console.IsOutputRedirected) Console.Clear();
                    Console.WriteLine(@"
    ____             _      __                                                      __            
   / __ \___  ____ _(_)____/ /____  _____   ____ _   _________ ______   ___  ____  / /________  __
  / /_/ / _ \/ __ `/ / ___/ __/ _ \/ ___/  / __ `/  / ___/ __ `/ ___/  / _ \/ __ \/ __/ ___/ / / /
 / _, _/  __/ /_/ / (__  ) /_/  __/ /     / /_/ /  / /__/ /_/ / /     /  __/ / / / /_/ /  / /_/ / 
/_/ |_|\___/\__, /_/____/\__/\___/_/      \__,_/   \___/\__,_/_/      \___/_/ /_/\__/_/   \__, /  
           /____/                                                                        /____/   
");

                    Console.Write("Enter The Registration Number: ");
                    string registrationNumber = Console.ReadLine();
                    Console.WriteLine();

                    if (!string.IsNullOrEmpty(registrationNumber) && rg.IsMatch(registrationNumber) && CarRegistrations.SingleOrDefault(car => car.RegistrationNumber == registrationNumber) == null)
                    {
                        CarRegistration carRegistration = new CarRegistration { RegistrationNumber = registrationNumber, EntryTime = DateTime.UtcNow };
                        CarRegistrations.Add(carRegistration);
                        this.AvailableParkingSpots = 10 - CarRegistrations.Count();
                        tryAgain = false;
                        Console.WriteLine("Car Registered Succesfuly!");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(registrationNumber))
                        {
                            Console.WriteLine("Please enter a Registration Number");
                        }
                        if (!string.IsNullOrEmpty(registrationNumber) && !rg.IsMatch(registrationNumber))
                        {
                            Console.WriteLine("The Registration Number you entered has a wrong format");
                            Console.WriteLine("Please check the spelling and try again");
                        }
                        if (CarRegistrations.SingleOrDefault(car => car.RegistrationNumber == registrationNumber) != null)
                        {
                            Console.WriteLine("The Registration Number is already registered");
                            Console.WriteLine("Please check the spelling and try again");
                        }

                        Console.WriteLine();
                        Console.Write("Try Again? (y/n): ");
                        string answer = Console.ReadLine();

                        if (answer.ToLower() == "y")
                        {
                            tryAgain = true;
                        }
                        else
                        {
                            tryAgain = false;
                        }
                    }
                }
                while (tryAgain);
            }
            else
            {
                if (!Console.IsOutputRedirected) Console.Clear();
                Console.WriteLine(@"
    ____             _      __                                                      __            
   / __ \___  ____ _(_)____/ /____  _____   ____ _   _________ ______   ___  ____  / /________  __
  / /_/ / _ \/ __ `/ / ___/ __/ _ \/ ___/  / __ `/  / ___/ __ `/ ___/  / _ \/ __ \/ __/ ___/ / / /
 / _, _/  __/ /_/ / (__  ) /_/  __/ /     / /_/ /  / /__/ /_/ / /     /  __/ / / / /_/ /  / /_/ / 
/_/ |_|\___/\__, /_/____/\__/\___/_/      \__,_/   \___/\__,_/_/      \___/_/ /_/\__/_/   \__, /  
           /____/                                                                        /____/   
");
                Console.WriteLine("There are no parking spots available. Please come back later!");
            }

            Console.WriteLine();
            Console.WriteLine("Press Any key to go back to the main menu..");
            Console.ReadLine();
        }
    }
}
