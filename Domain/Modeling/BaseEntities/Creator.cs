using Domain.RandomNumberGenerators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Modeling.BaseEntities
{
    public class Creator<T> : SchemaElementBase 
        where T : EventBase, ICloneable, new()
    {
        protected IList<(T element, double probability)> possibleElements;
        private RandomSelection randomSelection = new RandomSelection();

        public Creator(string name, Func<double> delayGenerator) 
            : base(name, delayGenerator)
        {
            NextEventTime = GetDelay();
            possibleElements = new List<(T element, double probability)> { (new T(), 1d) };
        }

        public Creator(string name, Func<double> delayGenerator, IList<(T element, int weight)> possibleElements)
            : base(name, delayGenerator)
        {
            this.possibleElements = randomSelection.WeightToPercent(possibleElements);
            NextEventTime = GetDelay();
        }

        public Creator(string name, Func<double> delayGenerator, IList<(T element, double probability)> possibleElements)
            : base(name, delayGenerator)
        {
            this.possibleElements = randomSelection.PercentToPercent(possibleElements);
            NextEventTime = GetDelay();
        }

        public override EventBase OutAct()
        {
            CompletedEvents++;
            NextEventTime = CurrentTime + GetDelay();

            var randomIndex = randomSelection.GetRandomIndex(possibleElements.Select(el => el.probability));
            var newEvent = possibleElements[randomIndex].element.Clone() as T;
            newEvent.CreateTime = CurrentTime;

            return newEvent;
        }

        public override void DoStatistic(double changedTime) { }

        public override void DisplayStatistic(double simulateTime)
        {
            Console.WriteLine($"[Creator] ({Name}) created: {CompletedEvents} events");
        }
    }
}
