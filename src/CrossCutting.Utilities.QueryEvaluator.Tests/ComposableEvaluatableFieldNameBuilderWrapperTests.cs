namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public class ComposableEvaluatableFieldNameBuilderWrapperTests : TestBase
{
    public class IsEqualTo_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsEqualTo("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<EqualConditionBuilder>();
            var equalConditionBuilder = (EqualConditionBuilder)result.Conditions.Single();
            equalConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)equalConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            equalConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)equalConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class IsEqualTo_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsEqualTo(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<EqualConditionBuilder>();
            var equalConditionBuilder = (EqualConditionBuilder)result.Conditions.Single();
            equalConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)equalConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            equalConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)equalConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class IsGreaterOrEqualThan_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsGreaterOrEqualThan("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<GreaterThanOrEqualConditionBuilder>();
            var greaterThanOrEqualConditionBuilder = (GreaterThanOrEqualConditionBuilder)result.Conditions.Single();
            greaterThanOrEqualConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)greaterThanOrEqualConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            greaterThanOrEqualConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)greaterThanOrEqualConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class IsGreaterOrEqualThan_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsGreaterOrEqualThan(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<GreaterThanOrEqualConditionBuilder>();
            var greaterThanOrEqualConditionBuilder = (GreaterThanOrEqualConditionBuilder)result.Conditions.Single();
            greaterThanOrEqualConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)greaterThanOrEqualConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            greaterThanOrEqualConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)greaterThanOrEqualConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class IsGreaterThan_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsGreaterThan("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<GreaterThanConditionBuilder>();
            var greaterThanConditionBuilder = (GreaterThanConditionBuilder)result.Conditions.Single();
            greaterThanConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)greaterThanConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            greaterThanConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)greaterThanConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class IsGreaterThan_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsGreaterThan(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<GreaterThanConditionBuilder>();
            var greaterThanConditionBuilder = (GreaterThanConditionBuilder)result.Conditions.Single();
            greaterThanConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)greaterThanConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            greaterThanConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)greaterThanConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class IsSmallerOrEqualThan_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
                [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsSmallerOrEqualThan("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<SmallerThanOrEqualConditionBuilder>();
            var smallerThanOrEqualConditionBuilder = (SmallerThanOrEqualConditionBuilder)result.Conditions.Single();
            smallerThanOrEqualConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)smallerThanOrEqualConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            smallerThanOrEqualConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)smallerThanOrEqualConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class IsSmallerOrEqualThan_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsSmallerOrEqualThan(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<SmallerThanOrEqualConditionBuilder>();
            var smallerThanOrEqualConditionBuilder = (SmallerThanOrEqualConditionBuilder)result.Conditions.Single();
            smallerThanOrEqualConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)smallerThanOrEqualConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            smallerThanOrEqualConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)smallerThanOrEqualConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class IsSmallerThan_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsSmallerThan("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<SmallerThanConditionBuilder>();
            var smallerThanConditionBuilder = (SmallerThanConditionBuilder)result.Conditions.Single();
            smallerThanConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)smallerThanConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            smallerThanConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)smallerThanConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }       
    }

    public class IsSmallerThan_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsSmallerThan(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<SmallerThanConditionBuilder>();
            var smallerThanConditionBuilder = (SmallerThanConditionBuilder)result.Conditions.Single();
            smallerThanConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)smallerThanConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            smallerThanConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)smallerThanConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }    
    }

    public class IsNotEqualTo_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsNotEqualTo("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<NotEqualConditionBuilder>();
            var notEqualConditionBuilder = (NotEqualConditionBuilder)result.Conditions.Single();
            notEqualConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)notEqualConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            notEqualConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)notEqualConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class IsNotEqualTo_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsNotEqualTo(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<NotEqualConditionBuilder>();
            var notEqualConditionBuilder = (NotEqualConditionBuilder)result.Conditions.Single();
            notEqualConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)notEqualConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            notEqualConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)notEqualConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class IsNull : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsNull();

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<NullConditionBuilder>();
            var nullConditionBuilder = (NullConditionBuilder)result.Conditions.Single();
            nullConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)nullConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
        }
    }

    public class IsNotNull : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.IsNotNull();

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<NotNullConditionBuilder>();
            var notNullConditionBuilder = (NotNullConditionBuilder)result.Conditions.Single();
            notNullConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)notNullConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
        }
    }

    public class StartsWith_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.StartsWith("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringStartsWithConditionBuilder>();
            var startsWithConditionBuilder = (StringStartsWithConditionBuilder)result.Conditions.Single();
            startsWithConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)startsWithConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            startsWithConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)startsWithConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class StartsWith_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.StartsWith(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringStartsWithConditionBuilder>();
            var startsWithConditionBuilder = (StringStartsWithConditionBuilder)result.Conditions.Single();
            startsWithConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)startsWithConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            startsWithConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)startsWithConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }
    
    public class EndsWith_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.EndsWith("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringEndsWithConditionBuilder>();
            var endsWithConditionBuilder = (StringEndsWithConditionBuilder)result.Conditions.Single();
            endsWithConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)endsWithConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            endsWithConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)endsWithConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class EndsWith_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.EndsWith(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringEndsWithConditionBuilder>();
            var endsWithConditionBuilder = (StringEndsWithConditionBuilder)result.Conditions.Single();
            endsWithConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)endsWithConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            endsWithConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)endsWithConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }
    
    public class Contains_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.Contains("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringContainsConditionBuilder>();
            var containsConditionBuilder = (StringContainsConditionBuilder)result.Conditions.Single();
            containsConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)containsConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            containsConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)containsConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class Contains_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.Contains(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringContainsConditionBuilder>();
            var containsConditionBuilder = (StringContainsConditionBuilder)result.Conditions.Single();
            containsConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)containsConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            containsConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)containsConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class DoesNotStartWith_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.DoesNotStartWith("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringNotStartsWithConditionBuilder>();
            var notStartsWithConditionBuilder = (StringNotStartsWithConditionBuilder)result.Conditions.Single();
            notStartsWithConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)notStartsWithConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            notStartsWithConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)notStartsWithConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class NotStartsWith_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.DoesNotStartWith(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringNotStartsWithConditionBuilder>();
            var notStartsWithConditionBuilder = (StringNotStartsWithConditionBuilder)result.Conditions.Single();
            notStartsWithConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)notStartsWithConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            notStartsWithConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)notStartsWithConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }
    
    public class NotEndsWith_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.DoesNotEndWith("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringNotEndsWithConditionBuilder>();
            var notEndsWithConditionBuilder = (StringNotEndsWithConditionBuilder)result.Conditions.Single();
            notEndsWithConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)notEndsWithConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            notEndsWithConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)notEndsWithConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class NotEndsWith_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.DoesNotEndWith(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringNotEndsWithConditionBuilder>();
            var notEndsWithConditionBuilder = (StringNotEndsWithConditionBuilder)result.Conditions.Single();
            notEndsWithConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)notEndsWithConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            notEndsWithConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)notEndsWithConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }
    
    public class NotContains_Object : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.DoesNotContain("some value");

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringNotContainsConditionBuilder>();
            var notContainsConditionBuilder = (StringNotContainsConditionBuilder)result.Conditions.Single();
            notContainsConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)notContainsConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            notContainsConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)notContainsConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }

    public class NotContains_EvaluatableBuilder : ComposableEvaluatableFieldNameBuilderWrapperTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ComposableEvaluatableFieldNameBuilderWrapper<SingleEntityQueryBuilder>(new(), "MyProperty");

            // Act
            var result = sut.DoesNotContain(new LiteralEvaluatableBuilder("some value"));

            // Assert
            result.Conditions.Count.ShouldBe(1);
            result.Conditions.Single().ShouldBeOfType<StringNotContainsConditionBuilder>();
            var notContainsConditionBuilder = (StringNotContainsConditionBuilder)result.Conditions.Single();
            notContainsConditionBuilder.SourceExpression.ShouldBeOfType<PropertyNameEvaluatableBuilder>();
            ((PropertyNameEvaluatableBuilder)notContainsConditionBuilder.SourceExpression).PropertyName.ShouldBe("MyProperty");
            notContainsConditionBuilder.CompareExpression.ShouldBeOfType<LiteralEvaluatableBuilder>();
            ((LiteralEvaluatableBuilder)notContainsConditionBuilder.CompareExpression).Value.ShouldBe("some value");
        }
    }
}