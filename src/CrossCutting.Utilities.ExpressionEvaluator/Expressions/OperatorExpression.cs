namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public static class OperatorExpression
{
    public static bool EvaluateBooleanExpression(string expression)
    {
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));

        var result = ProcessRecursive(ref expression);

        var @operator = "&";
        foreach (var character in expression)
        {
            bool currentResult;
            switch (character)
            {
                case '&':
                    @operator = "&";
                    break;
                case '|':
                    @operator = "|";
                    break;
                case 'T':
                case 'F':
                    currentResult = character == 'T';
                    result = @operator == "&"
                        ? result && currentResult
                        : result || currentResult;
                    break;
            }
        }

        return result;
    }

    public static bool ProcessRecursive(ref string expression)
    {
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));

        var result = true;
        var openIndex = -1;
        int closeIndex;
        do
        {
            closeIndex = expression.IndexOf(")");
            if (closeIndex > -1)
            {
                openIndex = expression.LastIndexOf("(", closeIndex);
                if (openIndex > -1)
                {
                    result = EvaluateBooleanExpression(expression.Substring(openIndex + 1, closeIndex - openIndex - 1));
                    expression = string.Concat(GetPrefix(expression, openIndex),
                                               GetCurrent(result),
                                               GetSuffix(expression, closeIndex));
                }
            }
        } while (closeIndex > -1 && openIndex > -1);
        return result;
    }

    public static string GetPrefix(string expression, int openIndex)
    {
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));

        return openIndex == 0
            ? string.Empty
            : expression.Substring(0, openIndex - 2);
    }

    public static string GetCurrent(bool result)
        => result
            ? "T"
            : "F";

    public static string GetSuffix(string expression, int closeIndex)
    {
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));

        return closeIndex == expression.Length
            ? string.Empty
            : expression.Substring(closeIndex + 1);
    }
}
