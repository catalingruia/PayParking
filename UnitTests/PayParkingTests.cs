using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PayParking;
using PayParking.Models;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {


        [TestMethod]
        public void TestForRegisteringACarEntry()
        {
            ParkingEngine parkingEngine = new ParkingEngine();

            var registrationNumber = new StringReader("B101FFF");
            Console.SetIn(registrationNumber);

            var output = new StringWriter();
            Console.SetOut(output);
            parkingEngine.RegisterCarEntry();
            if (output.ToString().Contains("Car Registered Succesfuly!"))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }

        }

        [TestMethod]
        public void TestForRegisteringALeavingCar()
        {
            ParkingEngine parkingEngine = new ParkingEngine();

            var registrationNumber = new StringReader("B10FFF");
            Console.SetIn(registrationNumber);

            var output = new StringWriter();
            Console.SetOut(output);
            parkingEngine.RegisterCarExit();
            if (output.ToString().Contains("Total Price:"))
            {
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void TestForPrintingTheCarList()
        {
            ParkingEngine parkingEngine = new ParkingEngine();

            var input = new StringReader("");
            Console.SetIn(input);
            parkingEngine.PrintList();
        }

        [TestMethod]
        public void TestForFullCycle()
        {
            ParkingEngine parkingEngine = new ParkingEngine();

            var input1 = new StringReader("B101DDD");
            Console.SetIn(input1);

            parkingEngine.RegisterCarEntry();

            var input2 = new StringReader("B101DDD");
            Console.SetIn(input2);
            parkingEngine.RegisterCarExit();
        }
    }
}
