namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.MathematicExpressions.Aggregators;

public class DivideAggregatorTests
{
    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Byte()
    {
        // Act
        var result = new DivideAggregator().Aggregate((byte)4, (byte)2, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Short()
    {
        // Act
        var result = new DivideAggregator().Aggregate((short)4, (short)2, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Int()
    {
        // Act
        var result = new DivideAggregator().Aggregate(4, 2, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Long()
    {
        // Act
        var result = new DivideAggregator().Aggregate((long)4, (long)2, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((long)2);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Float()
    {
        // Act
        var result = new DivideAggregator().Aggregate((float)4.5, (float)1.5, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((float)3);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Decimal()
    {
        // Act
        var result = new DivideAggregator().Aggregate((decimal)4.5, (decimal)1.5, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(3);
    }

    [Fact]
    public void Aggregate_Returns_Correct_Result_On_Double()
    {
        // Act
        var result = new DivideAggregator().Aggregate(4.5, 1.5, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(3);
    }
}
