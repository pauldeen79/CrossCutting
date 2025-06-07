using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Shouldly;
using Xunit;

namespace CrossCutting.Common.Testing.Tests;

public interface IMyInterface1 { }
public interface IMyInterface2 { }

public class TypeExtensionsTests
{
    public class CreateInstance : TypeExtensionsTests
    {
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

        [Fact]
        public void Can_Get_Mocks()
        {
            // Arrange
            var mocks = new Dictionary<Type, object?>();

            // Act
            var sut = CreateSut<MyClass>(mocks);

            // Assert
            sut.ShouldNotBeNull();
            mocks.Count.ShouldBe(2);
        }
    }

    protected T CreateSut<T>(IDictionary<Type, object?> mocks) => Testing.CreateInstance<T>(ClassFactory, mocks, p => null)!;

    // Class factory for NSubstitute, see Readme.md
    private static object? ClassFactory(Type t)
        => t.CreateInstance(parameterType => Substitute.For([parameterType], []), null, null);
}
