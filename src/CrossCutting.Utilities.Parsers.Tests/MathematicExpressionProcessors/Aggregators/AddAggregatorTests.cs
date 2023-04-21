namespace CrossCutting.Utilities.Parsers.Tests.MathematicExpressionProcessors.Aggregators;

public class AddAggregatorTests
{
    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Short()
    {
        // Act
        var result = new AddAggregator().Aggregate((short)1, (short)1);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Int()
    {
        // Act
        var result = new AddAggregator().Aggregate(1, 1);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Long()
    {
        // Act
        var result = new AddAggregator().Aggregate((long)1, (long)1);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Float()
    {
        // Act
        var result = new AddAggregator().Aggregate((float)1.5, (float)1.5);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(3);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Decimal()
    {
        // Act
        var result = new AddAggregator().Aggregate((decimal)1.5, (decimal)1.5);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(3);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Double()
    {
        // Act
        var result = new AddAggregator().Aggregate(1.5, 1.5);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().Be(3);
    }
}

