namespace CrossCutting.Utilities.Parsers.InsertQueryParser;

internal class InsertQueryParserState
{
    public StringBuilder CurrentSection { get; } = new StringBuilder();
    public bool InsertIntoFound { get; set; }
    public bool ValuesFound { get; set; }
    public bool StopInsertInto { get; set; }
    public bool ValuesOpenBracketFound { get; set; }
    public bool ValuesCloseBracketFound { get; set; }
    public bool SelectFound { get; set; }
    public bool FromFound { get; set; }
    public bool InsertIntoOpenBracketFound { get; set; }
    public bool InsertIntoCloseBracketFound { get; set; }
    public bool InValue { get; set; }
    public int OpenBracketCount { get; set; }
    public int OpenRoundBracketCount { get; set; }
    public List<string> ColumnNames { get; } = new List<string>();
    public List<string> ColumnValues { get; } = new List<string>();

    private readonly IEnumerable<IInsertQueryParserProcessor> _processors;
    private readonly IEnumerable<IInsertQueryParserResultGenerator> _resultGenerators;

    public InsertQueryParserState(IEnumerable<IInsertQueryParserProcessor> processors,
                                  IEnumerable<IInsertQueryParserResultGenerator> resultGenerators)
    {
        _processors = processors;
        _resultGenerators = resultGenerators;
    }

    public ParseResult<string, string> Process(char character)
    {
        foreach (var processor in _processors)
        {
            var result = processor.Process(character, this);
            if (result.Understood)
            {
                return result.Outcome;
            }
        }
        return ParseResult.NotUnderstood<string, string>();
    }

    public void AddColumnName()
    {
        ColumnNames.Add(CurrentSection.ToString());
        CurrentSection.Clear();
    }

    public void AddValue()
    {
        ColumnValues.Add(CurrentSection.ToString());
        CurrentSection.Clear();
    }

    public ParseResult<string, string> GetResult()
    {
        foreach (var processor in _resultGenerators)
        {
            var result = processor.Process(this);
            if (result.Understood)
            {
                return result.Outcome;
            }
        }
        return ParseResult.NotUnderstood<string, string>();
    }
}
