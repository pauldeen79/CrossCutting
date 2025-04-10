namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionTokenizer;

internal sealed class ExpressionTokenizer
{
    private readonly string _input;
    private int _position;

    public ExpressionTokenizer(string input)
    {
        _input = input;
    }

    public Result<List<ExpressionToken>> Tokenize()
    {
        var tokens = new List<ExpressionToken>();
        var inQuotes = false;

        while (_position < _input.Length)
        {
            char current = _input[_position];

            if (current == '"')
            {
                inQuotes = !inQuotes;
            }

            if (char.IsWhiteSpace(current) && !inQuotes)
            {
                _position++;
                continue;
            }

            if (inQuotes)
            {
                tokens.Add(ReadOther(true));
                inQuotes = false;
                continue;
            }

            switch (current)
            {
                case '+':
                    if (char.IsNumber(Peek()))
                    {
                        tokens.Add(ReadOtherFromPlusOrMinus());
                    }
                    else
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.Plus));
                        _position++;
                    }
                    break;
                case '-':
                    if (char.IsNumber(Peek()))
                    {
                        tokens.Add(ReadOtherFromPlusOrMinus());
                    }
                    else
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.Minus));
                        _position++;
                    }
                    break;
                case '*':
                    tokens.Add(new ExpressionToken(ExpressionTokenType.Multiply));
                    _position++;
                    break;
                case '/':
                    tokens.Add(new ExpressionToken(ExpressionTokenType.Divide));
                    _position++;
                    break;
                case '(':
                    tokens.Add(new ExpressionToken(ExpressionTokenType.LeftParen));
                    _position++;
                    break;
                case ')':
                    tokens.Add(new ExpressionToken(ExpressionTokenType.RightParen));
                    _position++;
                    break;
                case '=':
                    if (Match('='))
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.EqualEqual));
                    }
                    else
                    {
                        return Result.Invalid<List<ExpressionToken>>("Unexpected '='");
                    }

                    break;
                case '!':
                    if (Match('='))
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.NotEqual));
                    }
                    else
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.Bang));
                        _position++;
                    }

                    break;
                case '<':
                    if (Match('='))
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.LessEqual));
                    }
                    else
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.Less));
                        _position++;
                    }

                    break;
                case '>':
                    if (Match('='))
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.GreaterEqual));
                    }
                    else
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.Greater));
                        _position++;
                    }

                    break;
                case '&':
                    if (Match('&'))
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.AndAnd));
                    }
                    else
                    {
                        return Result.Invalid<List<ExpressionToken>>("Single '&' is not supported.");
                    }

                    break;
                case '|':
                    if (Match('|'))
                    {
                        tokens.Add(new ExpressionToken(ExpressionTokenType.OrOr));
                    }
                    else
                    {
                        return Result.Invalid<List<ExpressionToken>>("Single '|' is not supported.");
                    }

                    break;
                default:
                    tokens.Add(ReadOther(false));
                    break;
            }
        }

        tokens.Add(new ExpressionToken(ExpressionTokenType.EOF));
        return Result.Success(tokens);
    }

    private ExpressionToken ReadOther(bool inQuotes)
    {
        var start = _position;
        while (_position < _input.Length && !IsTokenSign(_input[_position], inQuotes))
        {
            _position++;
        }

        var value = _input.Substring(start, _position - start);
        return new ExpressionToken(ExpressionTokenType.Other, value);
    }

    private ExpressionToken ReadOtherFromPlusOrMinus()
    {
        var start = _position;
        while (_position < _input.Length && (_position == start || !IsTokenSign(_input[_position], false)))
        {
            _position++;
        }

        var value = _input.Substring(start, _position - start);
        return new ExpressionToken(ExpressionTokenType.Other, value);
    }

    private static bool IsTokenSign(char current, bool inQuotes)
    {
        if (inQuotes)
        {
            return false;
        }

        return TokenSigns.Contains(current);
    }

    private bool Match(char expected)
    {
        if (_position + 1 >= _input.Length || _input[_position + 1] != expected)
        {
            return false;
        }

        _position += 2;
        return true;
    }

    private char Peek()
    {
        if (_position + 1 >= _input.Length)
        {
            return ' ';
        }

        return _input[_position + 1];
    }

    private static readonly char[] TokenSigns = ['+', '-', '*', '/', '(', ')', '=', '!', '<', '>', '&', '|'];
}
