using System;
using System.Collections.Generic;

namespace Domain.Modeling.Abstract
{
    public interface ISchemaQueue<T>
    {
        int QueueLimit { get; set; }
        IEnumerable<T> Queue { get; set; }
    }
}
