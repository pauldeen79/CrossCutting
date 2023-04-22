namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFormattableStringStateProcessor
{
    Result<string> Process(FormattableStringParserState state);
}
