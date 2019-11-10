using System;

namespace Domain.Modeling.BaseEntities
{
    public class EventBase : ICloneable
    {
        public double CreateTime { get; set; } = double.MinValue;
        public double FinishTime { get; set; } = double.MinValue;

        public object Clone()
        {
            return new EventBase { CreateTime = this.CreateTime, FinishTime = this.FinishTime };
        }
    }
}
