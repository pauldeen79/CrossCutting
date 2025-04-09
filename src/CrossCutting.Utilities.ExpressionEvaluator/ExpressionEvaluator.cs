namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IExpression[] _expressions;

    public ExpressionEvaluator(IEnumerable<IExpression> expressions)
    {
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _expressions = expressions.OrderBy(x => x.Order).ToArray();
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var validationResult = context.Validate<object?>();
        if (!validationResult.IsSuccessful())
        {
            return validationResult;
        }

        // First try simple expression
        var expressionResult = _expressions
            .Select(x => x.Evaluate(context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue);

        if (expressionResult is not null)
        {
            return expressionResult;
        }

        var tokenizer = new Tokenizer(context.Expression);
        var tokensResult = tokenizer.Tokenize();
        if (!tokensResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(tokensResult);
        }
        var parser = new Parser(tokensResult.Value!);
        var exprResult = parser.Parse();
        if (!exprResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(exprResult);
        }

        return exprResult.Value!.Evaluate(context, value => DoEvaluate(context.CreateChildContext(value)));
    }

    public Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return DoEvaluateTyped<T>(context);
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var validationResult = context.Validate<ExpressionParseResult>();
        if (!validationResult.IsSuccessful())
        {
            return new ExpressionParseResultBuilder()
                .WithStatus(validationResult.Status)
                .WithErrorMessage(validationResult.ErrorMessage);
        }

        return DoParse(context);
    }

    private Result<object?> DoEvaluate(ExpressionEvaluatorContext context)
        => _expressions
            .Select(x => x.Evaluate(context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<object?>($"Unknown expression type found in fragment: {context.Expression}");

    private Result<T> DoEvaluateTyped<T>(ExpressionEvaluatorContext context)
        => context.Validate<T>()
            .OnSuccess(() => _expressions
                .Select(x => x is IExpression<T> typedExpression
                    ? typedExpression.EvaluateTyped(context)
                    : x.Evaluate(context).TryCastAllowNull<T>())
                .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                    ?? Result.Invalid<T>($"Unknown expression type found in fragment: {context.Expression}"));

    private ExpressionParseResult DoParse(ExpressionEvaluatorContext context)
    {
        var expressionParseResult = _expressions
            .Select(x => x.Parse(context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue);

        return expressionParseResult is null
            ? new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Invalid)
                .WithErrorMessage($"Unknown expression type found in fragment: {context.Expression}")
            : expressionParseResult;
    }
}

internal enum TokenType
{
    Other,
    Plus,
    Minus,
    Multiply,
    Divide,
    LeftParen,
    RightParen,
    EqualEqual,
    NotEqual,
    Less,
    LessEqual,
    Greater,
    GreaterEqual,
    AndAnd,
    OrOr,
    EOF
}

internal class Tokenizer
{
    private readonly string _input;
    private int _position;

    public Tokenizer(string input)
    {
        _input = input;
    }

    public Result<List<Token>> Tokenize()
    {
        var tokens = new List<Token>();
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
                    tokens.Add(new Token(TokenType.Plus));
                    _position++;
                    break;
                case '-':
                    tokens.Add(new Token(TokenType.Minus));
                    _position++;
                    break;
                case '*':
                    tokens.Add(new Token(TokenType.Multiply));
                    _position++;
                    break;
                case '/':
                    tokens.Add(new Token(TokenType.Divide));
                    _position++;
                    break;
                case '(':
                    tokens.Add(new Token(TokenType.LeftParen));
                    _position++;
                    break;
                case ')':
                    tokens.Add(new Token(TokenType.RightParen));
                    _position++;
                    break;
                case '=':
                    if (Match('='))
                    {
                        tokens.Add(new Token(TokenType.EqualEqual));
                    }
                    else
                    {
                        return Result.Invalid<List<Token>>("Unexpected '='");
                    }

                    break;
                case '!':
                    if (Match('='))
                    {
                        tokens.Add(new Token(TokenType.NotEqual));
                    }
                    else
                    {
                        return Result.Invalid<List<Token>>("Unexpected '!'");
                    }

                    break;
                case '<':
                    if (Match('='))
                    {
                        tokens.Add(new Token(TokenType.LessEqual));
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.Less));
                        _position++;
                    }

                    break;
                case '>':
                    if (Match('='))
                    {
                        tokens.Add(new Token(TokenType.GreaterEqual));
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.Greater));
                        _position++;
                    }

                    break;
                case '&':
                    if (Match('&'))
                    {
                        tokens.Add(new Token(TokenType.AndAnd));
                    }
                    else
                    {
                        return Result.Invalid<List<Token>>("Single '&' is not supported.");
                    }

                    break;
                case '|':
                    if (Match('|'))
                    {
                        tokens.Add(new Token(TokenType.OrOr));
                    }
                    else
                    {
                        return Result.Invalid<List<Token>>("Single '|' is not supported.");
                    }

                    break;
                default:
                    tokens.Add(ReadOther(false));
                    break;
            }
        }

        tokens.Add(new Token(TokenType.EOF));
        return Result.Success(tokens);
    }

    private Token ReadOther(bool inQuotes)
    {
        var start = _position;
        while (_position < _input.Length && !IsTokenSign(_input[_position], inQuotes))
        {
            _position++;
        }

        var value = _input.Substring(start, _position - start);
        return new Token(TokenType.Other, value);
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

    private static readonly char[] TokenSigns = ['+', '-', '*', '/', '(', ')', '=', '!', '<', '>', '&', '|'];
}

internal interface IExpr
{
    Result<object?> Evaluate(ExpressionEvaluatorContext context, Func<string, Result<object?>> @delegate);
}

internal sealed class OtherExpr : IExpr
{
    private string Value { get; }

    public OtherExpr(string value)
    {
        Value = value;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context, Func<string, Result<object?>> @delegate) => @delegate(Value);
}

internal sealed class BinaryExpr : IExpr
{
    public Result<IExpr> Left { get; }
    public TokenType Operator { get; }
    public Result<IExpr> Right { get; }

    public BinaryExpr(Result<IExpr> left, TokenType op, Result<IExpr> right)
    {
        Left = left;
        Operator = op;
        Right = right;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context, Func<string, Result<object?>> @delegate)
    {
        var results = new ResultDictionaryBuilder<object?>()
            .Add(Constants.LeftExpression, () => Left.Value?.Evaluate(context, @delegate) ?? Result.FromExistingResult<object?>(Left))
            .Add(Constants.RightExpression, () => Right.Value?.Evaluate(context, @delegate) ?? Result.FromExistingResult<object?>(Right))
            .Build();

        var error = results.GetError();
        if (error is not null)
        {
            return error;
        }

        return Operator switch
        {
            TokenType.Plus => Add.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            TokenType.Minus => Subtract.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            TokenType.Multiply => Multiply.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            TokenType.Divide => Divide.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.FormatProvider),
            TokenType.EqualEqual => Equal.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.StringComparison).TryCastAllowNull<object?>(),
            TokenType.NotEqual => NotEqual.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression), context.Settings.StringComparison).TryCastAllowNull<object?>(),
            TokenType.Less => SmallerThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            TokenType.LessEqual => SmallerOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            TokenType.Greater => GreaterThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            TokenType.GreaterEqual => GreaterOrEqualThan.Evaluate(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)).TryCastAllowNull<object?>(),
            TokenType.AndAnd => EvaluateAnd(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            TokenType.OrOr => EvaluateOr(results.GetValue(Constants.LeftExpression), results.GetValue(Constants.RightExpression)),
            _ => Result.Invalid<object?>("Unsupported operator")
        };
    }

    private static Result<object?> EvaluateAnd(object? left, object? right)
        => Result.Success<object?>(GetBooleanValue(left) && GetBooleanValue(right));

    private static Result<object?> EvaluateOr(object? left, object? right)
        => Result.Success<object?>(GetBooleanValue(left) || GetBooleanValue(right));

    private static bool GetBooleanValue(object? value)
    {
        if (value is bool b)
        {
            return b;
        }
        else if (value is string s)
        {
            // design decision: if it's a string, then do a null or empty check
            return !string.IsNullOrEmpty(s);
        }
        else
        {
            // design decision: if it's not a boolean, then do a null check
            return value is not null;
        }
    }
}

internal sealed class Parser
{
    private readonly List<Token> _tokens;
    private int _position;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public Result<IExpr> Parse()
    {
        return ParseLogicalOr();
    }

    private Result<IExpr> ParseLogicalOr()
    {
        var expr = ParseLogicalAnd();

        while (Match(TokenType.OrOr))
        {
            var op = Previous();
            var right = ParseLogicalAnd();
            expr = Result.Success<IExpr>(new BinaryExpr(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IExpr> ParseLogicalAnd()
    {
        var expr = ParseEquality();

        while (Match(TokenType.AndAnd))
        {
            var op = Previous();
            var right = ParseEquality();
            expr = Result.Success<IExpr>(new BinaryExpr(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IExpr> ParseEquality()
    {
        var expr = ParseComparison();

        while (Match(TokenType.EqualEqual, TokenType.NotEqual))
        {
            var op = Previous();
            var right = ParseAdditive();
            expr = Result.Success<IExpr>(new BinaryExpr(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IExpr> ParseComparison()
    {
        var expr = ParseAdditive();

        while (Match(TokenType.Less, TokenType.LessEqual, TokenType.Greater, TokenType.GreaterEqual))
        {
            var op = Previous();
            var right = ParseAdditive();
            expr = Result.Success<IExpr>(new BinaryExpr(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IExpr> ParseAdditive()
    {
        var expr = ParseMultiplicative();

        while (Match(TokenType.Plus, TokenType.Minus))
        {
            var op = Previous();
            var right = ParseMultiplicative();
            expr = Result.Success<IExpr>(new BinaryExpr(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IExpr> ParseMultiplicative()
    {
        var expr = ParsePrimary();

        while (Match(TokenType.Multiply, TokenType.Divide))
        {
            var op = Previous();
            var right = ParsePrimary();
            expr = Result.Success<IExpr>(new BinaryExpr(expr, op.Type, right));
        }

        return expr;
    }

    private Result<IExpr> ParsePrimary()
    {
        if (Match(TokenType.Other))
        {
            return Result.Success<IExpr>(new OtherExpr(Previous().Value));
        }

        if (Match(TokenType.LeftParen))
        {
            var expr = Parse();
            _ = Consume(TokenType.RightParen, "Expect ')' after expression.");
            return expr;
        }

        return Result.Invalid<IExpr>("Unexpected token");
    }

    private bool Match(params TokenType[] types)
    {
        if (types.Any(Check))
        {
            Advance();
            return true;
        }

        return false;
    }


    private bool Check(TokenType type)
    {
        if (IsAtEnd())
        {
            return false;
        }

        return Peek().Type == type;
    }

    private Result<Token> Advance()
    {
        if (!IsAtEnd())
        {
            _position++;
        }

        return Result.Success(Previous());
    }

    private bool IsAtEnd() => Peek().Type == TokenType.EOF;
    private Token Peek() => _tokens[_position];
    private Token Previous() => _tokens[_position - 1];

    private Result<Token> Consume(TokenType type, string message)
    {
        if (!Check(type))
        {
            return Result.Invalid<Token>(message);
        }

        return Advance();
    }
}

internal sealed class Token
{
    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value = "")
    {
        Type = type;
        Value = value;
    }

    public override string ToString() => $"{Type} ({Value})";
}
