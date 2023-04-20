namespace CrossCutting.Utilities.Parsers;

public static class FormattableStringParser
{
    private const char OpenSign = '{';
    private const char CloseSign = '}';

    public static Result<string> Parse(string input, Func<string, Result<string>> placeholderDelegate)
    {
        var resultBuilder = new StringBuilder();
        var placeholderBuilder = new StringBuilder();
        var inPlaceholder = false;

        for (var index = 0; index < input.Length; index++)
        {
            var current = input[index];
            if (current == OpenSign && !NextPositionIsSign(input, index, OpenSign) && !PreviousPositionIsSign(input, index, OpenSign))
            {
                if (inPlaceholder)
                {
                    return Result<string>.NotSupported("Recursive placeholder detected, this is not supported");
                }

                inPlaceholder = true;
            }
            else if (current == CloseSign && !NextPositionIsSign(input, index, CloseSign) && !PreviousPositionIsSign(input, index, CloseSign))
            {
                if (!inPlaceholder)
                {
                    return Result<string>.Invalid("Missing open sign '{'. To use the '}' character, you have to escape it with an additional '}' character");
                }

                var placeholderResult = placeholderDelegate.Invoke(placeholderBuilder.ToString());
                if (!placeholderResult.IsSuccessful())
                {
                    return placeholderResult;
                }

                inPlaceholder = false;
                resultBuilder.Append(placeholderResult.Value!);
                placeholderBuilder.Clear();
            }
            else if (inPlaceholder)
            {
                placeholderBuilder.Append(current);
            }
            else
            {
                resultBuilder.Append(current);
            }
        }

        if (inPlaceholder)
        {
            return Result<string>.Invalid("Missing close sign '}'. To use the '{' character, you have to escape it with an additional '{' character");
        }

        return Result<string>.Success(resultBuilder.ToString());
    }

    private static bool NextPositionIsSign(string input, int index, char sign)
    {
        if (index + 1 == input.Length)
        {
            // We're at the end of the string, so this is not possible
            return false;
        }

        return input[index + 1] == sign;
    }

    private static bool PreviousPositionIsSign(string input, int index, char sign)
    {
        if (index == 0)
        {
            // We're at the end of the string, so this is not possible
            return false;
        }

        return input[index - 1] == sign;
    }
}
