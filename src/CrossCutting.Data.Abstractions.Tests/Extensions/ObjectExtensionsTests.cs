using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Abstractions.Extensions;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Data.Abstractions.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ObjectExtensionsTests
    {
        [Theory]
        [MemberData(nameof(FixDbNullData))]
        public void FixDbNull_Returns_Correct_Value(object input, object expectedOutput)
        {
            // Act
            var actual = input.FixDbNull();

            // Asset
            actual.Should().Be(expectedOutput);
        }

        [Theory]
        [MemberData(nameof(FixNullData))]
        public void FixNull_Returns_Correct_Value(object input, object expectedOutput)
        {
            // Act
            var actual = input.FixNull();

            // Asset
            actual.Should().Be(expectedOutput);
        }

        public static IEnumerable<object?[]> FixDbNullData =>
        new List<object?[]>
        {
            new object?[] { DBNull.Value, null },
            new object?[] { null, null },
            new object?[] { "", "" },
            new object?[] { "test", "test" },
            new object?[] { 1, 1 },
            new object?[] { false, false },
            new object?[] { new DateTime(1900, 1,1), new DateTime(1900, 1, 1) },
        };

        public static IEnumerable<object?[]> FixNullData =>
        new List<object?[]>
        {
            new object?[] { null, DBNull.Value },
            new object?[] { "", "" },
            new object?[] { "test", "test" },
            new object?[] { 1, 1 },
            new object?[] { false, false },
            new object?[] { new DateTime(1900, 1,1), new DateTime(1900, 1, 1) },
        };
    }
}
