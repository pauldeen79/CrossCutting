namespace CrossCutting.Data.Abstractions.Tests.Extensions;

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

    public static IEnumerable<object?[]> FixDbNullData
        => new List<object?[]>
        {
                new object?[] { DBNull.Value, null },
                new object?[] { null, null },
                new object?[] { "", "" },
                new object?[] { "test", "test" },
                new object?[] { 1, 1 },
                new object?[] { false, false },
                new object?[] { new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified) },
        };

    public static IEnumerable<object?[]> FixNullData
        => new List<object?[]>
        {
                new object?[] { null, DBNull.Value },
                new object?[] { "", "" },
                new object?[] { "test", "test" },
                new object?[] { 1, 1 },
                new object?[] { false, false },
                new object?[] { new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified) },
        };
}
