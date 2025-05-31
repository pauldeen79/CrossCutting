namespace CrossCutting.Common.Tests.Extensions;

public class AsyncResultDictionaryExtensionsTests
{
    public class TryCastValueAsync : AsyncResultDictionaryExtensionsTests
    {
        [Fact]
        public async Task Returns_Success_When_Key_Is_Found_And_Type_Is_Correct()
        {
            // Arrange
            var sut = new AsyncResultDictionaryBuilder<object?>()
                .Add("Key", Result.Success<object?>("Some value"))
                .BuildDeferred();

            // Act
            var result = await sut.TryCastValueAsync<string>("Key");

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("Some value");
        }

        [Fact]
        public async Task Returns_Invalid_When_Key_Is_Found_But_Type_Is_Not_Correct()
        {
            // Arrange
            var sut = new AsyncResultDictionaryBuilder<object?>()
                .Add("Key", Result.Success<object?>("Some value"))
                .BuildDeferred();

            // Act
            var result = await sut.TryCastValueAsync<int>("Key");

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public async Task Returns_NotFound_When_Key_Is_Not_Found()
        {
            // Arrange
            var sut = new AsyncResultDictionaryBuilder<object?>()
                .Add("Key", Result.Success<object?>("Some value"))
                .BuildDeferred();

            // Act
            var result = await sut.TryCastValueAsync<int>("WrongKey");

            // Assert
            result.Status.ShouldBe(ResultStatus.NotFound);
        }
    }
}
