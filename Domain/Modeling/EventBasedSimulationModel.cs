using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Modeling.BaseEntities;

namespace Domain.Modeling
{
    public class EventBasedSimulationModel
    {
        public readonly double SimulateTime;
        public readonly IEnumerable<SchemaElementBase> Elements;

        public EventBasedSimulationModel(IEnumerable<SchemaElementBase> elements, double simulateTime)
        {
            Elements = elements;
            SimulateTime = simulateTime;
        }

        public virtual void Simulate(bool withStepInfo)
        {
            double currentTime = GetNextEventTime();

            while (currentTime < SimulateTime)
            {
                UpdateElementsTime(currentTime);

                if (withStepInfo)
                {
                    PrintElementsInfo(currentTime);
                }

                currentTime = GetNextEventTime();
            }
        }

        public virtual double GetNextEventTime()
        {
            return Elements.Min(element => element.NextEventTime);
        }

        public virtual void UpdateElementsTime(double newTime)
        {
            foreach (var e in Elements)
            {
                e.CurrentTime = newTime;
            }
        }

        public virtual void PrintElementsInfo(double currentTime)
        {
            foreach(var element in Elements)
            {
                element.DisplayStatistic(currentTime);
            }
        }

        public virtual void PrintResultStatistic()
        {
            foreach (var e in Elements)
            {
                e.DisplayStatistic(SimulateTime);
            }

            var processors = Elements.Where(el => el is Processor).Select(el => el as Processor).ToList();

            Console.WriteLine("Max queue = " + processors.Max(p => p.MaxQueue));
            Console.WriteLine("Avg queue = " + processors.Sum(p => p.MeanQueue) / processors.Count / SimulateTime);
            Console.WriteLine("Avg workload = " + processors.Sum(p => p.WorkingTime) / processors.Count / SimulateTime);
        }
    }
}
