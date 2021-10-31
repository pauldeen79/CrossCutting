using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using Xunit;

namespace CrossCutting.Common.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class ValidatableObjectExtensionsTests
    {
        [Fact]
        public void Validate_ResultsOverload_Returns_True_When_Valid()
        {
            // Arrange
            var input = new MyValidatableClass { Value = "Filled" };
            var results = new List<ValidationResult>();

            // Act
            var actual = input.Validate(results, input.Value, nameof(MyValidatableClass.Value), new[] { new RequiredAttribute() });

            // Assert
            actual.Should().BeTrue();
            results.Should().HaveCount(0);
        }

        [Fact]
        public void Validate_ResultsOverload_Returns_False_When_Invalid()
        {
            // Arrange
            var input = new MyValidatableClass { Value = null };
            var results = new List<ValidationResult>();

            // Act
            var actual = input.Validate(results, input.Value, nameof(MyValidatableClass.Value), new[] { new RequiredAttribute() });

            // Assert
            actual.Should().BeFalse();
            results.Should().HaveCount(1);
        }

        [Fact]
        public void Validate_ResultsOverload_Returns_Null_When_ValidationAttributes_Is_Null_Or_Empty()
        {
            // Arrange
            var input = new MyValidatableClass { Value = null };
            var results = new List<ValidationResult>();

            // Act
            var actual_null = input.Validate(results, input.Value, nameof(MyValidatableClass.Value), null);
            var actual_empty = input.Validate(results, input.Value, nameof(MyValidatableClass.Value), Enumerable.Empty<RequiredAttribute>());

            // Assert
            actual_null.Should().BeNull();
            actual_empty.Should().BeNull();
        }

        [Fact]
        public void Validate_MemberNameOverload_Returns_Empty_When_Valid()
        {
            // Arrange
            var input = new MyValidatableClass { Value = "Filled" };

            // Act
            var actual = input.Validate(nameof(MyValidatableClass.Value));

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void Validate_MemberNameOverload_Returns_ErrorMessage_When_Invalid()
        {
            // Arrange
            var input = new MyValidatableClass { Value = null };

            // Act
            var actual = input.Validate(nameof(MyValidatableClass.Value));

            // Assert
            actual.Should().Be("Value is required");
        }

        [Fact]
        public void Validate_NoArgumentsOverload_Returns_Empty_When_Valid()
        {
            // Arrange
            var input = new MyValidatableClass { Value = "Filled" };

            // Act
            var actual = input.Validate();

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void Validate_NoArgumentsOverload_Returns_ErrorMessage_When_Invalid()
        {
            // Arrange
            var input = new MyValidatableClass { Value = null };

            // Act
            var actual = input.Validate();

            // Assert
            actual.Should().Be("Value is required");
        }

        [Fact]
        public void Validate_NoArgumentsOverload_Returns_ErrorMessage_When_Invalid_With_Two_Errors()
        {
            // Arrange
            var input = new MyValidatableClassWithTwoProperties { Value1 = null, Value2 = null };

            // Act
            var actual = input.Validate();

            // Assert
            actual.Should().Contain("Value1 is required");
            actual.Should().Contain("Value2 is required");
        }

        [Fact]
        public void Validate_ExceptionOverload_Does_Not_Throw_When_Valid()
        {
            // Arrange
            var input = new MyValidatableClass { Value = "Filled" };

            // Act & Assert
            input.Invoking(x => x.Validate<ValidationException>())
                 .Should().NotThrow();
        }

        [Fact]
        public void Validate_ExceptionOverload_Throws_When_Invalid()
        {
            // Arrange
            var input = new MyValidatableClass { Value = null };

            // Act & Assert
            input.Invoking(x => x.Validate<ValidationException>())
                 .Should().Throw<ValidationException>();
        }

        [ExcludeFromCodeCoverage]
        private class MyValidatableClass : IValidatableObject
        {
            public string? Value { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (string.IsNullOrEmpty(Value))
                {
                    yield return new ValidationResult("Value is required", new[] { nameof(Value) });
                }
            }
        }

        [ExcludeFromCodeCoverage]
        private class MyValidatableClassWithTwoProperties : IValidatableObject
        {
            public string? Value1 { get; set; }
            public string? Value2 { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (string.IsNullOrEmpty(Value1))
                {
                    yield return new ValidationResult("Value1 is required", new[] { nameof(Value1) });
                }
                if (string.IsNullOrEmpty(Value2))
                {
                    yield return new ValidationResult("Value2 is required", new[] { nameof(Value2) });
                }
            }
        }
    }
}
