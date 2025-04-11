namespace CrossCutting.Utilities.ExpressionEvaluator;

public enum OperatorExpressionTokenType
{
    Expression,
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
    EOF
}
