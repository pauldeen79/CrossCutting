namespace CrossCutting.Utilities.ExpressionEvaluator.Tests;

public class MemberDescriptorMapperTests : TestBase<MemberDescriptorMapper>
{
    public class Map : MemberDescriptorMapperTests, IDynamicDescriptorsProvider
    {
        [Fact]
        public void Returns_Correct_Result_On_Function_Class()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Map(new MyFunction(), null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldNotBeNull();
            result.Value.Count.ShouldBe(1);
            result.Value.First().Name.ShouldBe("MyCustomName");
            result.Value.First().Arguments.Count.ShouldBe(2);
            result.Value.First().Arguments.First().Name.ShouldBe("Argument1");
            result.Value.First().Arguments.First().Type.ShouldBe(typeof(string));
            result.Value.First().Arguments.First().IsRequired.ShouldBe(true);
            result.Value.First().Arguments.Last().Name.ShouldBe("Argument2");
            result.Value.First().Arguments.Last().Type.ShouldBe(typeof(object));
            result.Value.First().Arguments.Last().IsRequired.ShouldBe(false);
        }

        [Fact]
        public void Returns_Non_Successful_Result_From_DynamicDescriptorProvider()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Map(this, null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }

        [Fact]
        public void Returns_Non_Successful_Result_When_SourceObject_Is_Not_Function_Or_GenericFunction()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var result = sut.Map(new object(), null);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Exception occured");
            result.Exception.ShouldNotBeNull();
            result.Exception.Message.ShouldBe("MemberType cannot be Unknown");
        }

        [MemberName("MyCustomName")]
        [MemberArgument("Argument1", typeof(string), true)]
        [MemberArgument("Argument2", typeof(object), false)]
        public class MyFunction : IFunction
        {
            public Result<object?> Evaluate(FunctionCallContext context)
                => Result.NotImplemented<object?>();
        }

        [MemberName(nameof(object.ToString))]
        [MemberInstanceType(typeof(object))]
        [MemberResultType(typeof(string))]
        public static Result<object?> EvaluateToString(DotExpressionComponentState state, object sourceValue)
            => Result.Success<object?>(sourceValue.ToString(state.Context.Settings.FormatProvider));

        [MemberName(nameof(object.ToString))]
        [MemberResultType(typeof(string))]
        public static Result<object?> MissingMemberInstanceTypeAttribute(DotExpressionComponentState state, object sourceValue)
            => Result.Success<object?>(sourceValue.ToString(state.Context.Settings.FormatProvider));

        public Result<IReadOnlyCollection<MemberDescriptor>> GetDescriptors()
            => Result.Error<IReadOnlyCollection<MemberDescriptor>>("Kaboom");
    }
}
