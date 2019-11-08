using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Modeling.BaseEntities
{
    public class Core : SchemaElementBase
    {
        public double WorkingTime { get; private set; }

        public Core(Func<double> delayGenerator) : base("core", delayGenerator) { }

        public override void MakeJob(double changedTime)
        {
            // make job in owner - Processor
        }

        public override void InAct(EventBase element)
        {
            base.InAct(element);
        }

        public override EventBase OutAct()
        {
            return base.OutAct();
        }

        public override void DisplayStatistic(double simulateTime)
        {
            Console.WriteLine($"[Core] wotk time: {WorkingTime}");
        }

        public override void DoStatistic(double changedTime)
        {
            if (ExecutionState == ExecutionState.Work)
            {
                WorkingTime += changedTime;
            }
        }
    }
}
