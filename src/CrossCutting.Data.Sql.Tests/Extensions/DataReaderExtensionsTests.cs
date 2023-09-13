namespace CrossCutting.Data.Sql.Tests.Extensions;

public class DataReaderExtensionsTests
{
    [Fact]
    public void GetValue_NoDefaultValue_Returns_Default_When_Column_Is_Unknown_And_SkipUnknownColumn_Is_True()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();

        // Act
        var result = reader.GetValue<string>("UnknownColumn", skipUnknownColumn: true);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetValue_NoDefaultValue_Throws_When_Column_Is_Unknown_And_SkipUnknownColumn_Is_False()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();

        // Act & Assert
        reader.Invoking(x => x.GetValue<string>("UnknownColumn", skipUnknownColumn: false))
                     .Should().Throw<ArgumentOutOfRangeException>()
                     .And.ParamName.Should().Be("columnName");
    }

    [Fact]
    public void GetValue_NoDefaultValue_Returns_SystemDefaultValue_When_Column_Is_Known_And_Value_Is_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(true);

        // Act
        var result = reader.GetValue<string>("KnownColumn");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetValue_NoDefaultValue_Returns_Value_When_Column_Is_Known_And_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetValue(14).Returns("test");

        // Act
        var result = reader.GetValue<string>("KnownColumn");

        // Assert
        result.Should().Be("test");
    }

    [Fact]
    public void GetValue_DefaultValue_Returns_ProvidedDefaultValue_When_Column_Is_Known_And_Value_Is_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(true);

        // Act
        var result = reader.GetValue("KnownColumn", "DefaultValue");

        // Assert
        result.Should().Be("DefaultValue");
    }

    [Theory]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public void IsDBNull_Returns_Correct_Value(bool input, bool expectedOutput)
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(input);

        // Act
        var actual = reader.IsDBNull("KnownColumn");

        // Assert
        actual.Should().Be(expectedOutput);
    }

    [Fact]
    public void GetBoolean_No_DefaultValue_Throws_On_Invalid_ColumnName_When_SkipUnknownColumn_Is_False()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);

        // Act
        reader.Invoking(x => x.GetBoolean("UnknownColumn"))
                     .Should().Throw<ArgumentOutOfRangeException>()
                     .And.Message.Should().StartWith("Column [UnknownColumn] could not be found");
    }

    [Fact]
    public void GetBoolean_No_DefaultValue_Throws_On_Valid_ColumnName_When_Value_Is_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(true);

        // Act
        reader.Invoking(x => x.GetBoolean("KnownColumn"))
                     .Should().Throw<DataException>()
                     .And.Message.Should().Be("Column [KnownColumn] is DBNull");
    }

    [Fact]
    public void GetBoolean_No_DefaultValue_Returns_System_DefaultValue_On_Invalid_ColumnName_When_SkipUnknownColumn_Is_True()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(true);

        // Act
        var actual = reader.GetBoolean("UnknownColumn", skipUnknownColumn: true);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void GetBoolean_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetBoolean(14).Returns(true);

        // Act
        var actual = reader.GetBoolean("KnownColumn");

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void GetBoolean_DefaultValue_Returns_System_DefaultValue_On_Invalid_ColumnName_When_SkipUnknownColumn_Is_True()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(true);

        // Act
        var actual = reader.GetBoolean("UnknownColumn", false, skipUnknownColumn: true);

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void GetBoolean_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetBoolean(14).Returns(true);

        // Act
        var actual = reader.GetBoolean("KnownColumn", false, skipUnknownColumn: false);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void GetNullableBoolean_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetBoolean(14).Returns(true);

        // Act
        var actual = reader.GetNullableBoolean("KnownColumn", false, skipUnknownColumn: false);

        // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void GetByte_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetByte(14).Returns((byte)55);

        // Act
        var actual = reader.GetByte("KnownColumn");

        // Assert
        actual.Should().Be(55);
    }

    [Fact]
    public void GetByte_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetByte(14).Returns((byte)24);

        // Act
        var actual = reader.GetByte("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(24);
    }

    [Fact]
    public void GetNullableByte_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetByte(14).Returns((byte)123);

        // Act
        var actual = reader.GetNullableByte("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(123);
    }

    [Fact]
    public void GetDateTime_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetDateTime(14).Returns(new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Unspecified));

        // Act
        var actual = reader.GetDateTime("KnownColumn");

        // Assert
        actual.Should().Be(new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Unspecified));
    }

    [Fact]
    public void GetDateTime_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetDateTime(14).Returns(new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Unspecified));

        // Act
        var actual = reader.GetDateTime("KnownColumn", new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), skipUnknownColumn: false);

        // Assert
        actual.Should().Be(new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Unspecified));
    }

    [Fact]
    public void GetNullableDateTime_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetDateTime(14).Returns(new DateTime(2020, 3, 1, 0, 0, 0, DateTimeKind.Unspecified));

        // Act
        var actual = reader.GetNullableDateTime("KnownColumn", new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), skipUnknownColumn: false);

        // Assert
        actual.Should().Be(new DateTime(2020, 3, 1, 0, 0, 0, DateTimeKind.Unspecified));
    }

    [Fact]
    public void GetDecimal_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetDecimal(14).Returns(55);

        // Act
        var actual = reader.GetDecimal("KnownColumn");

        // Assert
        actual.Should().Be(55);
    }

    [Fact]
    public void GetDecimal_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetDecimal(14).Returns(24);

        // Act
        var actual = reader.GetDecimal("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(24);
    }

    [Fact]
    public void GetNullableDecimal_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetDecimal(14).Returns(123);

        // Act
        var actual = reader.GetNullableDecimal("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(123);
    }

    [Fact]
    public void GetDouble_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetDouble(14).Returns(55);

        // Act
        var actual = reader.GetDouble("KnownColumn");

        // Assert
        actual.Should().Be(55);
    }

    [Fact]
    public void GetDouble_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetDouble(14).Returns(24);

        // Act
        var actual = reader.GetDouble("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(24);
    }

    [Fact]
    public void GetNullableDouble_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetDouble(14).Returns(123);

        // Act
        var actual = reader.GetNullableDouble("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(123);
    }

    [Fact]
    public void GetFloat_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetFloat(14).Returns(55);

        // Act
        var actual = reader.GetFloat("KnownColumn");

        // Assert
        actual.Should().Be(55);
    }

    [Fact]
    public void GetFloat_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetFloat(14).Returns(24);

        // Act
        var actual = reader.GetFloat("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(24);
    }

    [Fact]
    public void GetNullableFloat_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetFloat(14).Returns(123);

        // Act
        var actual = reader.GetNullableFloat("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(123);
    }

    [Fact]
    public void GetGuid_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        var guid = Guid.NewGuid();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetGuid(14).Returns(guid);

        // Act
        var actual = reader.GetGuid("KnownColumn");

        // Assert
        actual.Should().Be(guid);
    }

    [Fact]
    public void GetGuid_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        var guid = Guid.NewGuid();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetGuid(14).Returns(guid);

        // Act
        var actual = reader.GetGuid("KnownColumn", Guid.Empty, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(guid);
    }

    [Fact]
    public void GetNullableGuid_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        var guid = Guid.NewGuid();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetGuid(14).Returns(guid);

        // Act
        var actual = reader.GetNullableGuid("KnownColumn", Guid.Empty, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(guid);
    }

    [Fact]
    public void GetInt16_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetInt16(14).Returns((byte)55);

        // Act
        var actual = reader.GetInt16("KnownColumn");

        // Assert
        actual.Should().Be(55);
    }

    [Fact]
    public void GetInt16_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetInt16(14).Returns((byte)24);

        // Act
        var actual = reader.GetInt16("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(24);
    }

    [Fact]
    public void GetNullableInt16_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetInt16(14).Returns((byte)123);

        // Act
        var actual = reader.GetNullableInt16("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(123);
    }

    [Fact]
    public void GetInt32_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetInt32(14).Returns(55);

        // Act
        var actual = reader.GetInt32("KnownColumn");

        // Assert
        actual.Should().Be(55);
    }

    [Fact]
    public void GetInt32_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetInt32(14).Returns(24);

        // Act
        var actual = reader.GetInt32("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(24);
    }

    [Fact]
    public void GetNullableInt32_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetInt32(14).Returns(123);

        // Act
        var actual = reader.GetNullableInt32("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(123);
    }

    [Fact]
    public void GetInt64_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetInt64(14).Returns(55);

        // Act
        var actual = reader.GetInt64("KnownColumn");

        // Assert
        actual.Should().Be(55);
    }

    [Fact]
    public void GetInt64_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetInt64(14).Returns(24);

        // Act
        var actual = reader.GetInt64("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(24);
    }

    [Fact]
    public void GetNullableInt64_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetInt64(14).Returns(123);

        // Act
        var actual = reader.GetNullableInt64("KnownColumn", 11, skipUnknownColumn: false);

        // Assert
        actual.Should().Be(123);
    }

    [Fact]
    public void GetString_No_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetString(14).Returns("test");

        // Act
        var actual = reader.GetString("KnownColumn");

        // Assert
        actual.Should().Be("test");
    }

    [Fact]
    public void GetString_DefaultValue_Returns_Value_From_Database_When_Value_Is_Not_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetString(14).Returns("test");

        // Act
        var actual = reader.GetString("KnownColumn", "Default", skipUnknownColumn: false);

        // Assert
        actual.Should().Be("test");
    }

    [Fact]
    public void GetByteArray_Returns_Empty_Array_When_Database_Value_Is_DBNull()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(true);

        // Act
        var actual = reader.GetByteArray("KnownColumn");

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void GetByteArray_Returns_Entire_Array_When_Database_Value_Is_Not_DBNull_And_Length_Is_Null()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        var expected = new byte[] { 1, 3, 2, 4, 5 };
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetValue(14).Returns(expected);

        // Act
        var actual = reader.GetByteArray("KnownColumn");

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void GetByteArray_Returns_Partial_Array_When_Length_Is_Supplied()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        var expected = new byte[] { 1, 3, 2, 4, 5 };
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);
        reader.IsDBNull(14).Returns(false);
        reader.GetBytes(14, 0, Arg.Any<byte[]>(), Arg.Any<int>(), Arg.Any<int>())
              .Returns(x =>
              {
                  expected.Take(x.ArgAt<int>(4)).ToArray().CopyTo(x.ArgAt<byte[]>(2), 0);

                  return expected.Length;
              });

        // Act
        var actual = reader.GetByteArray("KnownColumn", length: 3);

        // Assert
        actual.Should().BeEquivalentTo(expected.Take(3));
    }

    [Fact]
    public void GetByteArray_Returns_Empty_Array_When_Column_Is_Unknown_And_SkipUnknownColumn_Is_True()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);

        // Act
        var actual = reader.GetByteArray("UnnownColumn", skipUnknownColumn: true);

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void GetByteArray_Throws_When_Column_Is_Unknown_And_SkipUnknownColumn_Is_False()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();
        reader.FieldCount.Returns(1);
        reader.GetName(0).Returns("KnownColumn");
        reader.GetOrdinal("KnownColumn").Returns(14);

        // Act & Assert
        reader.Invoking(x => x.GetByteArray("UnkownColumn", skipUnknownColumn: false))
                     .Should().Throw<ArgumentOutOfRangeException>()
                     .And.Message.Should().StartWith("Column [UnkownColumn] could not be found");

    }

    [Fact]
    public void FindOne_Returns_Default_When_No_Data_Is_Found()
    {
        // Arrange
        var reader = Substitute.For<IDataReader>();

        // Act
        var result = reader.FindOne(Map);

        // Assert
        result.Should().BeNull();
    }

    private MyEntity Map(IDataReader reader)
        => new MyEntity
        {
            Property = reader.GetString("Property")
        };

    private sealed class MyEntity
    {
        public string? Property { get; set; }
    }
}
