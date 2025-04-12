namespace CrossCutting.Utilities.Aggregators.Tests;

public class AddTests
{
    public class Evaluate : AddTests
    {
        [Fact]
        public void Returns_Correct_Result_On_Two_Strings()
        {
            // Act
            var result = Add.Evaluate("hello ", "world!", CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("hello " + "world!");
        }
    }
}
