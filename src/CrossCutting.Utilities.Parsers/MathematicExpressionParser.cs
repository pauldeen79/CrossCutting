namespace CrossCutting.Utilities.Parsers;

public static class MathematicExpressionParser
{
    private const string TemporaryDelimiter = "``";

    private static readonly Dictionary<char, Func<object, object, Result<object>>> _aggregators = new()
    {
        { '^', Power },           // M
        { '*', Multiply },        // V
        { '/', Divide },          // D
        //{ '\u221A', SquareRoot }, // W
        { '+', Add },             // O
        { '-', Subtract },        // A
    };

    /*
    Er geldt tegenwoordig een nieuwe regel: 'Hoe Moeten Wij Van De Onvoldoendes Afkomen'.
H = haakjes

M = machtsverheffen
W = worteltrekken

V = vermenigvuldigen
D = delen


A = aftrekken

Bij deze nieuwe regel geldt ook de stelling: ‘ze gaan zoals ze staan’, ofwel: bereken de som in die volgorde waarin hij wordt weergegeven (van links naar rechts).

VOORBEELD

Wij willen de nieuwe regel “hoe moeten wij van de onvoldoendes afkomen” laten zien middels het volgende voorbeeld:

7 – 16 : 8 x 2 + 8=

Het is belangrijk om eerst te kijken of er bewerkingen van : (delen) en x (vermenigvuldigen  óf  + (optellen) en – (aftrekken) bij elkaar staan. Is dit het geval, dan moeten deze stukken van de som berekend worden volgens de regel: ze gaan zoals ze staan.
In deze som is dit ook het geval, namelijk 16 : 8 x 2 (delen en vermenigvuldigen staan in deze som bij elkaar).

We moeten dan eerst berekenen:

16 : 8 x 2

Let hierbij goed op de regel: ze gaan zoals ze staan, ofwel: berekenen in de volgorde waarin het staat:

16 : 8 = 2… x 2 = 4

Nu we dit deel van de som eerst hebben berekend, kunnen we de som herschrijven:

7 – 4 + 8 = 11

Het antwoord op deze som is dus 11.
     */
    public static Result<object> Parse(string input, Func<string, Result<object>> parseExpressionDelegate)
    {
        if (string.IsNullOrEmpty(input))
        {
            return Result<object>.NotFound("Input cannot be null or empty");
        }

        if (input.Contains(TemporaryDelimiter))
        {
            return Result<object>.NotSupported($"Input cannot contain {TemporaryDelimiter}, as this is used internally for formatting");
        }

        var remainder = input;
        var results = new List<Result<object>>();

        #region Handle recursion
        do
        {
            var closeIndex = input.IndexOf(")");
            if (closeIndex <= -1)
            {
                if (input.IndexOf("(") > -1)
                {
                    return Result<object>.NotFound("Missing close bracket");
                }
                continue;
            }
            
            var openIndex = input.LastIndexOf("(", closeIndex);
            if (openIndex == -1)
            {
                return Result<object>.NotFound("Missing open bracket");
            }

            var found = remainder.Substring(openIndex + 1, closeIndex - openIndex - 1);
            var subResult = Parse(found, parseExpressionDelegate);
            if (!subResult.IsSuccessful())
            {
                return subResult;
            }

            remainder = remainder.Replace($"({found})", FormattableString.Invariant($"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"));
            results.Add(subResult);

        } while (remainder.IndexOf("(") > -1 || remainder.IndexOf(")") > -1);
        #endregion

        foreach (var aggregator in _aggregators)
        {
            var index = -1;
            do
            {
                index = remainder.IndexOf(aggregator.Key);
                if (index == -1)
                {
                    continue;
                }

                var previousIndexes = _aggregators.Keys.Select(x => new
                {
                    Key = x,
                    Index = index == 0
                        ? -1
                        : remainder.LastIndexOf(x, index - 1)
                }).Where(x => x.Index > -1).OrderByDescending(x => x.Index).ToArray();

                string leftPart;
                if (previousIndexes.Any())
                {
                    leftPart = remainder.Substring(previousIndexes.First().Index + 1, index - previousIndexes.First().Index - 1).Trim();
                }
                else
                {
                    leftPart = remainder.Substring(0, index).Trim();
                }

                var nextIndexes = _aggregators.Keys.Select(x => new
                {
                    Key = x,
                    Index = index == remainder.Length
                        ? -1
                        : remainder.IndexOf(x, index + 1)
                }).Where(x => x.Index > -1).OrderBy(x => x.Index).ToArray();

                string rightPart;
                if (nextIndexes.Any())
                {
                    rightPart = remainder.Substring(index + 1, nextIndexes.First().Index - index - 1).Trim();
                }
                else
                {
                    rightPart = remainder.Substring(index + 1).Trim();
                }

                Result<object> leftPartResult;
                if (leftPart.StartsWith(TemporaryDelimiter) && leftPart.EndsWith(TemporaryDelimiter))
                {
                    leftPartResult = results[int.Parse(leftPart.Substring(TemporaryDelimiter.Length, leftPart.Length - (TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture)];
                }
                else
                {
                    leftPartResult = parseExpressionDelegate.Invoke(leftPart);
                    if (!leftPartResult.IsSuccessful())
                    {
                        return leftPartResult;
                    }
                }

                Result<object> rightPartResult;
                if (rightPart.StartsWith(TemporaryDelimiter) && rightPart.EndsWith(TemporaryDelimiter))
                {
                    rightPartResult = results[int.Parse(rightPart.Substring(TemporaryDelimiter.Length, rightPart.Length - (TemporaryDelimiter.Length * 2)), CultureInfo.InvariantCulture)];
                }
                else
                {
                    rightPartResult = parseExpressionDelegate.Invoke(rightPart);
                    if (!rightPartResult.IsSuccessful())
                    {
                        return rightPartResult;
                    }
                }

                var aggregateResult = aggregator.Value.Invoke(leftPartResult.Value!, rightPartResult.Value!);
                if (!aggregateResult.IsSuccessful())
                {
                    return aggregateResult;
                }

                remainder = string.Concat
                (
                    remainder.Substring(0, previousIndexes.Any() ? previousIndexes.First().Index + 1 : 0),
                    FormattableString.Invariant($"{TemporaryDelimiter}{results.Count}{TemporaryDelimiter}"),
                    (
                        nextIndexes.Any()
                            ? remainder.Substring(nextIndexes.First().Index)
                            : string.Empty
                    )
                );
                results.Add(aggregateResult);

            } while (index > -1);
        }

        return results.Any()
            ? results.Last()
            : parseExpressionDelegate(input);
    }

    private static Result<object> Power(object arg1, object arg2)
        => NumericAggregator.Evaluate(arg1, arg2
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => x ^ y
            , (x, y) => Math.Pow(x, y)
            , (x, y) => Math.Pow(Convert.ToDouble(x), Convert.ToDouble(y))
            , (x, y) => Math.Pow(x, y));

    private static Result<object> Multiply(object arg1, object arg2)
        => NumericAggregator.Evaluate(arg1, arg2
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y
            , (x, y) => x * y);

    private static Result<object> Divide(object arg1, object arg2)
        => NumericAggregator.Evaluate(arg1, arg2
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y
            , (x, y) => x / y);

    private static Result<object> Add(object arg1, object arg2)
        => NumericAggregator.Evaluate(arg1, arg2
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y
            , (x, y) => x + y);

    private static Result<object> Subtract(object arg1, object arg2)
        => NumericAggregator.Evaluate(arg1, arg2
            , (x, y) => x - y
            , (x, y) => x - y
            , (x, y) => x - y
            , (x, y) => x - y
            , (x, y) => x - y
            , (x, y) => x - y
            , (x, y) => x - y);
}
