﻿using System.Collections.Generic;

namespace CrossCutting.DataTableDumper.Abstractions
{
    public interface IColumnDataProvider<in T>
        where T : class
    {
        IReadOnlyCollection<string> Get(T item);
    }
}