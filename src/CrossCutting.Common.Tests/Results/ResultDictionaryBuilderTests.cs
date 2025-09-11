namespace CrossCutting.Common.Tests.Results;

public class ResultDictionaryBuilderTests
{
    protected static Result NonGenericDelegate() => Result.Success();
    protected static Result NonGenericArgumentDelegate(IReadOnlyDictionary<string, Result> results) => Result.Success();
    protected static Result<string> GenericDelegate() => Result.Success(string.Empty);
    protected static Result<string> GenericArgumentDelegate(IReadOnlyDictionary<string, Result> results) => Result.Success(string.Empty);
    protected static Result<string> GenericArgumentDelegate2(IReadOnlyDictionary<string, Result<string>> results) => Result.Success(string.Empty);
    protected static Result GenericArgumentDelegate3(IReadOnlyDictionary<string, Result<string>> results) => Result.Success(string.Empty);
    protected static Result<string> GenericErrorDelegate() => Result.Error<string>("Kaboom");

    protected static IEnumerable<Result> NonGenericRangeDelegate() => [Result.Success(), Result.Success()];
    protected static IEnumerable<Result<string>> GenericRangeDelegate() => [Result.Success(string.Empty), Result.Success(string.Empty)];
    protected static IEnumerable<Result> NonGenericErrorRangeDelegate() => [Result.Success(), Result.Error("Kaboom"), Result.Success()];
    protected static IEnumerable<Result<string>> GenericErrorRangeDelegate() => [Result.Success(string.Empty), Result.Error<string>("Kaboom"), Result.Success(string.Empty)];

    public class NonGeneric : ResultDictionaryBuilderTests
    {
        public class Add : NonGeneric
        {
            [Fact]
            public void Adds_Non_Generic_Result_Delegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();

                // Act
                sut.Add("Test", NonGenericDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Adds_Non_Generic_Result_ArgumentDelegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();

                // Act
                sut.Add("Test", NonGenericArgumentDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Adds_Generic_Result_Delegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();

                // Act
                sut.Add("Test", GenericDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Adds_Generic_Result_ArgumentDelegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();

                // Act
                sut.Add(GenericArgumentDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("0001");
            }

            [Fact]
            public void Throws_On_Duplicate_Key()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();
                sut.Add("Test", NonGenericDelegate);

                // Act & Assert
                Action a = () => sut.Add("Test", NonGenericDelegate);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class AddRange : NonGeneric
        {
            [Fact]
            public void Adds_Non_Generic_Result_Delegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();

                // Act
                sut.AddRange("Test.{0}", NonGenericRangeDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(2);
                dictionary.First().Key.ShouldBe("Test.0");
                dictionary.Last().Key.ShouldBe("Test.1");
            }

            [Fact]
            public void Adds_Generic_Result_Delegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();

                // Act
                sut.AddRange("Test.{0}", GenericRangeDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(2);
                dictionary.First().Key.ShouldBe("Test.0");
                dictionary.Last().Key.ShouldBe("Test.1");
            }

            [Fact]
            public void Throws_On_Duplicate_Key()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();
                sut.AddRange("Test.{0}", NonGenericRangeDelegate);

                // Act & Assert
                Action a = () => sut.AddRange("Test.{0}", NonGenericRangeDelegate);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test.0");
            }

            [Fact]
            public void Stops_On_First_NonSuccessful_Result()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();

                // Act
                sut.AddRange("Test.{0}", NonGenericErrorRangeDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(2);
                dictionary.First().Key.ShouldBe("Test.0");
                dictionary.First().Value.Status.ShouldBe(ResultStatus.Ok);
                dictionary.Last().Key.ShouldBe("Test.1");
                dictionary.Last().Value.Status.ShouldBe(ResultStatus.Error);
            }
        }

        public class Build : NonGeneric
        {
            [Fact]
            public void Builds_Results_Correctly()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();
                sut.Add("Test1", NonGenericDelegate);
                sut.Add("Test2", GenericDelegate);

                // Act
                var result = sut.Build();

                // Assert
                result.Count.ShouldBe(2);
            }

            [Fact]
            public void Stops_On_First_NonSuccessful_Result()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();
                sut.Add("Test1", NonGenericDelegate);
                sut.Add("Test2", GenericErrorDelegate); // This one returns an error
                sut.Add("Test3", GenericDelegate); // This one will not get executed because of the error

                // Act
                var result = sut.Build();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }

            [Fact]
            public void Catches_Exception_And_Returns_Error()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder();
                sut.Add("Test1", () => throw new InvalidOperationException("Kaboom"));
                sut.Add("Test2", GenericDelegate);

                // Act
                var result = sut.Build();

                // Assert
                result.Count.ShouldBe(1);
                result.First().Value.Status.ShouldBe(ResultStatus.Error);
                result.First().Value.Exception.ShouldBeOfType<InvalidOperationException>();
                result.First().Value.Exception!.Message.ShouldBe("Kaboom");
            }
        }
    }

    public class Generic : ResultDictionaryBuilderTests
    {
        public class Add : Generic
        {
            [Fact]
            public void Adds_Non_Generic_Result_Delegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", NonGenericDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Adds_Generic_Result_Delegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", GenericDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Adds_Generic_Result_ArgumentDelegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", GenericArgumentDelegate2);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Adds_Generic_Result_ArgumentDelegate_Untyped_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", GenericArgumentDelegate3);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Throws_On_Duplicate_Key()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();
                sut.Add("Test", GenericDelegate);

                // Act & Assert
                Action a = () => sut.Add("Test", GenericDelegate);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class AddRange : Generic
        {
            [Fact]
            public void Adds_Non_Generic_Result_Delegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();

                // Act
                sut.AddRange("Test.{0}", NonGenericRangeDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(2);
                dictionary.First().Key.ShouldBe("Test.0");
                dictionary.Last().Key.ShouldBe("Test.1");
            }

            [Fact]
            public void Adds_Generic_Result_Delegate_Successfully()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();

                // Act
                sut.AddRange("Test.{0}", GenericRangeDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(2);
                dictionary.First().Key.ShouldBe("Test.0");
                dictionary.Last().Key.ShouldBe("Test.1");
            }

            [Fact]
            public void Throws_On_Duplicate_Key()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();
                sut.AddRange("Test.{0}", GenericRangeDelegate);

                // Act & Assert
                Action a = () => sut.AddRange("Test.{0}", GenericRangeDelegate);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test.0");
            }

            [Fact]
            public void Stops_On_First_NonSuccessful_Result()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();

                // Act
                sut.AddRange("Test.{0}", GenericErrorRangeDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(2);
                dictionary.First().Key.ShouldBe("Test.0");
                dictionary.First().Value.Status.ShouldBe(ResultStatus.Ok);
                dictionary.Last().Key.ShouldBe("Test.1");
                dictionary.Last().Value.Status.ShouldBe(ResultStatus.Error);
            }
        }

        public class Build : Generic
        {
            [Fact]
            public void Builds_Results_Correctly()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();
                sut.Add("Test1", NonGenericDelegate);
                sut.Add("Test2", GenericDelegate);

                // Act
                var result = sut.Build();

                // Assert
                result.Count.ShouldBe(2);
            }

            [Fact]
            public void Stops_On_First_NonSuccessful_Result()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();
                sut.Add("Test1", NonGenericDelegate);
                sut.Add("Test2", GenericErrorDelegate); // This one returns an error
                sut.Add("Test3", GenericDelegate); // This one will not get executed because of the error

                // Act
                var result = sut.Build();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }

            [Fact]
            public void Catches_Exception_And_Returns_Error()
            {
                // Arrange
                var sut = new ResultDictionaryBuilder<string>();
                sut.Add("Test1", () => throw new InvalidOperationException("Kaboom"));
                sut.Add("Test2", GenericDelegate);

                // Act
                var result = sut.Build();

                // Assert
                result.Count.ShouldBe(1);
                result.First().Value.Status.ShouldBe(ResultStatus.Error);
                result.First().Value.Exception.ShouldBeOfType<InvalidOperationException>();
                result.First().Value.Exception!.Message.ShouldBe("Kaboom");
            }
        }
    }
}
