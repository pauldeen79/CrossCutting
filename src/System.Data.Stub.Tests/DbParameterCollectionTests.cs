namespace System.Data.Stub.Tests;

public class DbParameterCollectionTests
{
    [Fact]
    public void Can_Use_String_Indexer_To_Locate_Parameter()
    {
        // Arrange
        var sut = new DbParameterCollection
        {
            new DbDataParameter { ParameterName = "Parameter1", Value = "value1" }
        };

        // Act
        var actual = sut["Parameter1"];

        // Assert
        actual.ShouldBeOfType<DbDataParameter>();
        ((DbDataParameter)actual.Value!).Value.ShouldBe("value1");
    }

    [Fact]
    public void Can_Use_Int32_Indexer_To_Locate_Parameter()
    {
        // Arrange
        var sut = new DbParameterCollection
        {
            new DbDataParameter { ParameterName = "Parameter1", Value = "value1" }
        };

        // Act
        var actual = sut[0];

        // Assert
        actual.ShouldBeOfType<DbDataParameter>();
        ((DbDataParameter)actual.Value!).Value.ShouldBe("value1");
    }

    [Fact]
    public void Can_Count_Parameters()
    {
        // Arrange
        var sut = new DbParameterCollection
        {
            new DbDataParameter { ParameterName = "Parameter1", Value = "value1" }
        };

        // Act
        var actual = sut.Count;

        // Assert
        actual.ShouldBe(1);
    }

    [Theory, InlineData("Parameter1", true), InlineData("NonExisiting", false)]
    public void Can_Use_Contains_String(string parameterName, bool expectedResult)
    {
        // Arrange
        var sut = new DbParameterCollection
        {
            new DbDataParameter { ParameterName = "Parameter1", Value = "value1" }
        };

        // Act
        var actual = sut.Contains(parameterName);

        // Assert
        actual.ShouldBe(expectedResult);
    }

    [Theory, InlineData("value1", true), InlineData("NonExisiting", false)]
    public void Can_Use_Contains_Object(object parameterValue, bool expectedResult)
    {
        // Arrange
        var sut = new DbParameterCollection
        {
            new DbDataParameter { ParameterName = "Parameter1", Value = "value1" }
        };

        // Act
        var actual = sut.Contains(parameterValue);

        // Assert
        actual.ShouldBe(expectedResult);
    }

    [Fact]
    public void Add_Adds_Parameter()
    {
        // Arrange
        var sut = new DbParameterCollection
        {
            new DbDataParameter { ParameterName = "Parameter1", Value = "value1" },
            // Act
            "value 2"
        };

        // Assert
        sut.Count.ShouldBe(2);
    }

    [Fact]
    public void Clear_Clears_All_Parameters()
    {
        // Arrange
        var sut = new DbParameterCollection
        {
            new DbDataParameter { ParameterName = "Parameter1", Value = "value1" }
        };

        // Act
        sut.Clear();

        // Assert
        sut.Count.ShouldBe(0);
    }
}
