using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.ComponentModel;

public interface IEntityChanged<TEventId, TEntityId, TPropertyChanged>
    : IEntityEvent<TEventId, TEntityId>
    where TEntityId : notnull
{
}

public interface IEntityChanged<TEventId, TEntityId, TObjectType, TPropertyChanged>
    : IEntityChanged<TEventId, TEntityId, TPropertyChanged>
    , ITypedObject<TObjectType>
    where TEntityId : notnull
{
}
