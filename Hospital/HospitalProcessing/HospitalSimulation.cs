using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Modeling;
using Domain.Modeling.BaseEntities;

namespace Hospital.HospitalProcessing
{
    public class HospitalSimulation : EventBasedSimulationModel
    {
        public HospitalSimulation(IEnumerable<SchemaElementBase> elements, double simulateTime) : base(elements, simulateTime)
        {
        }

        public override void PrintResultStatistic()
        {
            foreach (var e in Elements)
            {
                e.DisplayStatistic(SimulateTime);
            }

            var exits = Elements.Where(el => el is Disposer).Select(el => el as Disposer).ToList();

            Console.WriteLine($"Average time in clinic: {exits.Select(ex => ex.SumCustomersTimeInBank / ex.CompletedEvents).Sum() / exits.Count}");
        }
    }
}
