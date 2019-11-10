using Domain.Modeling.BaseEntities;
using Domain.RandomNumberGenerators;
using System;

namespace Hospital
{
    class Program
    {
        private double SimulationTime = 1000d;

        static void Main(string[] args)
        {
            Console.WriteLine("===== Hospital Simulation");
            HospitalSimulation();

            Console.ReadKey();
        }

        private static void HospitalSimulation()
        {
            var HospitalEntrance = new Creator("Hospital Entrance");

            var AcceptanceRoom = new Processor("Acceptance room", RandomGeneratorsFactory.)

            var HospitalRoom = new Disposer("Hospital room");
            var HospitalExit = new Disposer("Hospital exit");

        }
    }
}
