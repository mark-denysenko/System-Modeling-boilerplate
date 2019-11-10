using Domain.Modeling;
using Domain.Modeling.BaseEntities;
using Domain.RandomNumberGenerators;
using Hospital.Branches;
using Hospital.HospitalProcessing;
using Hospital.HospitalVisitors;
using System;
using System.Collections.Generic;

namespace Hospital
{
    class Program
    {
        private static double SimulationTime = 10000d;

        static void Main(string[] args)
        {
            Console.WriteLine($"===== Hospital Simulation (Time : {SimulationTime})");
            HospitalSimulation();

            Console.ReadKey();
        }

        private static void HospitalSimulation()
        {
            const int MAX_LIMIT = 1000;

            const double visitorArrivingDelay = 15d;
            const int registrationQueue = MAX_LIMIT;
            const int laboratoryQueue = MAX_LIMIT;
            const int doctorsInRegistration = 2;
            const int AccompanyingFromAcceptanceRoomToHospitalRoom = 3;
            const int AccompanyingFromAcceptanceRoomToLaboratory = MAX_LIMIT;
            const int AccompanyingFromLaboratoryToAcceptanceRoom = MAX_LIMIT;
            const int laboratoryRegistrators = 1;
            const int laboratoryAssistans = 2;

            var vistorsProbability = new List<(BaseVisitor element, double probability)>
            {
                (new BaseVisitor { VisitorType = VisitorType.EnteredExamination }, 0.4d),
                (new BaseVisitor { VisitorType = VisitorType.NotFullyExaminated }, 0.1d),
                (new BaseVisitor { VisitorType = VisitorType.StartedTreatment }, 0.5d),
            };

            var HospitalEntrance = new HospitalVisitorCreator(
                "Hospital Entrance", 
                RandomGeneratorsFactory.GetExponential(visitorArrivingDelay), 
                vistorsProbability);

            var AcceptanceRoom = new AcceptanceRoomProcessor("Acceptance room", () => 0d, registrationQueue, doctorsInRegistration);
            var GoingFromAcceptanceRoomToHospitalRoom = new Processor("Palata going", RandomGeneratorsFactory.GetUniform(3d, 8d), registrationQueue, AccompanyingFromAcceptanceRoomToHospitalRoom);

            var LaboratoryRegistration = new Processor("Laboratory Registration room", RandomGeneratorsFactory.Erlang(3, 4.5d), laboratoryQueue, laboratoryRegistrators);
            var LaboratoryAnalysis = new Processor("Laboratory Analysis room", RandomGeneratorsFactory.Erlang(2, 4d), laboratoryQueue, laboratoryAssistans);
            var GoingFromAcceptanceRoomToLaboratory = new Processor("Palata going", RandomGeneratorsFactory.GetUniform(2d, 5d), registrationQueue, AccompanyingFromAcceptanceRoomToLaboratory);
            var GoingFromLaboratoryToAcceptanceRoom = new Processor("Palata going", RandomGeneratorsFactory.GetUniform(2d, 5d), registrationQueue, AccompanyingFromLaboratoryToAcceptanceRoom);

            var HospitalRoom = new Disposer("Hospital room");
            var HospitalExit = new Disposer("Hospital exit");

            var acceptanceRoomBranch = new AcceptanceRoomBranch(GoingFromAcceptanceRoomToHospitalRoom, GoingFromAcceptanceRoomToLaboratory);
            var laboratoryBranch = new LaboratoryBranch(GoingFromLaboratoryToAcceptanceRoom, HospitalExit);

            // make relations between elements
            HospitalEntrance.NextElement = AcceptanceRoom;
            AcceptanceRoom.NextElement = acceptanceRoomBranch;
            GoingFromAcceptanceRoomToHospitalRoom.NextElement = HospitalRoom;

            GoingFromAcceptanceRoomToLaboratory.NextElement = LaboratoryRegistration;
            LaboratoryRegistration.NextElement = LaboratoryAnalysis;
            LaboratoryAnalysis.NextElement = laboratoryBranch;

            GoingFromLaboratoryToAcceptanceRoom.NextElement = AcceptanceRoom;

            var simulationModel = new HospitalSimulation(new List<SchemaElementBase>
            {
                HospitalEntrance,
                AcceptanceRoom,
                GoingFromAcceptanceRoomToHospitalRoom,
                GoingFromAcceptanceRoomToLaboratory,
                LaboratoryRegistration,
                LaboratoryAnalysis,
                GoingFromLaboratoryToAcceptanceRoom,
                HospitalRoom,
                HospitalExit
            },
            SimulationTime);

            simulationModel.Simulate(false);
            simulationModel.PrintResultStatistic();
        }
    }
}
