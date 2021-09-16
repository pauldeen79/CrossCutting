using System;
using System.Data;

namespace CrossCutting.Data.Abstractions.Extensions
{
    public static class DatabaseCommandTypeExtensions
    {
        public static CommandType ToCommandType(this DatabaseCommandType instance)
        {
            switch (instance)
            {
                case DatabaseCommandType.StoredProcedure:
                    return CommandType.StoredProcedure;
                case DatabaseCommandType.Text:
                    return CommandType.Text;
                default:
                    throw new ArgumentOutOfRangeException(nameof(instance), $"Unknown DbCommandType: {instance}");
            }
        }
    }
}
