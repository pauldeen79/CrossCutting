using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrossCutting.Common.Testing.Tests;

public interface IMyInterface1 { }
public interface IMyInterface2 { }

public class TypeExtensionsTests : TestBase
{
    public class CreateInstance : TypeExtensionsTests
    {

        [Fact]
        public void Can_Get_Mocks()
        {
            // Act
            var sut = CreateSut<MyClass>();

            // Assert
            sut.ShouldNotBeNull();
            Mocks.Count.ShouldBe(2);
        }

        [Fact]
        public void Can_Construct_Class_With_String_Argument()
        {
            // Act
            var sut = CreateSut<MyClassWithStringParameter>();

            // Assert
            sut.ShouldNotBeNull();
            Mocks.ShouldBeEmpty();
        }

        [Fact]
        public void Can_Perform_NullCheck_For_Constructor_With_String_Argument()
        {
            // Act
            typeof(MyClassWithStringParameter).ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(ClassFactory);
        }
    }

    internal sealed class MyClass
    {
        public MyClass(IMyInterface1 parameter1, IEnumerable<IMyInterface2> parameter2)
        {
            Parameter1 = parameter1 ?? throw new ArgumentNullException(nameof(parameter1));
            Parameter2 = parameter2?.ToArray() ?? throw new ArgumentNullException(nameof(parameter2));
        }

        public IMyInterface1 Parameter1 { get; }
        public IMyInterface2[] Parameter2 { get; }
    }

    internal sealed class MyClassWithStringParameter
    {
        public MyClassWithStringParameter(string stringParameter)
        {
            StringParameter = stringParameter ?? throw new ArgumentNullException(nameof(stringParameter));
        }

        public string StringParameter { get; }
    }
}

public abstract class TestBase
{
    protected Dictionary<Type, object?> Mocks { get; } = new Dictionary<Type, object?>();

    protected T CreateSut<T>()
        => Testing.CreateInstance<T>(ClassFactory, Mocks, p => null)!;

    // Class factory for NSubstitute, see Readme.md
    protected object? ClassFactory(Type t)
        => t.CreateInstance(parameterType => Substitute.For([parameterType], []), Mocks, null, null);
}
