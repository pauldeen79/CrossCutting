namespace CrossCutting.CodeGeneration;

public static class Constants
{
    public const string ProjectName = "CrossCutting";

    public static class Namespaces
    {
        public const string UtilitiesParsers = "CrossCutting.Utilities.Parsers";
        public const string UtilitiesParsersFunctionCallArguments = "CrossCutting.Utilities.Parsers.FunctionCallArguments";
        public const string UtilitiesParsersFunctionCallTypeArguments = "CrossCutting.Utilities.Parsers.FunctionCallTypeArguments";
        public const string UtilitiesParsersFunctionDescriptorArguments = "CrossCutting.Utilities.Parsers.FunctionDescriptorArguments";
        public const string UtilitiesParsersBuilders = "CrossCutting.Utilities.Parsers.Builders";
        public const string UtilitiesParsersBuildersFunctionCallArguments = "CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments";
        public const string UtilitiesParsersBuildersFunctionCallTypeArguments = "CrossCutting.Utilities.Parsers.Builders.FunctionCallTypeArguments";
        public const string UtilitiesParsersBuildersFunctionDescriptorArguments = "CrossCutting.Utilities.Parsers.Builders.FunctionDescriptorArguments";
    }

    [ExcludeFromCodeCoverage]
    public static class Paths
    {
        public const string FunctionCallArguments = $"{Namespaces.UtilitiesParsers}/{nameof(FunctionCallArguments)}";
        public const string FunctionCallArgumentBuilders = $"{Namespaces.UtilitiesParsers}/Builders/{nameof(FunctionCallArguments)}";

        public const string FunctionCallTypeArguments = $"{Namespaces.UtilitiesParsers}/{nameof(FunctionCallTypeArguments)}";
        public const string FunctionCallTypeArgumentBuilders = $"{Namespaces.UtilitiesParsers}/Builders/{nameof(FunctionCallTypeArguments)}";

        public const string FunctionDescriptorArguments = $"{Namespaces.UtilitiesParsers}/{nameof(FunctionDescriptorArguments)}";
        public const string FunctionDescriptorArgumentBuilders = $"{Namespaces.UtilitiesParsers}/Builders/{nameof(FunctionDescriptorArguments)}";
    }
}
