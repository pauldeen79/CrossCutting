namespace CrossCutting.Utilities.Parsers.Tests;

public class FunctionCallContextTests
{
    public class Constructor : FunctionCallContextTests
    {
        [Fact]
        public void Throws_On_Null_FunctionCall()
        {
            // Act & Assert
            this.Invoking(_ => new FunctionCallContext(null!, Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), Substitute.For<IFormatProvider>(), null))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Throws_On_Null_FunctionEvaluator()
        {
            // Act & Assert
            this.Invoking(_ => new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), null!, Substitute.For<IExpressionEvaluator>(), Substitute.For<IFormatProvider>(), null))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Throws_On_Null_ExpressionEvaluator()
        {
            // Act & Assert
            this.Invoking(_ => new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), null!, Substitute.For<IFormatProvider>(), null))
                .Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Throws_On_Null_FormatProvider()
        {
            // Act & Assert
            this.Invoking(_ => new FunctionCallContext(new FunctionCallBuilder().WithName("Dummy").Build(), Substitute.For<IFunctionEvaluator>(), Substitute.For<IExpressionEvaluator>(), null!, null))
                .Should().Throw<ArgumentNullException>();
        }
    }
}
