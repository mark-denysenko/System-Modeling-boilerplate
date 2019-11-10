using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Modeling.BaseEntities;
using Hospital.HospitalProcessing;
using Hospital.HospitalVisitors;

namespace Hospital
{
    public class AcceptanceRoomProcessor : Processor
    {
        public AcceptanceRoomProcessor(string name, Func<double> delayGenerator, int maxQueue, int cores) : base(name, delayGenerator, maxQueue, cores)
        {
            var coreList = new List<HospitalDoctor>(cores);
            for (int i = 0; i < cores; i++)
            {
                coreList.Add(new HospitalDoctor());
            }

            this.cores = coreList;
        }

        public override IEnumerable<EventBase> Queue
        {
            get => base.Queue.Select(e => e as BaseVisitor).OrderByDescending(visitor => visitor.VisitorType == VisitorType.StartedTreatment);
            set => base.Queue = value;
        }
    }
}
