using System;
using System.Collections.Generic;
using Domain.Modeling.BaseEntities;
using Hospital.HospitalVisitors;

namespace Hospital.Branches
{
    public class AcceptanceRoomBranch : Branch
    {
        public Processor HospitalRoom;
        public Processor Laboratory;

        public AcceptanceRoomBranch(Processor hospitalRoom, Processor laboratory): base(new List<(SchemaElementBase element, double probability)>())
        {
            HospitalRoom = hospitalRoom;
            Laboratory = laboratory;
        }

        public override void InAct(EventBase e)
        {
            var visitor = e as BaseVisitor;

            if(visitor.VisitorType == VisitorType.StartedTreatment)
            {
                HospitalRoom.InAct(e);
            }
            else
            {
                Laboratory.InAct(e);
            }
        }
    }
}
