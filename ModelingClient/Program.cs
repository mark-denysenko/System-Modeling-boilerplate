using Domain.Modeling;
using Domain.Modeling.BaseEntities;
using Domain.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ModelingClient
{
    class Program
    {
        private static double SimulateTime = 10_000d;

        static void Main(string[] args)
        {
            //Lab3();
            Console.WriteLine("Simulate dynamic systems");

            int N = 0;
            bool isInputCorrect = true;

            while (isInputCorrect || N > 0)
            {
                Console.Write("Simulate for N = ");
                isInputCorrect = int.TryParse(Console.ReadLine(), out N);

                if (!isInputCorrect)
                    break;

                LinearSchemaGenerator(N);
                SchemaWithQuickWaysGenerator(N);
            }

            Console.ReadKey();
        }

        private static void LinearSchemaGenerator(int N)
        {
            const double creatorDelay = 1.0d;
            var creatorGenerator = RandomGeneratorsFactory.GetExponential(creatorDelay);
            const double processDelay = 1.0d;
            var processorGenerator = RandomGeneratorsFactory.GetExponential(processDelay);

            var creator = new Creator<EventBase>("creator", creatorGenerator);
            var despose = new Disposer("despose");
            var elementsList = new List<SchemaElementBase>(N + 2)
            {
                creator,
                despose
            };

            SchemaElementBase previosElement = creator;
            foreach(var index in Enumerable.Range(0, N))
            {
                var newProcessor = new Processor("processor" + index, processorGenerator, 100, 1);
                previosElement.NextElement = newProcessor;
                elementsList.Add(newProcessor);

                previosElement = newProcessor;
            }
            previosElement.NextElement = despose;

            Console.WriteLine("Simualtion linear model " + DateTime.Now);
            var simulationModel = new EventBasedSimulationModel(elementsList, SimulateTime);
            simulationModel.Simulate(false);
            simulationModel.PrintResultStatistic();
        }

        private static void SchemaWithQuickWaysGenerator(int N)
        {
            const double creatorDelay = 1.0f;
            var creatorGenerator = RandomGeneratorsFactory.GetExponential(creatorDelay);
            const double processDelay = 1.0f;
            var processorGenerator = RandomGeneratorsFactory.GetExponential(processDelay);

            var creator = new Creator<EventBase>("creator", creatorGenerator);
            var despose = new Disposer("despose");
            var elementsList = new List<SchemaElementBase>(N + 2)
            {
                creator,
                despose
            };

            SchemaElementBase previosElement = creator;
            foreach (var index in Enumerable.Range(0, N))
            {
                var newProcessor = new Processor("processor" + index, processorGenerator, 100, 1);
                elementsList.Add(newProcessor);
                var newBranch = new Branch(new List<(SchemaElementBase element, double percent)> { (newProcessor, 0.95), (despose, 0.05) });
                previosElement.NextElement = newBranch;

                previosElement = newProcessor;
            }
            previosElement.NextElement = despose;

            Console.WriteLine("Simualtion linear model with quick way " + DateTime.Now);
            var simulationModel = new EventBasedSimulationModel(elementsList, SimulateTime);
            simulationModel.Simulate(false);
            simulationModel.PrintResultStatistic();
        }

        private static void Lab3()
        {
            const double creatorDelay = 1.0f;
            var creatorGenerator = RandomGeneratorsFactory.GetExponential(creatorDelay);
            const double processDelay = 2.5f;
            var processorGenerator = RandomGeneratorsFactory.GetExponential(processDelay);
            const int processMaxQueue = 5;

            var creator = new Creator<EventBase>("creator", creatorGenerator);

            var process1 = new Processor("process1", processorGenerator, processMaxQueue, 1);
            var process2 = new Processor("process2", processorGenerator, processMaxQueue, 1);
            var process3 = new Processor("process3", processorGenerator, processMaxQueue, 1);
            var process4 = new Processor("process4", processorGenerator, processMaxQueue, 1);

            var despose1 = new Disposer("despose1");
            var despose2 = new Disposer("despose2");

            var branch1 = new Branch(new List<(SchemaElementBase element, int weight)> { (process1, 4), (despose2, 1) });
            var branch2 = new Branch(new List<(SchemaElementBase element, int weight)> { (process2, 3), (process3, 2) });

            creator.NextElement = branch1;
            process1.NextElement = branch2;
            process2.NextElement = despose1;
            process3.NextElement = process4;
            process4.NextElement = despose2;

            var model = new EventBasedSimulationModel(new List<SchemaElementBase>
            {
                creator,
                process1,
                process2,
                process3,
                process4,
                despose1,
                despose2
            },
            SimulateTime);

            model.Simulate(false);
            model.PrintResultStatistic();
        }
    }
}
