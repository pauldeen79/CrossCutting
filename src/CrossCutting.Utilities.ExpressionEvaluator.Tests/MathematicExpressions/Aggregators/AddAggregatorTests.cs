namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.MathematicExpressions.Aggregators;

public class AddAggregatorTests
{
    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Byte()
    {
        // Act
        var result = new AddAggregator().Aggregate((byte)1, (byte)1, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Short()
    {
        // Act
        var result = new AddAggregator().Aggregate((short)1, (short)1, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Int()
    {
        // Act
        var result = new AddAggregator().Aggregate(1, 1, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Long()
    {
        // Act
        var result = new AddAggregator().Aggregate((long)1, (long)1, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((long)2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Float()
    {
        // Act
        var result = new AddAggregator().Aggregate((float)1.5, (float)1.5, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((float)3);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Decimal()
    {
        // Act
        var result = new AddAggregator().Aggregate((decimal)1.5, (decimal)1.5, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe((decimal)3);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Double()
    {
        // Act
        var result = new AddAggregator().Aggregate(1.5, 1.5, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe((double)3);
    }
}
