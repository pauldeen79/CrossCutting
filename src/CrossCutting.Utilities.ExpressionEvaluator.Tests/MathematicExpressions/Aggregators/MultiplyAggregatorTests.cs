namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.MathematicExpressions.Aggregators;

public class MultiplyAggregatorTests
{
    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Byte()
    {
        // Act
        var result = new MultiplyAggregator().Aggregate((byte)2, (byte)3, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(6);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Short()
    {
        // Act
        var result = new MultiplyAggregator().Aggregate((short)2, (short)3, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(6);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Int()
    {
        // Act
        var result = new MultiplyAggregator().Aggregate(2, 3, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(6);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Long()
    {
        // Act
        var result = new MultiplyAggregator().Aggregate((long)2, (long)3, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((long)6);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Float()
    {
        // Act
        var result = new MultiplyAggregator().Aggregate((float)1.5, (float)2, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((float)3);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Decimal()
    {
        // Act
        var result = new MultiplyAggregator().Aggregate((decimal)1.5, (decimal)2, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(3);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Double()
    {
        // Act
        var result = new MultiplyAggregator().Aggregate(1.5, 2, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(3);
    }
}
