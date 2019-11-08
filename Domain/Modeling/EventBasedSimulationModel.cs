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
            double currentTime = 0.0;

            while (currentTime < SimulateTime)
            {
                var timeNextEvent = SimulateIteration(currentTime);

                //if (timeNextEvent <= currentTime)
                //    break;

                currentTime = timeNextEvent;

                if (withStepInfo)
                {
                    PrintElementsInfo(currentTime);
                }
            }
        }

        /// <summary>
        /// Go through elements and search for next event
        /// </summary>
        /// <returns>next event time</returns>
        public virtual double SimulateIteration(double currentTime)
        {
            double tnext = Elements.Min(element => element.NextEventTime);

            foreach (var e in Elements)
            {
                e.CurrentTime = tnext;
            }

            return tnext;
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
