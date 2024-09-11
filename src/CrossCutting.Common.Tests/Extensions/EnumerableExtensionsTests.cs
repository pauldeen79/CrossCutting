using Castle.Components.DictionaryAdapter.Xml;

namespace CrossCutting.Common.Tests.Extensions;

public class EnumerableExtensionsTests
{
    [Fact]
    public void Can_Use_NotNull_On_Null_Enumerable_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string>? input = null;

        // Act
        var actual = input.NotNull();

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Can_Use_NotNull_With_Predicate_On_Null_Enumerable_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string>? input = null;

        // Act
        var actual = input.NotNull(x => x.StartsWith('A'));

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Can_Use_NotNull_With_Predicate_On_NonNull_Enumerable_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string> input = ["A", "B", "C"];

        // Act
        var actual = input.NotNull(x => x.StartsWith('A'));

        // Assert
        actual.Should().BeEquivalentTo("A");
    }

    [Fact]
    public void Can_Use_DefaultWhenNull_On_Null_Enumerable_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string>? input = null;

        // Act
        var actual = input.DefaultWhenNull();

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void Can_Use_DefaultWhenNull_On_Null_Enumerable_With_DefaultValue_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string>? input = null;

        // Act
        var actual = input.DefaultWhenNull(new[] { "a", "b", "c" });

        // Assert
        actual.Should().BeEquivalentTo("a", "b", "c");
    }

    [Fact]
    public void Can_Use_DefaultWhenNull_On_NonNull_Enumerable_With_DefaultValue_To_Work_With_Null_Easily()
    {
        // Arrange
        IEnumerable<string> input = ["a", "b", "c"];

        // Act
        var actual = input.DefaultWhenNull(new[] { "A", "B", "C" });

        // Assert
        actual.Should().BeEquivalentTo("a", "b", "c");
    }

    [Fact]
    public void WhenEmpty_Takes_Source_When_Its_Not_Empty_Using_Enumerable()
    {
        // Arrange
        var input = new[] { "1", "2", "3" };

        // Act
        var actual = input.WhenEmpty(new[] { "4", "5", "6" });

        // Assert
        actual.Should().BeEquivalentTo(input);
    }

    [Fact]
    public void WhenEmpty_Takes_Source_When_Its_Not_Empty_Using_Enumerable_Delegate()
    {
        // Arrange
        var input = new[] { "1", "2", "3" };

        // Act
        var actual = input.WhenEmpty(() => ["4", "5", "6"]);

        // Assert
        actual.Should().BeEquivalentTo(input);
    }

    [Fact]
    public void WhenEmpty_Takes_Supplied_Value_When_Its_Not_Empty_Using_Enumerable()
    {
        // Arrange
        var input = Enumerable.Empty<string>();

        // Act
        var actual = input.WhenEmpty(new[] { "4", "5", "6" });

        // Assert
        actual.Should().BeEquivalentTo("4", "5", "6");
    }

    [Fact]
    public void WhenEmpty_Takes_Supplied_Value_When_Its_Not_Empty_Using_Enumerable_Delegate()
    {
        // Arrange
        var input = Enumerable.Empty<string>();

        // Act
        var actual = input.WhenEmpty(() => ["4", "5", "6"]);

        // Assert
        actual.Should().BeEquivalentTo("4", "5", "6");
    }

    [Fact]
    public void Pipe_Return_Seed_When_No_Items_Are_Available()
    {
        // Arrange
        var items = Array.Empty<string>();
        var seed = Result.Success();

        // Act
        var result = items.Pipe(seed, (result, item) => Result.FromInstance(item), x => x.IsSuccessful());

        // Assert
        result.Should().BeSameAs(seed);
    }

    [Fact]
    public void Pipe_Returns_Last_Result_When_No_Items_Satisfy_Predicate()
    {
        // Arrange
        var items = new[] { "A", "B", "C" };
        var seed = Result.FromInstance(string.Empty);

        // Act
        var result = items.Pipe(seed, (result, item) => item == "D" ? Result.FromInstance(item) : Result.Continue(item), x => x.Status == ResultStatus.Continue);

        // Assert
        result.Value.Should().BeEquivalentTo("C");
    }

    [Fact]
    public void Pipe_Returns_Default_When_No_Items_Are_Available_And_Default_Is_Specified()
    {
        // Arrange
        var items = new[] { "A", "B", "C" };
        var seed = Result.FromInstance(string.Empty);

        // Act
        var result = items.Pipe(seed, (result, item) => item == "D" ? Result.FromInstance(item) : Result.Continue<string>(), x => x.Status == ResultStatus.Continue, _ => Result.FromInstance("Default"));

        // Assert
        result.Value.Should().BeEquivalentTo("Default");
    }

    [Fact]
    public void Pipe_Returns_First_Matching_Item_That_Satisfies_Predicate()
    {
        // Arrange
        var items = new[] { "A", "B", "C" };
        var seed = Result.FromInstance(string.Empty);

        // Act
        var result = items.Pipe(seed, (result, item) => item == "B" ? Result.FromInstance(item) : Result.Continue(item), x => x.Status == ResultStatus.Continue);

        // Assert
        result.Value.Should().BeEquivalentTo("B");
    }

    [Fact]
    public void TakeWhileWithFirstNonMatching_Throws_On_Null_Predicate()
    {
        // Arrange
        var sequence = new[] { 1, 2, 3 };

        // Act & Assert
        sequence.Invoking(x => x.TakeWhileWithFirstNonMatching(predicate: null!).ToArray())
                .Should().Throw<ArgumentNullException>().WithParameterName("predicate");
    }

    [Fact]
    public void TakeWhileWithFirstNonMatching_Returns_Matching_Items_And_The_First_Non_Matching_When_Non_Matching_Exists()
    {
        // Arrange
        var sequence = new[] { 1, 2, 3 };

        // Act
        var result = sequence.TakeWhileWithFirstNonMatching(x => x < 2).ToArray();

        // Assert
        result.Should().BeEquivalentTo(new[] { 1, 2 });
    }

    [Fact]
    public void TakeWhileWithFirstNonMatching_Returns_Everything_When_Non_Matching_Does_Not_Exist()
    {
        // Arrange
        var sequence = new[] { 1, 2, 3 };

        // Act
        var result = sequence.TakeWhileWithFirstNonMatching(x => x < 100).ToArray();

        // Assert
        result.Should().BeEquivalentTo(new[] { 1, 2, 3 });
    }
    
    [Fact]
    public async Task SelectAsync_Returns_Correct_Result()
    {
        // Arrange
        var input = new string[] { "a", "b", "c" };

        // Act
        var result = await input.SelectAsync(MyAsyncFunction);

        // Assert
        result.Should().BeEquivalentTo("A", "B", "C");
    }

    [Fact]
    public void PerformUntilFailure_Synchronous_Performs_Action_Until_First_NonSuccessful_Result()
    {
        // Arrange
        var input = new[]
        {
            "success 1",
            "success 2",
            "error 1",
            "error 2"
        };
        var counter = 0;

        // Act
        var result = input.PerformUntilFailure(x =>
        {
            counter++;
            return x.StartsWith("success") ? Result.Success(x) : Result.Error("Error");
        });

        // Assert
        result.Status.Should().Be(ResultStatus.Error); // error should be returned
        counter.Should().Be(3); // last error should be skipped
    }

    [Fact]
    public void PerformUntilFailure_Synchronous_Performs_Action_On_All_Items_When_No_NonSuccessful_Results_Are_Present()
    {
        // Arrange
        var input = new[]
        {
            "success 1",
            "success 2"
        };
        var counter = 0;

        // Act
        var result = input.PerformUntilFailure(x =>
        {
            counter++;
            return x.StartsWith("success") ? Result.Success(x) : Result.Error("Error");
        });

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.GetValue().Should().BeEquivalentTo("success 2"); // last result should be returned
        counter.Should().Be(2); // all items should have been processed
    }

    [Fact]
    public void PerformUntilFailure_Synchronous_Performs_Action_On_All_Items_When_No_NonSuccessful_Results_Are_Present_Custom_Default_Value_Provided()
    {
        // Arrange
        var input = new[]
        {
            "success 1",
            "success 2"
        };
        var counter = 0;

        // Act
        var result = input.PerformUntilFailure(Result.NotFound, x =>
        {
            counter++;
            return x.StartsWith("success") ? Result.Success(x) : Result.Error("Error");
        });

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.GetValue().Should().BeEquivalentTo("success 2"); // last result should be returned
        counter.Should().Be(2); // all items should have been processed
    }

    [Fact]
    public void PerformUntilFailure_Synchronous_Returns_Default_Value_When_No_Items_Are_Present()
    {
        // Arrange
        var input = Array.Empty<string>();
        var counter = 0;

        // Act
        var result = input.PerformUntilFailure(x =>
        {
            counter++;
            return x.StartsWith("success") ? Result.Success(x) : Result.Error("Error");
        });

        // Assert
        result.Status.Should().Be(ResultStatus.Ok); // default value should be returned
        counter.Should().Be(0); // no items to process
    }

    [Fact]
    public void PerformUntilFailure_Synchronous_Returns_Custom_Default_Value_When_No_Items_Are_Present()
    {
        // Arrange
        var input = Array.Empty<string>();
        var counter = 0;

        // Act
        var result = input.PerformUntilFailure(Result.NotFound, x =>
        {
            counter++;
            return x.StartsWith("success") ? Result.Success(x) : Result.Error("Error");
        });

        // Assert
        result.Status.Should().Be(ResultStatus.NotFound); // default value should be returned
        counter.Should().Be(0); // no items to process
    }

    [Fact]
    public async Task PerformUntilFailure_Async_Untyped_Performs_Action_Until_First_NonSuccessful_Result()
    {
        // Arrange
        IEnumerable input = new[]
        {
            "success 1",
            "success 2",
            "error 1",
            "error 2"
        };
        var counter = 0;

        // Act
        var result = await input.PerformUntilFailure(x =>
        {
            counter++;
            return Task.FromResult(x.ToStringWithDefault().StartsWith("success") ? Result.Success(x) : Result.Error("Error"));
        });

        // Assert
        result.Status.Should().Be(ResultStatus.Error); // error should be returned
        counter.Should().Be(3); // last error should be skipped
    }

    [Fact]
    public async Task PerformUntilFailure_Async_Untyped_Performs_Action_Until_First_NonSuccessful_Result_Custom_Default_Value_Provided()
    {
        // Arrange
        IEnumerable input = new[]
        {
            "success 1",
            "success 2",
            "error 1",
            "error 2"
        };
        var counter = 0;

        // Act
        var result = await input.PerformUntilFailure(() => Result.Success("default success"), x =>
        {
            counter++;
            return Task.FromResult(x.ToStringWithDefault().StartsWith("success") ? Result.Success(x) : Result.Error("Error"));
        });

        // Assert
        result.Status.Should().Be(ResultStatus.Error); // error should be returned
        counter.Should().Be(3); // last error should be skipped
    }

    [Fact]
    public async Task PerformUntilFailure_Async_Untyped_Performs_Action_Until_First_NonSuccessful_Result_No_Unsucessful_Items_Present()
    {
        // Arrange
        IEnumerable input = new[]
        {
            "success 1",
            "success 2"
        };
        var counter = 0;

        // Act
        var result = await input.PerformUntilFailure(x =>
        {
            counter++;
            return Task.FromResult(x.ToStringWithDefault().StartsWith("success") ? Result.Success(x) : Result.Error("Error"));
        });

        // Assert
        result.Status.Should().Be(ResultStatus.Ok); // error should be returned
        result.GetValue().Should().BeNull(); // no value is returned, we are just returning a new Result instance with status Ok and no value
        counter.Should().Be(2); // last error should be skipped
    }

    [Fact]
    public async Task PerformUntilFailure_Async_Untyped_Performs_Action_On_All_Items_When_No_NonSuccessful_Results_Are_Present_Custom_Default_Value_Provided()
    {
        // Arrange
        IEnumerable input = new[]
        {
            "success 1",
            "success 2"
        };
        var counter = 0;

        // Act
        var result = await input.PerformUntilFailure(() => Result.Success("default success"), x =>
        {
            counter++;
            return Task.FromResult(x.ToStringWithDefault().StartsWith("success") ? Result.Success(x) : Result.Error("Error"));
        });

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.GetValue().Should().BeEquivalentTo("default success"); // last result should be returned
        counter.Should().Be(2); // all items should have been processed
    }

    private Task<string> MyAsyncFunction(string input)
    {
        return Task.FromResult(input.ToUpperInvariant());
    }
}
