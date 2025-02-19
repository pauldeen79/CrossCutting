namespace System.Data.Stub.Tests;

public sealed class DataReaderTests : IDisposable
{
    private readonly DataReader _sut;

    public DataReaderTests() => _sut = new DataReader(CommandBehavior.Default, CultureInfo.InvariantCulture);

    [Fact]
    public void Can_Get_Boolean()
    {
        // Arrange
        _sut.Add(new { Value = true });
        _sut.Read();

        // Act
        var actual = _sut.GetBoolean(0);

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public void Can_Get_Byte()
    {
        // Arrange
        _sut.Add(new { Value = 12 });
        _sut.Read();

        // Act
        var actual = _sut.GetByte(0);

        // Assert
        actual.ShouldBe((byte)12);
    }

    [Fact]
    public void GetBytes_Throws_NotImplementedException()
    {
        // Act & Assert
        Action a = () => _sut.GetBytes(0, 0, [], 0, 0);
        a.ShouldThrow<NotImplementedException>();
    }

    [Fact]
    public void Can_Get_Char()
    {
        // Arrange
        _sut.Add(new { Value = 'a' });
        _sut.Read();

        // Act
        var actual = _sut.GetChar(0);

        // Assert
        actual.ShouldBe('a');
    }

    [Fact]
    public void GetChars_Throws_NotImplementedException()
    {
        // Act & Assert
        Action a = () => _sut.GetChars(0, 0, [], 0, 0);
        a.ShouldThrow<NotImplementedException>();
    }

    [Fact]
    public void GetData_Throws_NotSupportedException()
    {
        // Act & Assert
        Action a = () => _sut.GetData(0);
        a.ShouldThrow<NotSupportedException>();
    }

    [Fact]
    public void GetDataTypeName_Returns_Correct_TypeName()
    {
        // Arrange
        _sut.Add(new { Value = 'a' });
        _sut.Read();

        // Act
        var actual = _sut.GetDataTypeName(0);

        // Assert
        actual.ShouldBe(typeof(char).FullName);
    }

    [Fact]
    public void Can_Get_DateTime()
    {
        // Arrange
        var value = DateTime.Now;
        _sut.Add(new { Value = value });
        _sut.Read();

        // Act
        var actual = _sut.GetDateTime(0);

        // Assert
        actual.ShouldBe(value);
    }

    [Fact]
    public void Can_Get_Decimal()
    {
        // Arrange
        _sut.Add(new { Value = 12.34M });
        _sut.Read();

        // Act
        var actual = _sut.GetDecimal(0);

        // Assert
        actual.ShouldBe(12.34M);
    }

    [Fact]
    public void Can_Get_Double()
    {
        // Arrange
        _sut.Add(new { Value = 12.34D });
        _sut.Read();

        // Act
        var actual = _sut.GetDouble(0);

        // Assert
        actual.ShouldBe(12.34D);
    }

    [Fact]
    public void GetFieldType_Returns_Correct_TypeName()
    {
        // Arrange
        _sut.Add(new { Value = 'a' });
        _sut.Read();

        // Act
        var actual = _sut.GetFieldType(0);

        // Assert
        actual.ShouldBe(typeof(char));
    }

    [Fact]
    public void Can_Get_Float()
    {
        // Arrange
        _sut.Add(new { Value = 12.34f });
        _sut.Read();

        // Act
        var actual = _sut.GetFloat(0);

        // Assert
        actual.ShouldBe(12.34f);
    }

    [Fact]
    public void Can_Get_Guid()
    {
        // Arrange
        var value = Guid.NewGuid();
        _sut.Add(new { Value = value });
        _sut.Read();

        // Act
        var actual = _sut.GetGuid(0);

        // Assert
        actual.ShouldBe(value);
    }

    [Fact]
    public void Can_Get_Int16()
    {
        // Arrange
        short value = 12;
        _sut.Add(new { Value = value });
        _sut.Read();

        // Act
        var actual = _sut.GetInt16(0);

        // Assert
        actual.ShouldBe(value);
    }

    [Fact]
    public void Can_Get_Int32()
    {
        // Arrange
        var value = 12;
        _sut.Add(new { Value = value });
        _sut.Read();

        // Act
        var actual = _sut.GetInt32(0);

        // Assert
        actual.ShouldBe(value);
    }

    [Fact]
    public void Can_Get_Int64()
    {
        // Arrange
        long value = 12;
        _sut.Add(new { Value = value });
        _sut.Read();

        // Act
        var actual = _sut.GetInt64(0);

        // Assert
        actual.ShouldBe(value);
    }

    [Fact]
    public void GetName_Returns_Correct_Value()
    {
        // Arrange
        _sut.Add(new { Value = 'a' });
        _sut.Read();

        // Act
        var actual = _sut.GetName(0);

        // Assert
        actual.ShouldBe("Value");
    }

    [Theory]
    [InlineData("Value", 0)]
    [InlineData("Non existing column", -1)]
    public void GetOrdinal_Returns_Correct_Value(string columnName, int expectedResult)
    {
        // Arrange
        _sut.Add(new { Value = 'a' });
        _sut.Read();

        // Act
        var actual = _sut.GetOrdinal(columnName);

        // Assert
        actual.ShouldBe(expectedResult);
    }

    [Fact]
    public void GetSchemaTable_Throws_NotImplementedException()
    {
        // Act & Assert
        Action a = () => _sut.GetSchemaTable();
        a.ShouldThrow<NotImplementedException>();
    }

    [Fact]
    public void Can_Get_String()
    {
        // Arrange
        _sut.Add(new { Value = "Hello world!" });
        _sut.Read();

        // Act
        var actual = _sut.GetString(0);

        // Assert
        actual.ShouldBe("Hello world!");
    }

    [Fact]
    public void Can_Get_Value()
    {
        // Arrange
        _sut.Add(new { Value = 12.34f });
        _sut.Read();

        // Act
        var actual = _sut.GetValue(0);

        // Assert
        actual.ShouldBe(12.34f);
    }

    [Fact]
    public void Read_Returns_False_When_No_Data_Is_Available()
    {
        // Act
        var actual = _sut.Read();

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public void Read_Returns_True_When_Data_Is_Available()
    {
        // Arrange
        _sut.Add(new { Value = 12.34f });

        // Act
        var actual = _sut.Read();

        // Assert
        actual.ShouldBeTrue();
    }

    [Fact]
    public async Task ReadAsync_Returns_False_When_No_Data_Is_Available()
    {
        // Act
        var actual = await _sut.ReadAsync();

        // Assert
        actual.ShouldBeFalse();
    }

    [Fact]
    public async Task ReadAsync_Returns_True_When_Data_Is_Available()
    {
        // Arrange
        _sut.Add(new { Value = 12.34f });

        // Act
        var actual = await _sut.ReadAsync();

        // Assert
        actual.ShouldBeTrue();
    }

    public void Dispose() => _sut.Dispose();
}
