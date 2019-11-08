using System;

namespace Domain.Modeling.Abstract
{
    public interface IDelayGenerator
    {
        Func<double> GetDelay { get; set; }
    }
}
