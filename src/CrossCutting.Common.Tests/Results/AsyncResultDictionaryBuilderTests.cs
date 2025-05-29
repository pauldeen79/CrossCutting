namespace CrossCutting.Common.Tests.Results;

public class AsyncResultDictionaryBuilderTests
{
    protected static Task<Result> NonGenericTask => Task.FromResult(Result.Success());
    protected static Task<Result<string>> GenericTask => Task.FromResult(Result.Success(string.Empty));

    protected static Func<Result> NonGenericFunc => new Func<Result>(() => Result.Success());
    protected static Func<Result<string>> GenericFunc => new Func<Result<string>>(() => Result.Success(string.Empty));

    protected static Result NonGenericResult => Result.Success();
    protected static Result<string> GenericResult => Result.Success(string.Empty);

    protected static Task<Result> NonGenericErrorTask => Task.FromResult(Result.Error("Kaboom"));
    protected static Task<Result<string>> GenericErrorTask => Task.FromResult(Result.Error<string>("Kaboom"));
    protected static Task<Result<string>> GenericExceptionTask => Task.Run(new Func<Result<string>>(() => throw new InvalidOperationException("Kaboom")));

    public class NonGeneric : AsyncResultDictionaryBuilderTests
    {
        public class Add_Untyped_Task : NonGeneric
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

        public class Add_Typed_Task : NonGeneric
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

        public class Add_Untyped_Func : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.Add("Test", NonGenericFunc);

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
                sut.Add("Test", NonGenericFunc);

                // Act & Assert
                Action a = () => sut.Add("Test", NonGenericFunc);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Add_Typed_Func : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.Add("Test", GenericFunc);

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
                sut.Add("Test", GenericFunc);

                // Act & Assert
                Action a = () => sut.Add("Test", GenericFunc);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Add_Untyped_Result : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.Add("Test", NonGenericResult);

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
                sut.Add("Test", NonGenericResult);

                // Act & Assert
                Action a = () => sut.Add("Test", NonGenericResult);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Add_Typed_Result : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.Add("Test", GenericResult);

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
                sut.Add("Test", GenericResult);

                // Act & Assert
                Action a = () => sut.Add("Test", GenericResult);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Add_Typed_Value : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.Add("Test", "GenericResult");

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
                sut.Add("Test", GenericResult);

                // Act & Assert
                Action a = () => sut.Add("Test", "GenericResult");
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }
        public class AddRange_Untyped_Task : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.AddRange("Test", [NonGenericTask]);

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
                sut.Add("Test0", NonGenericTask);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", [NonGenericTask]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
            }
        }

        public class AddRange_Typed_Task : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.AddRange("Test", [GenericTask]);

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
                sut.Add("Test0", GenericTask);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", [GenericTask]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
            }
        }

        public class AddRange_Untyped_Func : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.AddRange("Test", [NonGenericFunc]);

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
                sut.Add("Test0", NonGenericFunc);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", [NonGenericFunc]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
            }
        }

        public class AddRange_Typed_Func : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.AddRange("Test", [GenericFunc]);

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
                sut.Add("Test0", GenericFunc);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", [GenericFunc]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
            }
        }

        public class AddRange_Untyped_Result : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.AddRange("Test", [NonGenericResult]);

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
                sut.Add("Test0", NonGenericResult);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", [NonGenericResult]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
            }
        }

        public class AddRange_Typed_Result : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.AddRange("Test", [GenericResult]);

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
                sut.Add("Test0", GenericResult);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", [GenericResult]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
            }
        }

        public class AddRange_Typed_Value : NonGeneric
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();

                // Act
                sut.AddRange("Test", ["some value"]);

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
                sut.Add("Test0", GenericResult);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", ["some value"]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
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

            [Fact]
            public async Task Wraps_Exception_In_Error()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();
                sut.Add("Test1", NonGenericTask);
                sut.Add("Test2", GenericExceptionTask); // This one returns an error
                sut.Add("Test3", GenericTask); // This one will not get executed because of the error

                // Act
                var result = await sut.Build();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }
        }

        public class BuildDeferred : NonGeneric
        {
            [Fact]
            public void Builds_Results_Correctly()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();
                sut.Add("Test1", NonGenericTask);
                sut.Add("Test2", GenericTask);

                // Act
                var result = sut.BuildDeferred();

                // Assert
                result.Count.ShouldBe(2);
            }
        }

        public class BuildLazy : NonGeneric
        {
            [Fact]
            public async Task Builds_Results_Correctly()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();
                sut.Add("Test1", NonGenericTask);
                sut.Add("Test2", GenericTask);

                // Act
                var result = await sut.BuildLazy();

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
                var result = await sut.BuildLazy();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }

            [Fact]
            public async Task Wraps_Exception_In_Error()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder();
                sut.Add("Test1", NonGenericTask);
                sut.Add("Test2", GenericExceptionTask); // This one returns an error
                sut.Add("Test3", GenericTask); // This one will not get executed because of the error

                // Act
                var result = await sut.BuildLazy();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }
        }
    }

    public class Generic : AsyncResultDictionaryBuilderTests
    {
        public class Add_Task : Generic
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

        public class Add_Func : Generic
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", GenericFunc);

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
                sut.Add("Test", GenericFunc);

                // Act & Assert
                Action a = () => sut.Add("Test", GenericFunc);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Add_Result : Generic
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", GenericResult);

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
                sut.Add("Test", GenericResult);

                // Act & Assert
                Action a = () => sut.Add("Test", GenericResult);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class Add_Value : Generic
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();

                // Act
                sut.Add("Test", "some value");

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
                sut.Add("Test", GenericResult);

                // Act & Assert
                Action a = () => sut.Add("Test", "some value");
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test");
            }
        }

        public class AddRange_Task : Generic
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();

                // Act
                sut.AddRange("Test", [GenericTask]);

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
                sut.Add("Test0", GenericTask);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", [GenericTask]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
            }
        }

        public class AddRange_Func : Generic
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();

                // Act
                sut.AddRange("Test", [GenericFunc]);

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
                sut.Add("Test0", GenericFunc);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", [GenericFunc]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
            }
        }

        public class AddRange_Result : Generic
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();

                // Act
                sut.AddRange("Test", [GenericResult]);

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
                sut.Add("Test0", GenericResult);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", [GenericResult]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
            }
        }

        public class AddRange_Value : Generic
        {
            [Fact]
            public async Task Adds_Result_Task_Successfully()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();

                // Act
                sut.AddRange("Test", ["some value"]);

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
                sut.Add("Test0", GenericResult);

                // Act & Assert
                Action a = () => sut.AddRange("Test{0}", ["some value"]);
                a.ShouldThrow<ArgumentException>()
                 .Message.ShouldBe("An item with the same key has already been added. Key: Test0");
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

            [Fact]
            public async Task Wraps_Exception_In_Error()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();
                sut.Add("Test1", GenericTask);
                sut.Add("Test2", GenericExceptionTask); // This one returns an error
                sut.Add("Test3", GenericTask); // This one will not get executed because of the error

                // Act
                var result = await sut.Build();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }
        }

        public class BuildDeferred : Generic
        {
            [Fact]
            public void Builds_Results_Correctly()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();
                sut.Add("Test1", GenericTask);
                sut.Add("Test2", GenericTask);

                // Act
                var result = sut.BuildDeferred();

                // Assert
                result.Count.ShouldBe(2);
            }
        }

        public class BuildLazy : Generic
        {
            [Fact]
            public async Task Builds_Results_Correctly()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();
                sut.Add("Test1", GenericTask);
                sut.Add("Test2", GenericTask);

                // Act
                var result = await sut.BuildLazy();

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
                var result = await sut.BuildLazy();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }

            [Fact]
            public async Task Wraps_Exception_In_Error()
            {
                // Arrange
                var sut = new AsyncResultDictionaryBuilder<string>();
                sut.Add("Test1", GenericTask);
                sut.Add("Test2", GenericExceptionTask); // This one returns an error
                sut.Add("Test3", GenericTask); // This one will not get executed because of the error

                // Act
                var result = await sut.BuildLazy();

                // Assert
                result.Count.ShouldBe(2);
                result.Keys.ToArray().ShouldBeEquivalentTo(new[] { "Test1", "Test2" });
            }
        }
    }
}
