namespace CrossCutting.Utilities.Parsers;

public static class StringFormatParser
{
    public static ParseResult<string, object> Parse(string formatString, params object[] args)
        => Parse(formatString, (IEnumerable<object>)args);

    public static ParseResult<string, object> Parse(string formatString, IEnumerable<object> args)
    {
        if (string.IsNullOrWhiteSpace(formatString))
        {
            return ParseResult.Error<string, object>("Format string is empty");
        }

        var state = new StringFormatParserState(args);

        foreach (var character in formatString)
        {
            if (character == '{')
            {
                state.BeginPlaceholder();
                if (state.OpenBracketCount > 1)
                {
                    return ParseResult.Error<string, object>("Too many open braces found");
                }
                else
                {
                    state.ClearCurrentSection();
                }
            }
            else if (character == '}')
            {
                state.EndPlaceholder();
                if (state.OpenBracketCount < 0)
                {
                    return ParseResult.Error<string, object>("Too many close braces found");
                }
                else
                {
                    state.ProcessCurrentSection();
                }
            }
            else if (character != '\r' && character != '\n' && character != '\t')
            {
                state.CurrentSection.Append(character);
            }
        }

        state.AddWarningsForMissingPlaceholders();

        //important: sort the placeholders! (i.e. Hello {1} {0} --> {0}, {1})
        state.SortPlaceholders();

        return state.GetResult();
    }

    public static ParseResult<string, object> ParseWithArgumentsString(string formatString, string argumentsString)
    {
        if (string.IsNullOrWhiteSpace(argumentsString))
        {
            return ParseResult.Error<string, object>("Arguments string is empty");
        }

        var currentSection = new StringBuilder();
        var inValue = false;
        var arguments = new List<string>();

        foreach (var character in argumentsString)
        {
            if (character == ',' && !inValue)
            {
                arguments.Add(currentSection.ToString().Trim());
                currentSection.Clear();
            }
            else if (character == '"' && !inValue)
            {
                inValue = true;
                currentSection.Append(character);
            }
            else if (character == '"' && inValue)
            {
                inValue = false;
                currentSection.Append(character);
            }
            else if (inValue || (character != '\r' && character != '\n' && character != '\t'))
            {
                currentSection.Append(character);
            }
        }

        if (currentSection.Length > 0)
        {
            arguments.Add(currentSection.ToString().Trim());
            currentSection.Clear();
        }

        return Parse(formatString, arguments.ToArray());
    }
}
