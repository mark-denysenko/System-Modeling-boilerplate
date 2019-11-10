using System;
using Domain.Modeling.BaseEntities;
using Hospital.HospitalVisitors;

namespace Hospital.HospitalProcessing
{
    public class HospitalDoctor : Core
    {
        public HospitalDoctor() : base(() => 0d) { }

        public override void InAct(EventBase element)
        {
            base.InAct(element);

            NextEventTime = CurrentTime + (element as BaseVisitor).RegistrationTime;
        }
    }
}
