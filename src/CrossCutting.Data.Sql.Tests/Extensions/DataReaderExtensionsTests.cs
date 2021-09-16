using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Data.Sql.Extensions;
using FluentAssertions;
using Moq;
using Xunit;

namespace CrossCutting.Data.Sql.Tests.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DataReaderExtensionsTests
    {
        [Fact]
        public void GetValue_NoDefaultValue_Returns_Default_When_Column_Is_Unknown_And_SkipUnknownColumn_Is_True()
        {
            // Arrange
            var reader = new Mock<IDataReader>();

            // Act
            var result = reader.Object.GetValue<string>("UnknownColumn", skipUnknownColumn: true);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetValue_NoDefaultValue_Throws_When_Column_Is_Unknown_And_SkipUnknownColumn_Is_False()
        {
            // Arrange
            var reader = new Mock<IDataReader>();

            // Act & Assert
            reader.Object.Invoking(x => x.GetValue<string>("UnknownColumn", skipUnknownColumn: false))
                         .Should().Throw<ArgumentOutOfRangeException>()
                         .And.ParamName.Should().Be("columnName");
        }

        [Fact]
        public void GetValue_NoDefaultValue_Returns_SystemDefaultValue_When_Column_Is_Known_And_Value_Is_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(true);

            // Act
            var result = reader.Object.GetValue<string>("KnownColumn");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetValue_NoDefaultValue_Returns_Value_When_Column_Is_Known_And_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetValue(14)).Returns("test");

            // Act
            var result = reader.Object.GetValue<string>("KnownColumn");

            // Assert
            result.Should().Be("test");
        }

        [Fact]
        public void GetValue_DefaultValue_Returns_ProvidedDefaultValue_When_Column_Is_Known_And_Value_Is_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(true);

            // Act
            var result = reader.Object.GetValue<string>("KnownColumn", "DefaultValue");

            // Assert
            result.Should().Be("DefaultValue");
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void IsDBNull_Returns_Correct_Value(bool input, bool expectedOutput)
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(input);

            // Act
            var actual = reader.Object.IsDBNull("KnownColumn");

            // Assert
            actual.Should().Be(expectedOutput);
        }

        [Fact]
        public void GetBoolean_No_DefaultValue_Throws_On_Invalid_ColumnName_When_SkipUnknownColumn_Is_False()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);

            // Act
            reader.Object.Invoking(x => x.GetBoolean("UnknownColumn"))
                         .Should().Throw<ArgumentOutOfRangeException>()
                         .And.Message.Should().StartWith("Column [UnknownColumn] could not be found");
        }

        [Fact]
        public void GetBoolean_No_DefaultValue_Throws_On_Valid_ColumnName_When_Value_Is_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(true);

            // Act
            reader.Object.Invoking(x => x.GetBoolean("KnownColumn"))
                         .Should().Throw<DataException>()
                         .And.Message.Should().Be("Column [KnownColumn] is DBNull");
        }

        [Fact]
        public void GetBoolean_No_DefaultValue_Returns_System_DefaultValue_On_Invalid_ColumnName_When_SkipUnknownColumn_Is_True()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(true);

            // Act
            var actual = reader.Object.GetBoolean("UnknownColumn", skipUnknownColumn: true);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void GetBoolean_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetBoolean(14)).Returns(true);

            // Act
            var actual = reader.Object.GetBoolean("KnownColumn");

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void GetBoolean_DefaultValue_Returns_System_DefaultValue_On_Invalid_ColumnName_When_SkipUnknownColumn_Is_True()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(true);

            // Act
            var actual = reader.Object.GetBoolean("UnknownColumn", false, skipUnknownColumn: true);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public void GetBoolean_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetBoolean(14)).Returns(true);

            // Act
            var actual = reader.Object.GetBoolean("KnownColumn", false, skipUnknownColumn: false);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void GetNullableBoolean_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetBoolean(14)).Returns(true);

            // Act
            var actual = reader.Object.GetNullableBoolean("KnownColumn", false, skipUnknownColumn: false);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public void GetByte_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetByte(14)).Returns(55);

            // Act
            var actual = reader.Object.GetByte("KnownColumn");

            // Assert
            actual.Should().Be(55);
        }

        [Fact]
        public void GetByte_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetByte(14)).Returns(24);

            // Act
            var actual = reader.Object.GetByte("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(24);
        }

        [Fact]
        public void GetNullableByte_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetByte(14)).Returns(123);

            // Act
            var actual = reader.Object.GetNullableByte("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(123);
        }

        [Fact]
        public void GetDateTime_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetDateTime(14)).Returns(new DateTime(2021, 2, 1));

            // Act
            var actual = reader.Object.GetDateTime("KnownColumn");

            // Assert
            actual.Should().Be(new DateTime(2021, 2, 1));
        }

        [Fact]
        public void GetDateTime_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetDateTime(14)).Returns(new DateTime(2021, 2,1));

            // Act
            var actual = reader.Object.GetDateTime("KnownColumn", new DateTime(1900, 1, 1), skipUnknownColumn: false);

            // Assert
            actual.Should().Be(new DateTime(2021, 2,1));
        }

        [Fact]
        public void GetNullableDateTime_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetDateTime(14)).Returns(new DateTime(2020, 3, 1));

            // Act
            var actual = reader.Object.GetNullableDateTime("KnownColumn", new DateTime(1900, 1, 1), skipUnknownColumn: false);

            // Assert
            actual.Should().Be(new DateTime(2020, 3, 1));
        }

        [Fact]
        public void GetDecimal_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetDecimal(14)).Returns(55);

            // Act
            var actual = reader.Object.GetDecimal("KnownColumn");

            // Assert
            actual.Should().Be(55);
        }

        [Fact]
        public void GetDecimal_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetDecimal(14)).Returns(24);

            // Act
            var actual = reader.Object.GetDecimal("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(24);
        }

        [Fact]
        public void GetNullableDecimal_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetDecimal(14)).Returns(123);

            // Act
            var actual = reader.Object.GetNullableDecimal("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(123);
        }

        [Fact]
        public void GetDouble_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetDouble(14)).Returns(55);

            // Act
            var actual = reader.Object.GetDouble("KnownColumn");

            // Assert
            actual.Should().Be(55);
        }

        [Fact]
        public void GetDouble_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetDouble(14)).Returns(24);

            // Act
            var actual = reader.Object.GetDouble("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(24);
        }

        [Fact]
        public void GetNullableDouble_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetDouble(14)).Returns(123);

            // Act
            var actual = reader.Object.GetNullableDouble("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(123);
        }

        [Fact]
        public void GetFloat_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetFloat(14)).Returns(55);

            // Act
            var actual = reader.Object.GetFloat("KnownColumn");

            // Assert
            actual.Should().Be(55);
        }

        [Fact]
        public void GetFloat_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetFloat(14)).Returns(24);

            // Act
            var actual = reader.Object.GetFloat("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(24);
        }

        [Fact]
        public void GetNullableFloat_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetFloat(14)).Returns(123);

            // Act
            var actual = reader.Object.GetNullableFloat("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(123);
        }

        [Fact]
        public void GetGuid_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            var guid = Guid.NewGuid();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetGuid(14)).Returns(guid);

            // Act
            var actual = reader.Object.GetGuid("KnownColumn");

            // Assert
            actual.Should().Be(guid);
        }

        [Fact]
        public void GetGuid_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            var guid = Guid.NewGuid();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetGuid(14)).Returns(guid);

            // Act
            var actual = reader.Object.GetGuid("KnownColumn", Guid.Empty, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(guid);
        }

        [Fact]
        public void GetNullableGuid_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            var guid = Guid.NewGuid();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetGuid(14)).Returns(guid);

            // Act
            var actual = reader.Object.GetNullableGuid("KnownColumn", Guid.Empty, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(guid);
        }
       
        [Fact]
        public void GetInt16_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetInt16(14)).Returns(55);

            // Act
            var actual = reader.Object.GetInt16("KnownColumn");

            // Assert
            actual.Should().Be(55);
        }

        [Fact]
        public void GetInt16_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetInt16(14)).Returns(24);

            // Act
            var actual = reader.Object.GetInt16("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(24);
        }

        [Fact]
        public void GetNullableInt16_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetInt16(14)).Returns(123);

            // Act
            var actual = reader.Object.GetNullableInt16("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(123);
        }

        [Fact]
        public void GetInt32_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetInt32(14)).Returns(55);

            // Act
            var actual = reader.Object.GetInt32("KnownColumn");

            // Assert
            actual.Should().Be(55);
        }

        [Fact]
        public void GetInt32_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetInt32(14)).Returns(24);

            // Act
            var actual = reader.Object.GetInt32("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(24);
        }

        [Fact]
        public void GetNullableInt32_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetInt32(14)).Returns(123);

            // Act
            var actual = reader.Object.GetNullableInt32("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(123);
        }

        [Fact]
        public void GetInt64_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetInt64(14)).Returns(55);

            // Act
            var actual = reader.Object.GetInt64("KnownColumn");

            // Assert
            actual.Should().Be(55);
        }

        [Fact]
        public void GetInt64_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetInt64(14)).Returns(24);

            // Act
            var actual = reader.Object.GetInt64("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(24);
        }

        [Fact]
        public void GetNullableInt64_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetInt64(14)).Returns(123);

            // Act
            var actual = reader.Object.GetNullableInt64("KnownColumn", 11, skipUnknownColumn: false);

            // Assert
            actual.Should().Be(123);
        }

        [Fact]
        public void GetString_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetString(14)).Returns("test");

            // Act
            var actual = reader.Object.GetString("KnownColumn");

            // Assert
            actual.Should().Be("test");
        }

        [Fact]
        public void GetString_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetString(14)).Returns("test");

            // Act
            var actual = reader.Object.GetString("KnownColumn", "Default", skipUnknownColumn: false);

            // Assert
            actual.Should().Be("test");
        }

        [Fact]
        public void GetByteArray_Returns_Empty_Array_When_Database_Value_Is_DBNull()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(true);

            // Act
            var actual = reader.Object.GetByteArray("KnownColumn");

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void GetByteArray_Returns_Entire_Array_When_Database_Value_Is_Not_DBNull_And_Length_Is_Null()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            var expected = new byte[] { 1, 3, 2, 4, 5 };
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetValue(14)).Returns(expected);

            // Act
            var actual = reader.Object.GetByteArray("KnownColumn");

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void GetByteArray_Returns_Partial_Array_When_Length_Is_Supplied()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            var expected = new byte[] { 1, 3, 2, 4, 5 };
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);
            reader.Setup(x => x.IsDBNull(14)).Returns(false);
            reader.Setup(x => x.GetBytes(14, 0, It.IsAny<byte[]>(), It.IsAny<int>(), It.IsAny<int>()))
                  .Callback<int, long, byte[], int, int>((i, fieldOffset, buffer, bufferoffset, length) =>
                  {
                      expected.Take(length).ToArray().CopyTo(buffer, 0);
                  })
                  .Returns(expected.Length);

            // Act
            var actual = reader.Object.GetByteArray("KnownColumn", length: 3);

            // Assert
            actual.Should().BeEquivalentTo(expected.Take(3));
        }

        [Fact]
        public void GetByteArray_Returns_Empty_Array_When_Column_Is_Unknown_And_SkipUnknownColumn_Is_True()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);

            // Act
            var actual = reader.Object.GetByteArray("UnnownColumn", skipUnknownColumn: true);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public void GetByteArray_Throws_When_Column_Is_Unknown_And_SkipUnknownColumn_Is_False()
        {
            // Arrange
            var reader = new Mock<IDataReader>();
            reader.SetupGet(x => x.FieldCount).Returns(1);
            reader.Setup(x => x.GetName(0)).Returns("KnownColumn");
            reader.Setup(x => x.GetOrdinal("KnownColumn")).Returns(14);

            // Act & Assert
            reader.Object.Invoking(x => x.GetByteArray("UnkownColumn", skipUnknownColumn: false))
                         .Should().Throw<ArgumentOutOfRangeException>()
                         .And.Message.Should().StartWith("Column [UnkownColumn] could not be found");

        }

        [Fact]
        public void FindOne_Throws_On_Null_MapFunction()
        {
            // Arrange
            var reader = new Mock<IDataReader>();

            // Act & Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            reader.Object.Invoking(x => x.FindOne<MyEntity>(null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                         .Should().Throw<ArgumentNullException>()
                         .And.ParamName.Should().Be("mapFunction");
        }

        [Fact]
        public void FindOne_Returns_Default_When_No_Data_Is_Found()
        {
            // Arrange
            var reader = new Mock<IDataReader>();

            // Act
            var result = reader.Object.FindOne(Map);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void FindMany_Throws_On_Null_MapFunction()
        {
            // Arrange
            var reader = new Mock<IDataReader>();

            // Act & Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            reader.Object.Invoking(x => x.FindMany<MyEntity>(null))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                         .Should().Throw<ArgumentNullException>()
                         .And.ParamName.Should().Be("mapFunction");
        }

        private MyEntity Map(IDataReader reader)
            => new MyEntity
            {
                Property = reader.GetString("Property")
            };

        private class MyEntity
        {
            public string? Property { get; set; }
        }
    }
}
