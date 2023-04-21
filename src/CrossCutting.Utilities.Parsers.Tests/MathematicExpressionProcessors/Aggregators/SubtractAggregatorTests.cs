namespace CrossCutting.Utilities.Parsers.Tests.MathematicExpressionProcessors.Aggregators;

public class SubtractAggregatorTests
{
    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Short()
    {
        // Act
        var result = new SubtractAggregator().Aggregate((short)5, (short)3);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Int()
    {
        // Act
        var result = new SubtractAggregator().Aggregate(5, 3);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Long()
    {
        // Act
        var result = new SubtractAggregator().Aggregate((long)5, (long)3);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Float()
    {
        // Act
        var result = new SubtractAggregator().Aggregate((float)4.5, (float)1.5);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(3);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Decimal()
    {
        // Act
        var result = new SubtractAggregator().Aggregate((decimal)4.5, (decimal)1.5);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(3);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Double()
    {
        // Act
        var result = new SubtractAggregator().Aggregate(4.5, 1.5);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(3);
    }
}

