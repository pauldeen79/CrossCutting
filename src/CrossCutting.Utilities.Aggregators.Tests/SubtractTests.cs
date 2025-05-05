namespace CrossCutting.Utilities.Aggregators.Tests;

public class SubtractTests
{
    public class Evaluate : SubtractTests
    {
        [Fact]
        public void Returns_Correct_Result_On_Two_DateTimes()
        {
            // Arrange
            var dt1 = new DateTime(2020, 12, 1, 0, 0, 0, DateTimeKind.Local);
            var dt2 = new DateTime(1979, 2, 14, 0, 0, 0, DateTimeKind.Local);

            // Act
            var result = Subtract.Evaluate(dt1, dt2, CultureInfo.InvariantCulture);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(dt1 - dt2);
        }
    }
}
