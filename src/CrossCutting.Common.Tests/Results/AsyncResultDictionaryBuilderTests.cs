namespace CrossCutting.Common.Tests.Results;

public class AsyncResultDictionaryBuilderTests
{
    protected static Task<Result> NonGenericTask() => Task.FromResult(Result.Success());
    protected static Task<Result<string>> GenericTask() => Task.FromResult(Result.Success(string.Empty));
    protected static Task<Result> NonGenericErrorTask() => Task.FromResult(Result.Error("Kaboom"));
    protected static Task<Result<string>> GenericErrorTask() => Task.FromResult(Result.Error<string>("Kaboom"));

    public class NonGeneric : AsyncResultDictionaryBuilderTests
    {
        public class Add_Untyped : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.Add("Test", NonGenericTask);

                // Assert
                var dictionary = await sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Throws_On_Duplicate_Key()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();
                sut.Add("Test", NonGenericTask);

                // Act & Assert
                Action a = () => sut.Add("Test", NonGenericTask);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Add_Typed : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.Add("Test", GenericTask);

                // Assert
                var dictionary = await sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Throws_On_Duplicate_Key()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();
                sut.Add("Test", GenericTask);

                // Act & Assert
                Action a = () => sut.Add("Test", GenericTask);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Build : NonGeneric
        {
            [Fact]
            public async Task Builds_Results_Correctly()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();
                sut.Add("Test1", NonGenericTask);
                sut.Add("Test2", GenericTask);

                // Act
                var result = await sut.Build();

                // Assert
                result.Count.ShouldBe(2);
            }

            [Fact]
            public async Task Stops_On_First_NonSuccessful_Result()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();
                sut.Add("Test1", NonGenericTask);
                sut.Add("Test2", GenericErrorTask); // This one returns an error
                sut.Add("Test3", GenericTask); // This one will not get executed because of the error

                // Act
                var result = await sut.Build();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }
        }

        public class BuildParallel : NonGeneric
        {
            [Fact]
            public async Task Builds_Results_Correctly()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();
                sut.Add("Test1", NonGenericTask);
                sut.Add("Test2", GenericTask);

                // Act
                var result = await sut.BuildParallel();

                // Assert
                result.Count.ShouldBe(2);
            }

            [Fact]
            public async Task Stops_On_First_NonSuccessful_Result()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();
                sut.Add("Test1", NonGenericTask);
                sut.Add("Test2", GenericErrorTask); // This one returns an error
                sut.Add("Test3", GenericTask); // This one will not get executed because of the error

                // Act
                var result = await sut.BuildParallel();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }
        }
    }

    public class Generic : AsyncResultDictionaryBuilderTests
    {
        public class Add : Generic
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", GenericTask);

                // Assert
                var dictionary = await sut.Build();
                dictionary.Count.ShouldBe(1);
                dictionary.First().Key.ShouldBe("Test");
            }

            [Fact]
            public void Throws_On_Duplicate_Key()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();
                sut.Add("Test", GenericTask);

                // Act & Assert
                Action a = () => sut.Add("Test", GenericTask);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Build : Generic
        {
            [Fact]
            public async Task Builds_Results_Correctly()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();
                sut.Add("Test1", GenericTask);
                sut.Add("Test2", GenericTask);

                // Act
                var result = await sut.Build();

                // Assert
                result.Count.ShouldBe(2);
            }

            [Fact]
            public async Task Stops_On_First_NonSuccessful_Result()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();
                sut.Add("Test1", GenericTask);
                sut.Add("Test2", GenericErrorTask); // This one returns an error
                sut.Add("Test3", GenericTask); // This one will not get executed because of the error

                // Act
                var result = await sut.Build();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }
        }

        public class BuildParallel : Generic
        {
            [Fact]
            public async Task Builds_Results_Correctly()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();
                sut.Add("Test1", GenericTask);
                sut.Add("Test2", GenericTask);

                // Act
                var result = await sut.BuildParallel();

                // Assert
                result.Count.ShouldBe(2);
            }

            [Fact]
            public async Task Stops_On_First_NonSuccessful_Result()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();
                sut.Add("Test1", GenericTask);
                sut.Add("Test2", GenericErrorTask); // This one returns an error
                sut.Add("Test3", GenericTask); // This one will not get returned because of the error

                // Act
                var result = await sut.BuildParallel();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }
        }
    }
}
