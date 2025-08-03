namespace CrossCutting.Utilities.ExpressionEvaluator.Domains;

public enum ExpressionTokenType
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
    Modulus,
    In,
    NotIn,
    EOF
}
