namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IChildEvaluatablesContainer
{
    IEnumerable<IEvaluatable> GetChildEvaluatables(); 
}