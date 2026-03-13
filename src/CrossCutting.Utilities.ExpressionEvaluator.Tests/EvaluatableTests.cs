namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class EvaluatableTests : TestBase
{
    [Fact]
    public void OfPropertyName_Generates_Evaluatable_With_Property()
    {
        // Act
        var sut = Evaluatable.OfPropertyName("MyProperty");

        // Assert
        sut.ShouldBeOfType<PropertyNameEvaluatable>();
        sut.PropertyName.ShouldBe("MyProperty");
        sut.Operand.ShouldBeOfType<ContextEvaluatable>();
    }

    [Fact]
    public void OfPropertyName_Generates_Evaluatable_With_NestedProperty()
    {
        // Act
        var sut = Evaluatable.OfProperty("MyNestedProperty", Evaluatable.OfPropertyName("MyProperty"));

        // Assert
        sut.ShouldBeOfType<PropertyNameEvaluatable>();
        sut.PropertyName.ShouldBe("MyNestedProperty");
        sut.Operand.ShouldBeOfType<PropertyNameEvaluatable>();
        ((PropertyNameEvaluatable)sut.Operand).PropertyName.ShouldBe("MyProperty");
        ((PropertyNameEvaluatable)sut.Operand).Operand.ShouldBeOfType<ContextEvaluatable>();
    }

    [Fact]
    public void OfValue_Generates_Evaluatable_With_Literal_Value()
    {
        // Act
        var sut = Evaluatable.OfValue("Some value");

        // Assert
        sut.ShouldBeOfType<LiteralEvaluatable<string>>();
        sut.Value.ShouldBe("Some value");
    }

    [Fact]
    public void OfDelegate_Generates_Evaluatable_With_Literal_Value()
    {
        // Act
        var sut = Evaluatable.OfDelegate(() => "Some value");

        // Assert
        sut.ShouldBeOfType<DelegateEvaluatable<string>>();
        sut.Value().ShouldBe("Some value");
    }

    [Fact]
    public void OfContext_Generates_ContextEvaluatable()
    {
        // Act
        var sut = Evaluatable.OfContext();

        // Assert
        sut.ShouldBeOfType<ContextEvaluatable>();
    }
}