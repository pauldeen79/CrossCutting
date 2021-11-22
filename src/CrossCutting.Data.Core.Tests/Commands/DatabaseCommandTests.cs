using System;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions;
using CrossCutting.Data.Core.Commands;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Data.Core.Tests.Commands
{
    [ExcludeFromCodeCoverage]
    public class DatabaseCommandTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NonGeneric_Ctor_Throws_On_CommandText(string commandText)
        {
            this.Invoking(_ => new SqlDatabaseCommand(commandText, DatabaseCommandType.Text))
                .Should().Throw<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("commandText");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Generic_Ctor_Throws_On_CommandText(string commandText)
        {
            this.Invoking(_ => new DatabaseCommand<DatabaseCommandTests>(commandText, DatabaseCommandType.Text, this, _ => null))
                .Should().Throw<ArgumentOutOfRangeException>()
                .And.ParamName.Should().Be("commandText");
        }
    }
}
