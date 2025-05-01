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
            var result = sut.Map(new MyFunction(), null).ToArray();

            // Assert
            result.Length.ShouldBe(1);
            result[0].Name.ShouldBe("MyCustomName");
            result[0].Arguments.Count.ShouldBe(2);
            result[0].Arguments.First().Name.ShouldBe("Argument1");
            result[0].Arguments.First().Type.ShouldBe(typeof(string));
            result[0].Arguments.First().IsRequired.ShouldBe(true);
            result[0].Arguments.Last().Name.ShouldBe("Argument2");
            result[0].Arguments.Last().Type.ShouldBe(typeof(object));
            result[0].Arguments.Last().IsRequired.ShouldBe(false);
        }

        [Fact]
        public void Returns_Correct_Result_On_DotEpression_Method()
        {
            // Arrange
            var sut = this;
            var callback = CreateSut();

            // Act
            var result = sut.GetDescriptors(callback).ToArray();

            // Assert
            result.Length.ShouldBe(1);
            result[0].Name.ShouldBe("ToString");
            result[0].Arguments.Count.ShouldBe(1);
            result[0].Arguments.First().Name.ShouldBe(Constants.DotArgument);
            result[0].Arguments.First().Type.ShouldBe(typeof(object));
            result[0].Arguments.First().IsRequired.ShouldBe(true);
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

        public IEnumerable<MemberDescriptor> GetDescriptors(IMemberDescriptorCallback callback)
        {
            yield return callback.Map(EvaluateToString).GetValueOrThrow();
        }
    }
}
