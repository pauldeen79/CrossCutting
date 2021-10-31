using System;
using System.Collections.Generic;

namespace CrossCutting.Data.Abstractions
{
    public interface IDatabaseCommandProcessor<T>
    {
        object ExecuteScalar(IDatabaseCommand command);
        int ExecuteNonQuery(IDatabaseCommand command);
        T FindOne(IDatabaseCommand command);
        IReadOnlyCollection<T> FindMany(IDatabaseCommand command);
        IPagedResult<T> FindPaged(IDatabaseCommand dataCommand,
                                  IDatabaseCommand recordCountCommand,
                                  int offset,
                                  int pageSize);
        T InvokeCommand(T instance);
    }
}
