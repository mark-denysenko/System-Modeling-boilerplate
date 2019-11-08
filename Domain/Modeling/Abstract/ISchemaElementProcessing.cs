using System;

namespace Domain.Modeling.Abstract
{
    public interface ISchemaElementProcessing<T>
    {
        void InAct(T element);
        T OutAct();
    }
}
