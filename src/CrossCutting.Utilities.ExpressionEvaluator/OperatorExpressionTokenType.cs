namespace CrossCutting.Utilities.ExpressionEvaluator;

public enum OperatorExpressionTokenType
{
    Other,
    Plus,
    Minus,
    Multiply,
    Divide,
    LeftParenthesis,
    RightParenthesis,
    Equal,
    NotEqual,
    Less,
    LessOrEqual,
    Greater,
    GreaterOrEqual,
    And,
    Or,
    Bang,
    Exponentiation,
    Modulo,
    Literal,
    InterpolatedString,
    Dollar,
    EOF
}
