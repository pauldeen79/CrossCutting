namespace CrossCutting.Utilities.Aggregators.Tests;

public class NumericAggregatorTests
{
    public class Evaluate : NumericAggregatorTests
    {
        [Fact]
        public void Returns_Ok_On_Valid_Byte_Values()
        {
            // Arrange
            var left = (byte)1;
            var right = (byte)1;

            // Act
            var result = NumericAggregator.Evaluate(left, right, CultureInfo.InvariantCulture
                , (b1, b2) => b1 + b2
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }

        [Fact]
        public void Returns_Ok_On_Valid_Short_Values()
        {
            // Arrange
            var left = (short)1;
            var right = (short)1;

            // Act
            var result = NumericAggregator.Evaluate(left, right, CultureInfo.InvariantCulture
                , (_, _) => new object()
                , (s1, s2) => s1 + s2
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }

        [Fact]
        public void Returns_Ok_On_Valid_Int_Values()
        {
            // Arrange
            var left = (int)1;
            var right = (int)1;

            // Act
            var result = NumericAggregator.Evaluate(left, right, CultureInfo.InvariantCulture
                , (_, _) => new object()
                , (_, _) => new object()
                , (i1, i2) => i1 + i2
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }

        [Fact]
        public void Returns_Ok_On_Valid_Long_Values()
        {
            // Arrange
            var left = (long)1;
            var right = (long)1;

            // Act
            var result = NumericAggregator.Evaluate(left, right, CultureInfo.InvariantCulture
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (l1, l2) => l1 + l2
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }

        [Fact]
        public void Returns_Ok_On_Valid_Float_Values()
        {
            // Arrange
            var left = (float)1;
            var right = (float)1;

            // Act
            var result = NumericAggregator.Evaluate(left, right, CultureInfo.InvariantCulture
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (f1, f2) => f1 + f2
                , (_, _) => new object()
                , (_, _) => new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }

        [Fact]
        public void Returns_Ok_On_Valid_Decimal_Values()
        {
            // Arrange
            var left = (decimal)1;
            var right = (decimal)1;

            // Act
            var result = NumericAggregator.Evaluate(left, right, CultureInfo.InvariantCulture
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (d1, d2) => d1 + d2
                , (_, _) => new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }

        [Fact]
        public void Returns_Ok_On_Valid_Double_Values()
        {
            // Arrange
            var left = (double)1;
            var right = (double)1;

            // Act
            var result = NumericAggregator.Evaluate(left, right, CultureInfo.InvariantCulture
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (d1, d2) => d1 + d2);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }

        [Fact]
        public void Returns_Invalid_When_Type_Is_Not_Supported()
        {
            // Act
            var result = NumericAggregator.Evaluate("apple", "pear", CultureInfo.InvariantCulture
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Error_When_Aggregator_Throws()
        {
            // Act
            var result = NumericAggregator.Evaluate(1, 2, CultureInfo.InvariantCulture
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => throw new InvalidOperationException("Kaboom")
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object()
                , (_, _) => new object());

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Aggregation failed. Error message: Kaboom");
        }
    }

    public class Validate : NumericAggregatorTests
    {
        [Fact]
        public void Returns_NoContent_On_Null_Type1()
        {
            // Act
            var result = NumericAggregator.Validate(null, typeof(byte));

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public void Returns_NoContent_On_Null_Type2()
        {
            // Act
            var result = NumericAggregator.Validate(typeof(byte), null);

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public void Returns_NoContent_On_Unequal_Types()
        {
            // Act
            var result = NumericAggregator.Validate(typeof(int), typeof(byte));

            // Assert
            result.Status.ShouldBe(ResultStatus.NoContent);
        }

        [Fact]
        public void Returns_Invalid_On_Unsupported_Type()
        {
            // Act
            var result = NumericAggregator.Validate(typeof(string), typeof(string));

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
        }

        [Fact]
        public void Returns_Ok_On_Valid_Byte_Values()
        {
            // Arrange
            var left = (byte)1;
            var right = (byte)1;

            // Act
            var result = NumericAggregator.Validate(left.GetType(), right.GetType());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(byte));
        }

        [Fact]
        public void Returns_Ok_On_Valid_Short_Values()
        {
            // Arrange
            var left = (short)1;
            var right = (short)1;

            // Act
            var result = NumericAggregator.Validate(left.GetType(), right.GetType());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(short));
        }

        [Fact]
        public void Returns_Ok_On_Valid_Int_Values()
        {
            // Arrange
            var left = (int)1;
            var right = (int)1;

            // Act
            var result = NumericAggregator.Validate(left.GetType(), right.GetType());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(int));
        }

        [Fact]
        public void Returns_Ok_On_Valid_Long_Values()
        {
            // Arrange
            var left = (long)1;
            var right = (long)1;

            // Act
            var result = NumericAggregator.Validate(left.GetType(), right.GetType());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(long));
        }

        [Fact]
        public void Returns_Ok_On_Valid_Float_Values()
        {
            // Arrange
            var left = (float)1;
            var right = (float)1;

            // Act
            var result = NumericAggregator.Validate(left.GetType(), right.GetType());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(float));
        }

        [Fact]
        public void Returns_Ok_On_Valid_Decimal_Values()
        {
            // Arrange
            var left = (decimal)1;
            var right = (decimal)1;

            // Act
            var result = NumericAggregator.Validate(left.GetType(), right.GetType());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(decimal));
        }

        [Fact]
        public void Returns_Ok_On_Valid_Double_Values()
        {
            // Arrange
            var left = (double)1;
            var right = (double)1;

            // Act
            var result = NumericAggregator.Validate(left.GetType(), right.GetType());

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(typeof(double));
        }
    }
}
