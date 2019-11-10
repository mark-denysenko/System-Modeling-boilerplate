using System;
using System.Collections.Generic;
using Domain.Modeling.BaseEntities;
using Hospital.HospitalVisitors;

namespace Hospital.Branches
{
    public class LaboratoryBranch : Branch
    {
        public Processor AcceptanceRoom;
        public Disposer Exit;

        public LaboratoryBranch(Processor acceptanceRoom, Disposer exit) : base(new List<(SchemaElementBase element, double probability)>())
        {
            AcceptanceRoom = acceptanceRoom;
            Exit = exit;
        }

        public override void InAct(EventBase e)
        {
            var visitor = e as BaseVisitor;

            if (visitor.VisitorType == VisitorType.EnteredExamination)
            {
                Exit.InAct(e);
            }
            else
            {
                visitor.VisitorType = VisitorType.StartedTreatment;
                AcceptanceRoom.InAct(visitor);
            }
        }
    }
}
