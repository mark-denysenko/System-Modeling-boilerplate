using System;

namespace Domain.Modeling.BaseEntities
{
    public class Creator : SchemaElementBase
    {
        public override double NextEventTime { get; set; }

        public Creator(string name, Func<double> delayGenerator) 
            : base(name, delayGenerator)
        {
            NextEventTime = GetDelay();
        }

        public override EventBase OutAct()
        {
            CompletedEvents++;
            NextEventTime = CurrentTime + GetDelay();

            return new EventBase { CreateTime = CurrentTime };
        }

        public override void DoStatistic(double changedTime) { }

        public override void DisplayStatistic(double simulateTime)
        {
            Console.WriteLine($"[Creator] ({Name}) created: {CompletedEvents} events");
        }
    }
}
