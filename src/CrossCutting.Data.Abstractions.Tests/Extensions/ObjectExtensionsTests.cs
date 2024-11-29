namespace CrossCutting.Data.Abstractions.Tests.Extensions;

public class ObjectExtensionsTests
{
    [Theory]
#pragma warning disable xUnit1045 // Avoid using TheoryData type arguments that might not be serializable
    [MemberData(nameof(FixDbNullData))]
#pragma warning restore xUnit1045 // Avoid using TheoryData type arguments that might not be serializable
    public void FixDbNull_Returns_Correct_Value(object? input, object? expectedOutput)
    {
        // Act
        var actual = input!.FixDbNull();

        // Asset
        actual.Should().Be(expectedOutput);
    }

    [Theory]
#pragma warning disable xUnit1045 // Avoid using TheoryData type arguments that might not be serializable
    [MemberData(nameof(FixNullData))]
#pragma warning restore xUnit1045 // Avoid using TheoryData type arguments that might not be serializable
    public void FixNull_Returns_Correct_Value(object? input, object expectedOutput)
    {
        // Act
        var actual = input.FixNull();

        // Asset
        actual.Should().Be(expectedOutput);
    }

    public static TheoryData<object?, object?> FixDbNullData
        => new()
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
        => new()
        {
            { null, DBNull.Value },
            { "", "" },
            { "test", "test" },
            { 1, 1 },
            { false, false },
            { new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified) },
        };
}
