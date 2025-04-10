namespace CrossCutting.Utilities.ExpressionEvaluator.ExpressionTokenizer;

internal enum ExpressionTokenType
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
    Bang,
    EOF
}
