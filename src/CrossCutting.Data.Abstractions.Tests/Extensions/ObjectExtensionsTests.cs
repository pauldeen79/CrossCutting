namespace CrossCutting.Data.Abstractions.Tests.Extensions;

public class ObjectExtensionsTests
{
    [Theory]
    [MemberData(nameof(FixDbNullData))]
    public void FixDbNull_Returns_Correct_Value(object? input, object? expectedOutput)
    {
        // Act
        var actual = input!.FixDbNull();

        // Asset
        actual.Should().Be(expectedOutput);
    }

    [Theory]
    [MemberData(nameof(FixNullData))]
    public void FixNull_Returns_Correct_Value(object? input, object expectedOutput)
    {
        // Act
        var actual = input.FixNull();

        // Asset
        actual.Should().Be(expectedOutput);
    }

    public static TheoryData<object?, object?> FixDbNullData
        => new TheoryData<object?, object?>
        {
            { DBNull.Value, null },
            { null, null }, // note that according to the interface, you can't send null. but it will not crash 8-)
            { "", "" },
            { "test", "test" },
            { 1, 1 },
            { false, false },
            { new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified) },
        };

    public static TheoryData<object?, object> FixNullData
        => new TheoryData<object?, object>
        {
            { null, DBNull.Value },
            { "", "" },
            { "test", "test" },
            { 1, 1 },
            { false, false },
            { new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified) },
        };
}
