namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration;

public static class Constants
{
    public const string ProjectName = "CrossCutting.Utilities.ExpressionEvaluator";

    public static class Namespaces
    {
        public const string UtilitiesExpressionEvaluator = "CrossCutting.Utilities.ExpressionEvaluator";
        public const string UtilitiesExpressionEvaluatorFunctionCallArguments = "CrossCutting.Utilities.ExpressionEvaluator.FunctionCallArguments";
        public const string UtilitiesExpressionEvaluatorFunctionCallTypeArguments = "CrossCutting.Utilities.ExpressionEvaluator.FunctionCallTypeArguments";
        public const string UtilitiesExpressionEvaluatorFunctionDescriptorArguments = "CrossCutting.Utilities.ExpressionEvaluator.FunctionDescriptorArguments";
        public const string UtilitiesExpressionEvaluatorBuilders = "CrossCutting.Utilities.ExpressionEvaluator.Builders";
        public const string UtilitiesExpressionEvaluatorBuildersFunctionCallArguments = "CrossCutting.Utilities.ExpressionEvaluator.Builders.FunctionCallArguments";
        public const string UtilitiesExpressionEvaluatorBuildersFunctionCallTypeArguments = "CrossCutting.Utilities.ExpressionEvaluator.Builders.FunctionCallTypeArguments";
        public const string UtilitiesExpressionEvaluatorBuildersFunctionDescriptorArguments = "CrossCutting.Utilities.ExpressionEvaluator.Builders.FunctionDescriptorArguments";
    }

    [ExcludeFromCodeCoverage]
    public static class Paths
    {
        public const string FunctionCallArguments = $"{Namespaces.UtilitiesExpressionEvaluator}/{nameof(FunctionCallArguments)}";
        public const string FunctionCallArgumentBuilders = $"{Namespaces.UtilitiesExpressionEvaluator}/Builders/{nameof(FunctionCallArguments)}";

        public const string FunctionCallTypeArguments = $"{Namespaces.UtilitiesExpressionEvaluator}/{nameof(FunctionCallTypeArguments)}";
        public const string FunctionCallTypeArgumentBuilders = $"{Namespaces.UtilitiesExpressionEvaluator}/Builders/{nameof(FunctionCallTypeArguments)}";

        public const string FunctionDescriptorArguments = $"{Namespaces.UtilitiesExpressionEvaluator}/{nameof(FunctionDescriptorArguments)}";
        public const string FunctionDescriptorArgumentBuilders = $"{Namespaces.UtilitiesExpressionEvaluator}/Builders/{nameof(FunctionDescriptorArguments)}";
    }
}
