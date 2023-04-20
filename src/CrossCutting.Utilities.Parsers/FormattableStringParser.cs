namespace CrossCutting.Utilities.Parsers;

public static class FormattableStringParser
{
    private const char OpenSign = '{';
    private const char CloseSign = '}';

    private static readonly FormattableStringStateProcessor[] _processors = new FormattableStringStateProcessor[]
    {
        new OpenSignProcessor(),
        new CloseSignProcessor(),
        new PlaceholderProcessor(),
        new ResultProcessor(),
    };

    public static Result<string> Parse(string input, Func<string, Result<string>> placeholderDelegate)
    {
        var state = new FormattableStringState(input, placeholderDelegate);

        for (var index = 0; index < input.Length; index++)
        {
            state.Current = input[index];
            state.Index = index;

            foreach (var processor in _processors)
            {
                var processorResult = processor.Process(state);
                if (processorResult.Status != ResultStatus.NotSupported && !processorResult.IsSuccessful())
                {
                    return processorResult;
                }
                else if (processorResult.Status == ResultStatus.NoContent)
                {
                    break;
                }
            }
        }

        if (state.InPlaceholder)
        {
            return Result<string>.Invalid("Missing close sign '}'. To use the '{' character, you have to escape it with an additional '{' character");
        }

        return Result<string>.Success(state.ResultBuilder.ToString());
    }

    private static bool NextPositionIsSign(string input, int index, char sign)
    {
        if (index + 1 == input.Length)
        {
            // We're at the end of the string, so this is not possible
            return false;
        }

        return input[index + 1] == sign;
    }

    private static bool PreviousPositionIsSign(string input, int index, char sign)
    {
        if (index == 0)
        {
            // We're at the end of the string, so this is not possible
            return false;
        }

        return input[index - 1] == sign;
    }

    private sealed class FormattableStringState
    {
        public string Input { get; }
        public Func<string, Result<string>> PlaceholderDelegate { get; }

        public StringBuilder ResultBuilder { get; } = new();
        public StringBuilder PlaceholderBuilder { get; } = new();
        public bool InPlaceholder { get; set;  }
        public char Current { get; set; }
        public int Index { get; set; }

        public FormattableStringState(string input, Func<string, Result<string>> placeholderDelegate)
        {
            Input = input;
            PlaceholderDelegate = placeholderDelegate;
        }
    }

    private abstract class FormattableStringStateProcessor
    {
        public abstract Result<string> Process(FormattableStringState state);
    }

    private sealed class OpenSignProcessor : FormattableStringStateProcessor
    {
        public override Result<string> Process(FormattableStringState state)
        {
            if (state.Current != OpenSign)
            {
                return Result<string>.NotSupported();
            }

            if (NextPositionIsSign(state.Input, state.Index, OpenSign) || PreviousPositionIsSign(state.Input, state.Index, OpenSign))
            {
                return Result<string>.Continue();
            }

            if (state.InPlaceholder)
            {
                return Result<string>.Invalid("Recursive placeholder detected, this is not supported");
            }

            state.InPlaceholder = true;

            return Result<string>.NoContent();
        }
    }

    private sealed class CloseSignProcessor : FormattableStringStateProcessor
    {
        public override Result<string> Process(FormattableStringState state)
        {
            if (state.Current != CloseSign)
            {
                return Result<string>.NotSupported();
            }

            if (NextPositionIsSign(state.Input, state.Index, CloseSign) || PreviousPositionIsSign(state.Input, state.Index, CloseSign))
            {
                return Result<string>.Continue();
            }

            if (!state.InPlaceholder)
            {
                return Result<string>.Invalid("Missing open sign '{'. To use the '}' character, you have to escape it with an additional '}' character");
            }

            var placeholderResult = state.PlaceholderDelegate.Invoke(state.PlaceholderBuilder.ToString());
            if (!placeholderResult.IsSuccessful())
            {
                return placeholderResult;
            }

            state.InPlaceholder = false;
            state.ResultBuilder.Append(placeholderResult.Value!);
            state.PlaceholderBuilder.Clear();

            return Result<string>.NoContent();
        }
    }

    private sealed class PlaceholderProcessor : FormattableStringStateProcessor
    {
        public override Result<string> Process(FormattableStringState state)
        {
            if (!state.InPlaceholder)
            {
                return Result<string>.NotSupported();
            }

            state.PlaceholderBuilder.Append(state.Current);

            return Result<string>.NoContent();
        }
    }

    private sealed class ResultProcessor : FormattableStringStateProcessor
    {
        public override Result<string> Process(FormattableStringState state)
        {
            state.ResultBuilder.Append(state.Current);

            return Result<string>.NoContent();
        }
    }
}
