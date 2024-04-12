namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFormattableStringStateProcessor
{
    Result<FormattableStringParserResult> Process(FormattableStringParserState state);
}
