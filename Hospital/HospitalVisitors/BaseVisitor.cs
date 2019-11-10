using System;
using System.Collections.Generic;
using Domain.Modeling.BaseEntities;

namespace Hospital.HospitalVisitors
{
    public class BaseVisitor : EventBase, ICloneable
    {
        private static Dictionary<VisitorType, double> visitorRegistrationTime = new Dictionary<VisitorType, double>
        {
            { VisitorType.EnteredExamination, 30d },
            { VisitorType.NotFullyExaminated, 40d },
            { VisitorType.StartedTreatment, 15d }
        };

        public VisitorType VisitorType { get; set; }
        public double RegistrationTime
        {
            get
            {
                return visitorRegistrationTime[VisitorType];
            }
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
