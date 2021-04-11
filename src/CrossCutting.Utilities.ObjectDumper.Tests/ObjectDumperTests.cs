using CrossCutting.Utilities.ObjectDumper.Contracts;
using CrossCutting.Utilities.ObjectDumper.Extensions;
using CrossCutting.Utilities.ObjectDumper.Parts.Filters;
using CrossCutting.Utilities.ObjectDumper.Parts.Transforms;
using CrossCutting.Utilities.ObjectDumper.Tests.Helpers;
using FluentAssertions;
using System;
using Xunit;

namespace CrossCutting.Utilities.ObjectDumper.Tests
{
    public class ObjectDumperTests
    {
        [Fact]
        public void CanDumpSimpleObject()
        {
            // Arrange
            var input = new
            {
                Name = "John Doe",
                Age = 21,
                Weight = 80.1
            };

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(
@"{
    ""Name"": ""John Doe"" [System.String],
    ""Age"": 21 [System.Int32],
    ""Weight"": 80.1 [System.Double]
} [AnonymousType]");
        }

        [Fact]
        public void CanDumpObjectWithNullProperty()
        {
            // Arrange
            var input = new MyType();

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(@"{
    ""Name"": NULL [System.String],
    ""Age"": 0 [System.Int32],
    ""Weight"": 0 [System.Double]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyType]");
        }

        [Fact]
        public void CanDumpNullObject()
        {
            // Arrange
            object input = null;

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be("NULL [System.Object]");
        }

        [Fact]
        public void CanDumpListOfObjects()
        {
            // Arrange
            var input = new[]
            {
                new MyType { Age = 1, Name = "Name 1", Weight = 11.1 },
                new MyType { Age = 2, Name = "Name 2", Weight = 22.2 },
                new MyType { Age = 3, Name = "Name 3", Weight = 33.3 }
            };

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(
@"[
    {
        ""Name"": ""Name 1"" [System.String],
        ""Age"": 1 [System.Int32],
        ""Weight"": 11.1 [System.Double]
    } [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyType],
    {
        ""Name"": ""Name 2"" [System.String],
        ""Age"": 2 [System.Int32],
        ""Weight"": 22.2 [System.Double]
    } [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyType],
    {
        ""Name"": ""Name 3"" [System.String],
        ""Age"": 3 [System.Int32],
        ""Weight"": 33.3 [System.Double]
    } [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyType]
] [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyType[]]");
        }

        [Fact]
        public void CanDumpNestedObject()
        {
            // Arrange
            var input = new
            {
                Name = "John Doe",
                Age = 21,
                Weight = 80.1,
                Sub = new
                {
                    Name = "Sub1",
                    Age = 1,
                    SubSub = new
                    {
                        Name = "Sub2"
                    }
                }
            };

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(
@"{
    ""Name"": ""John Doe"" [System.String],
    ""Age"": 21 [System.Int32],
    ""Weight"": 80.1 [System.Double],
    ""Sub"": 
    {
        ""Name"": ""Sub1"" [System.String],
        ""Age"": 1 [System.Int32],
        ""SubSub"": 
        {
            ""Name"": ""Sub2"" [System.String]
        } [AnonymousType]
    } [AnonymousType]
} [AnonymousType]");
        }

        [Fact]
        public void CanDumpSimpleObjectWithArrayProperty()
        {
            // Arrange
            var input = new
            {
                Name = "John Doe",
                Hobbies = new[]
                {
                    "Reading",
                    "Programming"
                }
            };

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(
@"{
    ""Name"": ""John Doe"" [System.String],
    ""Hobbies"": 
    [
        ""Reading"" [System.String],
        ""Programming"" [System.String]
    ] [System.String[]]
} [AnonymousType]");
        }

        [Fact]
        public void CanDumpObjectWithNestedArrayProperty()
        {
            // Arrange
            var input = new
            {
                Name = "John Doe",
                Children = new[]
                {
                    new
                    {
                        Name = "Child 1",
                        Age = 1
                    },
                    new
                    {
                        Name = "Child 2",
                        Age = 2
                    }
                }
            };

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(
@"{
    ""Name"": ""John Doe"" [System.String],
    ""Children"": 
    [
        {
            ""Name"": ""Child 1"" [System.String],
            ""Age"": 1 [System.Int32]
        } [AnonymousType],
        {
            ""Name"": ""Child 2"" [System.String],
            ""Age"": 2 [System.Int32]
        } [AnonymousType]
    ] [AnonymousType[]]
} [AnonymousType]");
        }

        [Fact]
        public void CanDumpWithTypeProperty()
        {
            // Arrange
            var input = new TypeWithTypeProperty
            {
                Name = "John Doe",
                Type = typeof(MyType)
            };

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(@"{
    ""Name"": ""John Doe"" [System.String],
    ""Type"": ""CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyType"" [System.RuntimeType]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.TypeWithTypeProperty]");
        }

        [Fact]
        public void CanLimitDepthOnDump()
        {
            // Arrange
            var input = new RecursiveType
            {
                Name = "Root",
                Child = new RecursiveType
                {
                    Name = "Child1",
                    Child = new RecursiveType
                    {
                        Name = "Child2"
                    }
                }
            };

            // Act
            var actual = input.Dump(new MaxDepthFilter(2));

            // Assert
            actual.Should().Be(@"{
    ""Name"": ""Root"" [System.String],
    ""Child"": 
    {
        ""Name"": ""Child1"" [System.String],
        ""Child"": 
    } [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.RecursiveType]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.RecursiveType]");
        }

        [Fact]
        public void CanSkipPropertyByName()
        {
            // Arrange
            var input = new RecursiveType
            {
                Name = "Root",
                Child = new RecursiveType
                {
                    Name = "Child1",
                    Child = new RecursiveType
                    {
                        Name = "Child2"
                    }
                }
            };

            // Act
            var actual = input.Dump(new PropertyNameExclusionFilter("Name", typeof(RecursiveType).FullName));

            // Assert
            actual.Should().Be(@"{
    ""Child"": 
    {
        ""Child"": 
        {
            ""Child"": NULL [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.RecursiveType]
        } [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.RecursiveType]
    } [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.RecursiveType]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.RecursiveType]");
        }

        [Fact]
        public void CanSkipPropertyByTypeName()
        {
            // Arrange
            var input = new RecursiveType
            {
                Name = "Root",
                Child = new RecursiveType
                {
                    Name = "Child1",
                    Child = new RecursiveType
                    {
                        Name = "Child2"
                    }
                }
            };

            // Act
            var actual = input.Dump(new PropertyTypeNameExclusionFilter(typeof(string).FullName));

            // Assert
            actual.Should().Be(@"{
    ""Child"": 
    {
        ""Child"": 
        {
            ""Child"": NULL [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.RecursiveType]
        } [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.RecursiveType]
    } [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.RecursiveType]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.RecursiveType]");
        }

        [Fact]
        public void CanDumpObjectWithPropertyThatThrowsException()
        {
            // Arrange
            var input = new TypeWithExceptionProperty { Name = "Test" };

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().StartWith(@"{
    ""Name"": ""Test"" [System.String],
    ""Error"": ""System.Reflection.TargetInvocationException: Property accessor 'Error' on object 'CrossCutting.Utilities.ObjectDumper.Tests.Helpers.TypeWithExceptionProperty' threw the following exception:'Operation is not valid due to the current state of the object.'");
            actual.Should().EndWith(@""" [System.Reflection.TargetInvocationException]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.TypeWithExceptionProperty]");
        }

        [Fact]
        public void CanDumpObjectWithSetterOnlyProperty()
        {
            // Arrange
            var input = new TypeWithSetterProperty { Name = "Test" };

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(@"{
    ""Name"": ""Test"" [System.String]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.TypeWithSetterProperty]");
        }

        [Fact]
        public void CanDumpObjectWithDynamicallyAddedProperties()
        {
            // Arrange
            var input = new MyType { Name = "Hello world" };
            using var manager = new DynamicPropertyManager<MyType>(input);
            manager.Properties.Add(DynamicPropertyManager.CreateProperty<MyType, string>("Name2", _ => "Name 2", null));
            manager.Properties.Add(DynamicPropertyManager.CreateProperty<MyType, string>("Name3", _ => "Name 3", null));

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(@"{
    ""Name"": ""Hello world"" [System.String],
    ""Age"": 0 [System.Int32],
    ""Weight"": 0 [System.Double],
    ""Name2"": ""Name 2"" [System.String],
    ""Name3"": ""Name 3"" [System.String]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyType]");
        }

        [Fact]
        public void CanDumpObjectWithErrorThrowingObjectDumperPart()
        {
            // Arrange
            var input = new
            {
                Name = "John Doe",
                Age = 21,
                Weight = 80.1
            };

            // Act
            var actual = input.Dump(new ExceptionThrowingPart());

            // Assert
            actual.Should().StartWith(@"""System.InvalidOperationException: Operation is not valid due to the current state of the object.");
            actual.Should().EndWith(@"ComposableObjectDumper.cs:line 75"" [System.InvalidOperationException]");
        }

        [Fact]
        public void CanDumpObjectUsingDelegateTransform()
        {
            // Arrange
            var input = new MyType { Name = "John Doe", Age = 21, Weight = 80.1 };
            var transform = new DelegateTransform(i => i is MyType myType
                ? new
                  {
                      myType.Name,
                      myType.Age,
                      myType.Weight,
                      AdditionalProperty = "Test"
                  }
                : i);

            // Act
            var actual = input.Dump(transform);

            // Assert
            actual.Should().Be(@"{
    ""Name"": ""John Doe"" [System.String],
    ""Age"": 21 [System.Int32],
    ""Weight"": 80.1 [System.Double],
    ""AdditionalProperty"": ""Test"" [System.String]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyType]");
        }

        [Fact]
        public void CanDumpObjectUsingTypedDelegateTransform()
        {
            // Arrange
            var input = new MyType
            {
                Name = "John Doe",
                Age = 21, 
                Weight = 80.1
            };
            var transform = new TypedDelegateTransform<MyType>(mt => mt.ToString());

            // Act
            var actual = input.Dump(transform);

            // Assert
            actual.Should().Be(@"""CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyType"" [System.String]");
        }

        [Fact]
        public void CanDumpImmutableObject()
        {
            // Arrange
            var input = new MyImmutableType("John Doe", 20);

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(
@"{
    ""Name"": ""John Doe"" [System.String],
    ""Age"": 20 [System.Int32]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyImmutableType]");
        }

        [Fact]
        public void CanDumpObjectWithEnumProperty()
        {
            // Arrange
            var input = new TypeWithEnumProperty
            {
                Property1 = "Test",
                Property2 = MyEnumeration.B
            };

            // Act
            var actual = input.Dump();

            // Assert
            actual.Should().Be(@"{
    ""Property1"": ""Test"" [System.String],
    ""Property2"": B [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.MyEnumeration]
} [CrossCutting.Utilities.ObjectDumper.Tests.Helpers.TypeWithEnumProperty]");
        }

        [Fact]
        public void CanDumpObjectWithDateTime()
        {
            var d = DateTime.Now.AddDays(-1);
            var a = new { Name = "Test", Weight = 2, Date = d };
            var b = new { Name = "Test", Weight = 2, Date = d };

            var dumpA = a.Dump();
            var dumpB = b.Dump();

            dumpB.Should().Be(dumpA);
        }

        [Fact]
        public void CanAssertMultiplePropertiesWithFluentAssertions()
        {
            var d = DateTime.Now.AddDays(-1);
            var a = new { Name = "Test", Weight = 2, Date = d }.Dump();
            var b = new { Name = "Test", Weight = 2, Date = d }.Dump();

            a.Should().BeEquivalentTo(b);
        }

        [Fact]
        public void CanAssertMultiplePropertiesWithFluentAssertions_SkipOneProperty()
        {
            var d = DateTime.Now.AddDays(-1);
            var dumpConfig = new IObjectDumperPart[] { new PropertyNameExclusionFilter("Skip") };
            var a = new { Name = "Test", Weight = 2, Date = d, Skip = "A" }.Dump(dumpConfig);
            var b = new { Name = "Test", Weight = 2, Date = d, Skip = "B" }.Dump(dumpConfig);

            a.Should().BeEquivalentTo(b);
        }

        [Fact]
        public void CanAssertMultiplePropertiesWithFluentAssertions_ExplicitlyNamePropertiesToCompare()
        {
            var d = DateTime.Now.AddDays(-1);
            var dumpConfig = new IObjectDumperPart[] { new PropertyNameFilter("Name", "Weight", "Date") };
            var a = new { Name = "Test", Weight = 2, Date = d, Skip = "A" }.Dump(dumpConfig);
            var b = new { Name = "Test", Weight = 2, Date = d, Skip = "B" }.Dump(dumpConfig);

            a.Should().BeEquivalentTo(b);
        }

        [Fact]
        public void CanAssertDifferentTypesWithSameProperties()
        {
            var d = DateTime.Now.AddDays(-1);
            var dumpConfig = new IObjectDumperPart[] { new OrderByPropertyNameTransform(t => t.FullName.Contains("Anonymous")) };
            var a = new { Name = "Test", Weight = 2, Date = d }.Dump(dumpConfig);
            var b = new { Name = "Test", Date = d, Weight = 2 }.Dump(dumpConfig);

            a.Should().BeEquivalentTo(b);
        }

        [Fact]
        public void CanAssertDifferentTypesWithSameProperties_ExplicitlyNamePropertiesToCompare()
        {
            var d = DateTime.Now.AddDays(-1);
            var dumpConfig = new IObjectDumperPart[] { new OrderByPropertyNameTransform(t => t.FullName.Contains("Anonymous")), new PropertyNameFilter("Name", "Weight", "Date") };
            var a = new { Name = "Test", Weight = 2, Date = d, Skip = "A" }.Dump(dumpConfig);
            var b = new { Name = "Test", Date = d, Weight = 2, Skip = "B" }.Dump(dumpConfig);

            a.Should().BeEquivalentTo(b);
        }
    }
}
