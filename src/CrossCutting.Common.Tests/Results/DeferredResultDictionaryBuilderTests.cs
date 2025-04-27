namespace CrossCutting.Common.Tests.Results;

public class DeferredResultDictionaryBuilderTests
{
    protected static Result NonGenericDelegate() => Result.Success();
    protected static Result<string> GenericDelegate() => Result.Success(string.Empty);
    protected static Result<string> GenericErrorDelegate() => Result.Error<string>("Kaboom");

    public class NonGeneric : DeferredResultDictionaryBuilderTests
    {
        public class Add : NonGeneric
        {
            [Fact]
            public void Adds_Non_Generic_Result_Delegate_Successfully()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder();

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
                var sut = new DeferredResultDictionaryBuilder();

                // Act
                sut.Add("Test", GenericDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Adds_Result_Successfully()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder();

                // Act
                sut.Add("Test", Result.NoContent());

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Adds_Value_Successfully()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder();

                // Act
                sut.Add("Test", new object());

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Throws_On_Duplicate_Key()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder();
                sut.Add("Test", NonGenericDelegate);

                // Act & Assert
                Action a = () => sut.Add("Test", NonGenericDelegate);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Build : NonGeneric
        {
            [Fact]
            public void Builds_Results_Correctly()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder();
                sut.Add("Test1", NonGenericDelegate);
                sut.Add("Test2", GenericDelegate);

                // Act
                var result = sut.Build();

                // Assert
                result.Count.ShouldBe(2);
            }

            [Fact]
            public void Does_Not_Stop_On_NonSuccessful_Result()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder();
                sut.Add("Test1", NonGenericDelegate);
                sut.Add("Test2", GenericErrorDelegate); // This one returns an error
                sut.Add("Test3", GenericDelegate); // This one will not get executed because of the error

                // Act
                var result = sut.Build();

                // Assert
                result.Count.ShouldBe(3);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2", "Test3" });
            }
        }
    }

    public class Generic : DeferredResultDictionaryBuilderTests
    {
        public class Add : Generic
        {
            [Fact]
            public void Adds_Generic_Result_Delegate_Successfully()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", GenericDelegate);

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Adds_Result_Successfully()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", Result.NoContent<string>());

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Adds_Value_Successfully()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", "my content");

                // Assert
                var dictionary = sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Throws_On_Duplicate_Key()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder<string>();
                sut.Add("Test", GenericDelegate);

                // Act & Assert
                Action a = () => sut.Add("Test", GenericDelegate);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Build : Generic
        {
            [Fact]
            public void Builds_Results_Correctly()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder<string>();
                sut.Add("Test1", GenericDelegate);
                sut.Add("Test2", GenericDelegate);

                // Act
                var result = sut.Build();

                // Assert
                result.Count.ShouldBe(2);
            }

            [Fact]
            public void Does_Not_Stop_On_NonSuccessful_Result()
            {
                // Arrange
                var sut = new DeferredResultDictionaryBuilder<string>();
                sut.Add("Test1", GenericDelegate);
                sut.Add("Test2", GenericErrorDelegate); // This one returns an error
                sut.Add("Test3", GenericDelegate); // This one will not get executed because of the error

                // Act
                var result = sut.Build();

                // Assert
                result.Count.ShouldBe(3);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2", "Test3" });
            }
        }
    }
}
