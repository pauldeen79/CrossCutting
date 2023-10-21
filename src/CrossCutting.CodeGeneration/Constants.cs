namespace CrossCutting.CodeGeneration;

public static class Constants
{
    public const string ProjectName = "CrossCutting";

    public static class Namespaces
    {
        public const string UtilitiesParsers = "CrossCutting.Utilities.Parsers";
        public const string UtilitiesParsersFunctionParseResultArguments = "CrossCutting.Utilities.Parsers.FunctionParseResultArguments";
        public const string UtilitiesParsersBuilders = "CrossCutting.Utilities.Parsers.Builders";
        public const string UtilitiesParsersBuildersFunctionParseResultArguments = "CrossCutting.Utilities.Parsers.Builders.FunctionParseResultArguments";
    }

    public static class Types
    {
        public const string FunctionParseResultArgument = "FunctionParseResultArgument";

        public const string FunctionParseResultArgumentBuilder = "FunctionParseResultArgumentBuilder";
    }

    [ExcludeFromCodeCoverage]
    public static class TypeNames
    {
        public const string FunctionParseResultArgument = $"{Namespaces.UtilitiesParsers}.FunctionParseResultArgument";
    }

    [ExcludeFromCodeCoverage]
    public static class Paths
    {
        public const string FunctionParseResultArguments = $"{Namespaces.UtilitiesParsers}/{nameof(FunctionParseResultArguments)}";

        public const string FunctionParseResultArgumentBuilders = $"{Namespaces.UtilitiesParsersBuilders}/{nameof(FunctionParseResultArguments)}";
    }
}
