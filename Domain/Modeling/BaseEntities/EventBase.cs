using System;

namespace Domain.Modeling.BaseEntities
{
    public class EventBase
    {
        public double CreateTime { get; set; } = double.MinValue;
        public double FinishTime { get; set; } = double.MinValue;
    }
}
