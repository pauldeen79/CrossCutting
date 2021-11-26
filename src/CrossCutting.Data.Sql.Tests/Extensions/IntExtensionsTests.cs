using System.Diagnostics.CodeAnalysis;
using CrossCutting.Data.Sql.Extensions;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Data.Sql.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class IntExtensionsTests
    {
        [Theory]
        [InlineData(null, null, 0)]
        [InlineData(100, 10, 10)]
        [InlineData(100, 1000, 100)]
        [InlineData(0, 100, 100)]
        public void IfNotGreaterThan_Returns_Correct_Value(int? queryLimit, int? overrideLimit, int expectedResult)
        {
            // Act
            var actual = queryLimit.IfNotGreaterThan(overrideLimit);

            // Asset
            actual.Should().Be(expectedResult);
        }
    }
}
