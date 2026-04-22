namespace CrossCutting.Utilities.QueryEvaluator.Tests;

public class ComposableEvaluatableFieldNameBuilderWrapperTests : TestBase
{
    //TODO: Copy unit tests for StartsWith, DoesNotStartWith, EndsWith, DoesNotEndWith, Contains and DoesNotContain
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
}