namespace CrossCutting.Utilities.Parsers;

public static class FlatTextParser
{
    public static string[] Parse(string instance, char delimiter, char? textQualifier = null)
    {
        var result = new List<string>();
        var currentSection = new StringBuilder();
        var inText = false;
        var lastDelimiter = -1;
        var index = -1;
        foreach (var character in instance)
        {
            index++;
            if (character == delimiter && !inText)
            {
                lastDelimiter = index;
                result.Add(currentSection.ToString());
                currentSection.Clear();
            }
            else if (textQualifier != null && character == textQualifier)
            {
                // skip this character
                inText = !inText;
            }
            else
            {
                currentSection.Append(character);
            }
        }

        if (currentSection.Length > 0)
        {
            result.Add(currentSection.ToString());
        }
        else if (lastDelimiter + 1 == instance.Length && instance.Length > 0)
        {
            result.Add(string.Empty);
        }

        return result.ToArray();
    }
}
