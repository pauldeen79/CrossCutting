namespace CrossCutting.Utilities.Parsers.Tests.InsertQueryParser;

public class InsertQueryParserTests
{
    [Fact]
    public void CanParseInsertIntoValuesQuery()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D) VALUES(0.5, 'Hello world!', @SqlParameter, :OracleParameter)";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello world!'|ColumnC;@SqlParameter|Column_D;:OracleParameter");
    }

    [Fact]
    public void CanParseInsertIntoValuesQueryWithNewLinesAndTabs()
    {
        // Arrange
        const string input = @"INSERT INTO
	[mytable]
	(
		[Column A],
		[Column B],
		[Column C]
	)
	values
	(
		'A',
		'B',
		'C'
	)";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;'A'|Column B;'B'|Column C;'C'");
    }

    [Fact]
    public void CanParseInsertIntoValuesQueryWithOutputClause()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D) OUTPUT INSERTED.[Column E], INSERTED.Column_F VALUES(0.5, 'Hello world!', @SqlParameter, :OracleParameter)";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello world!'|ColumnC;@SqlParameter|Column_D;:OracleParameter");
    }

    [Fact]
    public void CanParseInsertIntoValuesQueryWithMissingColumnNames()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D) VALUES(0.5, 'Hello world!', @SqlParameter, :OracleParameter, Missing1, Missing2)";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello world!'|ColumnC;@SqlParameter|Column_D;:OracleParameter|#MISSING#;Missing1|#MISSING#;Missing2");
    }

    [Fact]
    public void CanParseInsertIntoValuesQueryWithMissingColumnValues()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D, Missing1, Missing2) VALUES(0.5, 'Hello world!', @SqlParameter, :OracleParameter)";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello world!'|ColumnC;@SqlParameter|Column_D;:OracleParameter|Missing1;#MISSING#|Missing2;#MISSING#");
    }

    [Fact]
    public void CanParseInsertIntoValuesQueryWithUnnecessarySpaces()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A],     [Column B],     ColumnC,Column_D) VALUES(0.5,            'Hello world!',             @SqlParameter,:OracleParameter)";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello world!'|ColumnC;@SqlParameter|Column_D;:OracleParameter");
    }

    [Fact]
    public void CanParseInsertIntoValuesQueryWithUnnecessaryNewLines()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]\r\n(\r\n[Column A],\r\n[Column B],\r\nColumnC,\r\nColumn_D\r\n) VALUES\r\n(\r\n0.5,\r\n'Hello world!',\r\n@SqlParameter,\r\n:OracleParameter\r\n)";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello world!'|ColumnC;@SqlParameter|Column_D;:OracleParameter");
    }

    [Fact]
    public void CanParseInsertIntoValuesQueryWithQuotedValue()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D) VALUES(0.5, 'Hello ''world!''', @SqlParameter, :OracleParameter)";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello ''world!'''|ColumnC;@SqlParameter|Column_D;:OracleParameter");
    }

    [Fact]
    public void CanParseInsertIntoSelectQuery()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D) SELECT 0.5, 'Hello world!', FieldC, FieldD FROM Tabel";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello world!'|ColumnC;FieldC|Column_D;FieldD");
    }

    [Fact]
    public void CanParseInsertIntoSelectQueryWithFunction()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D) SELECT 0.5, 'Hello world!', FieldC, COALESCE(FieldD, 'Unknown') FROM Tabel";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello world!'|ColumnC;FieldC|Column_D;COALESCE(FieldD, 'Unknown')");
    }

    [Fact]
    public void CanParseInsertIntoSelectQueryWithMissingColumnNames()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D) SELECT 0.5, 'Hello world!', FieldC, FieldD, Missing1, Missing2 FROM Tabel";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello world!'|ColumnC;FieldC|Column_D;FieldD|#MISSING#;Missing1|#MISSING#;Missing2");
    }

    [Fact]
    public void CanParseInsertIntoSelectQueryWithMissingColumnValues()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D, Missing1, Missing2) SELECT 0.5, 'Hello world!', FieldC, FieldD FROM Tabel";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;'Hello world!'|ColumnC;FieldC|Column_D;FieldD|Missing1;#MISSING#|Missing2;#MISSING#");
    }

    [Fact] // note that you probably better use ToInsertIntoString instead of string.Format in this test
    public void CanParseBackAndForth()
    {
        // Arrange
        const string InsertQuery = "INSERT INTO [Tabel](ColumnA, ColumnB, ColumnC) VALUES (ValueA, ValueB, ValueC)";

        // Act
        var parseResult = Parsers.InsertQueryParser.InsertQueryParser.Parse(InsertQuery);

        // Act some more
        var insertQuery = string.Format
        (
            "INSERT INTO [{0}]({1}) VALUES ({2})"
            , "Tabel"
            , string.Join(", ", parseResult.Values.Select(kvp => kvp.Key))
            , string.Join(", ", parseResult.Values.Select(kvp => kvp.Value))
        );

        // Assert
        insertQuery.ShouldBe(InsertQuery);
    }

    [Fact] // note that you probably better use ToInsertIntoString instead of string.Format in this test
    public void CanParseBackAndForthWithSpacesInColumnNames()
    {
        // Arrange
        const string InsertQuery = "INSERT INTO [Tabel]([Column A], [Column B], [Column C]) VALUES (ValueA, ValueB, ValueC)";

        // Act
        var parseResult = Parsers.InsertQueryParser.InsertQueryParser.Parse(InsertQuery);

        // Act some more
        var insertQuery = string.Format
        (
            "INSERT INTO [{0}]({1}) VALUES ({2})"
            , "Tabel"
            , string.Join(", ", parseResult.Values.Select(kvp => $"[{kvp.Key}]"))
            , string.Join(", ", parseResult.Values.Select(kvp => kvp.Value))
        );

        // Assert
        insertQuery.ShouldBe(InsertQuery);
    }

    [Fact]
    public void CanParseInsertIntoValuesQueryWithFunction()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D) VALUES(0.5, CONVERT('Hello world!', 0.5, SYSDATE(), 'Alsjemenou'), @SqlParameter, :OracleParameter)";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeTrue();
        actual.ErrorMessages.ShouldBeEmpty();
        var contents = string.Join("|", actual.Values.Select(kvp => string.Format("{0};{1}", kvp.Key, kvp.Value)));
        contents.ShouldBe("Column A;0.5|Column B;CONVERT('Hello world!', 0.5, SYSDATE(), 'Alsjemenou')|ColumnC;@SqlParameter|Column_D;:OracleParameter");
    }

    [Fact]
    public void CanConvertParseResultBackToInsertIntoStatement()
    {
        // Arrange
        const string input = "INSERT INTO [Tabel]([Column A], [Column B], ColumnC, Column_D) VALUES(0.5, 'Hello world!', @SqlParameter, :OracleParameter)";
        var parseResult = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.ToInsertIntoString(parseResult, "Tabel");

        // Assert
        actual.ShouldBe(input);
    }

    [Fact]
    public void CantConvertParseResultBackToInsertIntoStatementWhenResultIsNotSuccesful()
    {
        // Arrange
        var parseResult = ParseResult.Error<string, string>("Some error");

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.ToInsertIntoString(parseResult, "Tabel");

        // Assert
        actual.ShouldBe("Error: Parse result was not successful. Error messages: Some error");
    }

    [Fact]
    public void Parse_With_Empty_Argument_Gives_Error_Result()
    {
        // Arrange
        var input = string.Empty;

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        actual.ErrorMessages.First().ShouldBe("Insert query is empty");
    }

    [Fact]
    public void Parse_With_Argument_Not_Containing_Insert_Into_Gives_Error_Result()
    {
        // Arrange
        var input = "Delete from mytable";

        // Act
        var actual = Parsers.InsertQueryParser.InsertQueryParser.Parse(input);

        // Assert
        actual.IsSuccessful.ShouldBeFalse();
        actual.ErrorMessages.Count().ShouldBe(1);
        actual.ErrorMessages.First().ShouldBe("INSERT INTO clause was not found");
    }
}
