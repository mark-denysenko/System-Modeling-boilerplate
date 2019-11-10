using Domain.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Modeling.BaseEntities
{
    public class Branch : SchemaElementBase
    {
        protected IList<(SchemaElementBase element, double probability)> possibleElements;
        private RandomSelection randomSelection = new RandomSelection();

        public override double NextEventTime { get => double.MaxValue; }

        public Branch(IList<(SchemaElementBase element, int weight)> possibleElements) 
            : base("branch", () => 0d)
        {
            this.possibleElements = randomSelection.WeightToPercent(possibleElements);
        }

        public Branch(IList<(SchemaElementBase element, double percent)> possibleElements) 
            : base("branch", () => 0d)
        {
            this.possibleElements = randomSelection.PercentToPercent(possibleElements);
        }

        public override void InAct(EventBase e)
        {
            var randomIndex = randomSelection.GetRandomIndex(possibleElements.Select(el => el.probability));
            possibleElements[randomIndex].element.InAct(e);
        }

        public override void DisplayStatistic(double simulateTime)
        {
            Console.WriteLine($"[Branch] {Name} pass {CompletedEvents} events");
        }

        public override void DoStatistic(double changedTime) { }
    }
}
