namespace CrossCutting.Utilities.Parsers;

internal sealed class StringFormatParserState
{
    public StringBuilder CurrentSection { get; } = new StringBuilder();
    public int OpenBracketCount { get; private set; }
    public List<string> FormatPlaceholders { get; private set; } = new List<string>();
    public List<string> ValidationErrors { get; } = new List<string>();
    public List<object> FormatValues { get; }

    private bool FormatValueCountUnequalToFormatPlaceholderCount
        => FormatValues.Count > 0 && FormatPlaceholders.Count < FormatValues.Count;

    private bool FormatPlaceholderCountUnequalToFormatValueCount
        => FormatPlaceholders.Count > 0 && FormatValues.Count < FormatPlaceholders.Count;

    public StringFormatParserState(IEnumerable<object> args)
    {
        FormatValues = new List<object>(args.ToList());
    }

    public void BeginPlaceholder()
    {
        OpenBracketCount++;
    }

    public void EndPlaceholder()
    {
        OpenBracketCount--;
    }

    public void SortPlaceholders()
    {
        FormatPlaceholders = new List<string>(FormatPlaceholders.OrderBy(s => s).ToList());
    }

    public void ClearCurrentSection()
    {
        CurrentSection.Clear();
    }

    public void ProcessCurrentSection()
    {
        var name = CurrentSection.ToString();
        var doublePointSplit = name.Split(':');
        if (doublePointSplit.Length > 1)
        {
            name = doublePointSplit[0];
        }
        if (!FormatPlaceholders.Contains(name))
        {
            FormatPlaceholders.Add(name);
        }
        ClearCurrentSection();
    }

    public void AddWarningsForMissingPlaceholders()
    {
        for (int i = 0; i < FormatValues.Count; i++)
        {
            if (!FormatPlaceholders.Contains(i.ToString()))
            {
                ValidationErrors.Add($"Warning: Format value {i} was not found in format placeholders");
                FormatPlaceholders.Add(i.ToString());
            }
        }
    }

    public ParseResult<string, object> GetResult()
    {
        if (FormatValueCountUnequalToFormatPlaceholderCount)
        {
            var result = ParseResult.Error(ValidationErrors.Concat(new[] { $"Format values count ({FormatValues.Count}) is not equal to column placeholders count ({FormatPlaceholders.Count}), see #MISSING# in format placeholders list (keys)" }), FormatPlaceholders.Zip(FormatValues, (name, value) => new KeyValuePair<string, object>(name, value)));
            FormatPlaceholders.AddRange(Enumerable.Range(1, FormatValues.Count - FormatPlaceholders.Count).Select(_ => "#MISSING#"));
            return result;
        }
        else if (FormatPlaceholderCountUnequalToFormatValueCount)
        {
            var result = ParseResult.Error(ValidationErrors.Concat(new[] { $"Format placeholders count ({FormatPlaceholders.Count}) is not equal to column values count ({FormatValues.Count}), see #MISSING# in format values list (values)" }), FormatPlaceholders.Zip(FormatValues, (name, value) => new KeyValuePair<string, object>(name, value)));
            FormatValues.AddRange(Enumerable.Range(1, FormatPlaceholders.Count - FormatValues.Count).Select(_ => "#MISSING#"));
            return result;
        }
        else if (FormatPlaceholders.Count == 0)
        {
            return ParseResult.Error(ValidationErrors.Concat(new[] { "No format placeholders were found" }), Array.Empty<KeyValuePair<string, object>>());
        }

        return ParseResult.Create(ValidationErrors.Count == 0, FormatPlaceholders.Zip(FormatValues, (name, value) => new KeyValuePair<string, object>(name, value)), ValidationErrors);
    }
}
