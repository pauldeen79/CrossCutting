﻿namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class DotExpressionComponentTests : TestBase<DotExpressionComponent>
{
    public class EvaluateAsync : DotExpressionComponentTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("SomeExpressionWithoutPeriods")]
        [InlineData("\"Some expression with . within quotes\"")]
        public async Task Returns_Continue_When_Expression_Does_Not_Contain_Period_Character(string input)
        {
            // Arrange
            var context = CreateContext(input);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Invalid_When_Left_Part_Of_Expression_Is_Null_Property()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("null.MyProperty", settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            // Error message sounds a little strange, but it's just repeating the left part of the expression, which is "null" in this case :)
            result.ErrorMessage.ShouldBe("null is null, cannot evaluate property MyProperty");
        }

        [Fact]
        public async Task Returns_Invalid_When_Left_Part_Of_Expression_Is_Null_Method()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("null.MyMethod()", settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            // Error message sounds a little strange, but it's just repeating the left part of the expression, which is "null" in this case :)
            result.ErrorMessage.ShouldBe("null is null, cannot evaluate method MyMethod");
        }

        [Fact]
        public async Task Returns_Invalid_When_Property_Does_Not_Exist()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("1.MyProperty", settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Type System.Int32 does not contain property MyProperty");
        }

        [Fact]
        public async Task Returns_Error_From_First_Expression_When_Not_Successful()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("this.MyProperty", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.NotSupported);
            // The following error message actually comes from the ExpressionEvaluator, which parses the first part of the expression (this)
            result.ErrorMessage.ShouldBe("Unsupported expression: this");
        }

        [Fact]
        public async Task Returns_Error_When_Property_Throws_Exception()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("state.MyErrorProperty", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldBeOfType<TargetInvocationException>();
        }

        [Fact]
        public async Task Returns_Error_When_Method_Throws_Exception()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("state.ErrorMethod()", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldBeOfType<TargetInvocationException>();
        }

        [Fact]
        public async Task Returns_Success_When_Property_Name_Exists()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("state.MyProperty", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(MyProperty);
        }

        [Fact]
        public async Task Returns_Success_When_Property_Name_Exists_Using_Nested_Expression()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("state.MyComplexProperty.MyComplexProperty.MyProperty", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(MyProperty);
        }

        [Fact]
        public async Task Returns_Success_When_Method_Exists()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("state.ToString()", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(ToString());
        }

        [Fact]
        public async Task Returns_Invalid_When_Method_Does_Not_Exist()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("1.MyMethod()", settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Type System.Int32 does not contain method MyMethod");
        }

        [Fact]
        public async Task Returns_Invalid_When_Method_Overload_Could_Not_Be_Resolved()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("state.Overload(\"hello\")", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Method Overload on type CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents.DotExpressionComponentTests+EvaluateAsync has multiple overloads with 1 arguments, this is not supported");
        }

        [Fact]
        public async Task Returns_Invalid_When_Method_Argument_Expression_Is_Not_Successful()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("state.DoSomething(error)", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Invalid_When_Right_Expression_Is_Unrecognized()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("1.4unrecognized", settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression on type System.Object: 4unrecognized");
        }

        public int MyProperty => 13;
        public int MyErrorProperty => throw new InvalidOperationException("Kaboom");
        public EvaluateAsync MyComplexProperty => this;
#pragma warning disable xUnit1013 // Public method should be marked as test
        public void DoSomething(string argument) => System.Diagnostics.Debug.WriteLine(argument);
        public void Overload(string argument) => System.Diagnostics.Debug.WriteLine(argument);
        public void Overload(int argument) => System.Diagnostics.Debug.WriteLine(argument);
        public void ErrorMethod() => throw new InvalidOperationException("Kaboom");
#pragma warning restore xUnit1013 // Public method should be marked as test
    }

    public class ParseAsync : DotExpressionComponentTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("SomeExpressionWithoutPeriods")]
        [InlineData("\"Some expression with . within quotes\"")]
        public async Task Returns_Continue_When_Expression_Does_Not_Contain_Period_Character(string input)
        {
            // Arrange
            var context = CreateContext(input);
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Error_From_First_Expression_When_Not_Successful()
        {
            // Arrange
            var context = CreateContext("error.SomeProperty");
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            // The following error message actually comes from the ExpressionEvaluator, which parses the first part of the expression (error)
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public async Task Returns_Ok_When_Parsing_First_Expression_Succeeds_Property()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("state.MyProperty", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Ok_When_Parsing_First_Expression_Succeeds_Method()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("state.DoSomething(\"hello\")", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
        }

        [Fact]
        public async Task Returns_Invalid_When_Right_Expression_Is_Unrecognized()
        {
            // Arrange
            var context = CreateContext("1.4unrecognized");
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Unknown expression: 4unrecognized");
        }

        [Fact]
        public async Task Returns_Invalid_When_Property_Does_Not_Exist()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("1.MyProperty", settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Type System.Int32 does not contain property MyProperty");
        }

        [Fact]
        public async Task Returns_Invalid_When_Method_Does_Not_Exist()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("1.MyMethod()", settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Type System.Int32 does not contain method MyMethod");
        }

        [Fact]
        public async Task Returns_Invalid_When_Method_Overload_Could_Not_Be_Resolved()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("state.Overload(\"hello\")", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Method Overload on type CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents.DotExpressionComponentTests+ParseAsync has multiple overloads with 1 arguments, this is not supported");
        }

        [Fact]
        public async Task Returns_Invalid_When_Left_Side_Is_Null()
        {
            // Arrange
            var settings = new ExpressionEvaluatorSettingsBuilder().WithAllowReflection();
            var context = CreateContext("null.MyProperty", state: this, settings: settings);
            var sut = CreateSut();

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("null is null, cannot evaluate property MyProperty");
        }

        public int MyProperty => 13;
#pragma warning disable xUnit1013 // Public method should be marked as test
        public void DoSomething(string argument) => System.Diagnostics.Debug.WriteLine(argument);
        public void Overload(string argument) => System.Diagnostics.Debug.WriteLine(argument);
        public void Overload(int argument) => System.Diagnostics.Debug.WriteLine(argument);
#pragma warning restore xUnit1013 // Public method should be marked as test
    }
}
