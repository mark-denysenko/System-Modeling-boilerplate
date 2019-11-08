using Domain.Modeling;
using Domain.Modeling.BaseEntities;
using Domain.RandomNumberGenerators;
using System;
using System.Collections.Generic;

namespace ModelingClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Lab3();
            Console.ReadKey();
        }

        private static void Lab3()
        {
            double SimulateTime = 10_000d;

            const double creatorDelay = 1.0f;
            var creatorGenerator = RandomGeneratorsFactory.GetExponential(creatorDelay);
            const double processDelay = 2.5f;
            var processorGenerator = RandomGeneratorsFactory.GetExponential(processDelay);
            const int processMaxQueue = 5;

            var creator = new Creator("creator", creatorGenerator);

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

            model.Simulate(true);
            model.PrintResultStatistic();
        }
    }
}
