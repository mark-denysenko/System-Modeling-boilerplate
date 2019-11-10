using System;
using System.Collections.Generic;
using Domain.Modeling.BaseEntities;

namespace Hospital.HospitalVisitors
{
    public class HospitalVisitorCreator : Creator<BaseVisitor>
    {
        protected Dictionary<VisitorType, int> createdVisitorsByType = new Dictionary<VisitorType, int>(Enum.GetNames(typeof(VisitorType)).Length);

        public HospitalVisitorCreator(string name, Func<double> delayGenerator, IList<(BaseVisitor element, double probability)> possibleElements) : base(name, delayGenerator, possibleElements)
        {
        }

        public override EventBase OutAct()
        {
            var visitor = base.OutAct() as BaseVisitor;

            if (!createdVisitorsByType.ContainsKey(visitor.VisitorType))
            {
                createdVisitorsByType.Add(visitor.VisitorType, 0);
            }

            createdVisitorsByType[visitor.VisitorType]++;

            return visitor;
        }

        public override void DisplayStatistic(double simulateTime)
        {
            Console.WriteLine($"Total hospital visitors: {CompletedEvents}");
            foreach(var type in createdVisitorsByType)
            {
                Console.WriteLine($" - {type.Key.ToString()} : {type.Value}");
            }
        }
    }
}
