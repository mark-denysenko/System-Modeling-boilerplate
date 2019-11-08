using System;

namespace Domain.Modeling.Abstract
{
    public interface IWorkingElement<T>
    {
        T CurrentEvent { get; set; }
        ExecutionState ExecutionState { get; set; }
    }
}
