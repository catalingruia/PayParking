using PayParking.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayParking
{
    class Program
    {
        static void Main(string[] args)
        {
            MainMenu mainMenu = new MainMenu();

            bool showMenu = true;

            while (showMenu)
            {
                showMenu = mainMenu.ShowMenu();
            }
        }

    }
}
