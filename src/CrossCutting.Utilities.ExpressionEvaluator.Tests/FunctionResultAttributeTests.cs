﻿namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class FunctionResultAttributeTests
{
    [Fact]
    public void Should_Construct_1()
    {
        // Act & Assert
        Action a = () => _ = new MemberResultAttribute(ResultStatus.Ok, typeof(string), "value", "description");
        a.ShouldNotThrow();
    }

    [Fact]
    public void Should_Construct_2()
    {
        // Act & Assert
        Action a = () => _ = new MemberResultAttribute(ResultStatus.Ok, "description");
        a.ShouldNotThrow();
    }

    [Fact]
    public void Should_Construct_3()
    {
        // Act & Assert
        Action a = () => _ = new MemberResultAttribute(ResultStatus.Ok);
        a.ShouldNotThrow();
    }
}
