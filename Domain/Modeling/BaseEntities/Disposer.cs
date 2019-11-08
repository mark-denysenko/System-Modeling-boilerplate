using System;

namespace Domain.Modeling.BaseEntities
{
    public class Disposer : SchemaElementBase
    {
        public override double NextEventTime { get => double.MaxValue; }

        public Disposer(string name) : base(name, () => 0d) { }

        public override void InAct(EventBase e)
        {
            base.InAct(e);
            e.FinishTime = CurrentTime;
            OutAct();
        }

        public override void DisplayStatistic(double simulateTime)
        {
            Console.WriteLine($"[Disposer] ({Name}) caught: {InputEvents} events");
        }

        public override void DoStatistic(double changedTime) { }
    }
}
