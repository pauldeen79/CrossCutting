namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Extensions;

public class DatabaseCommandExtensionsTests : TestBase
{
    public class ToPagedCommand : DatabaseCommandExtensionsTests
    {
        [Fact]
        public void Returns_Correct_Command()
        {
            // Arrange
            var sut = new SelectCommandBuilder()
                .Select("A, B, C")
                .From("MyTable")
                .Where("A = 1 AND B = @p0")
                .AppendParameter("p0", 2)
                .Build();

            // Act
            var result = sut.ToPagedCommand(20, 10);

            // Assert
            result.DataCommand.CommandText.ShouldBe(sut.CommandText);
            result.PageSize.ShouldBe(10);
            result.Offset.ShouldBe(20);
            result.RecordCountCommand.CommandText.ShouldBe("SELECT COUNT(*) FROM MyTable WHERE A = 1 AND B = @p0");
            result.RecordCountCommand.CommandType.ShouldBe(result.DataCommand.CommandType);
            result.RecordCountCommand.Operation.ShouldBe(result.DataCommand.Operation);
        }
    }
}