namespace CrossCutting.DataTableDumper.TabDelimited;

public static class Parser
{
    public static ParseResult Parse(string input)
    {
        var split = input.GetLines().Select(x => x.Split('\t'));
        var list = new List<ExpandoObject>();
        foreach (var item in split.Skip(1))
        {
            var target = new ExpandoObject();
            var dict = target as IDictionary<string, object>;
            foreach (var column in item.Select((x, index) => new { Value = x, Name = split.First()[index] }))
            {
                dict[column.Name] = column.Value;
            }
            list.Add(target);
        }
        return new ParseResult(new DataTableDumper<ExpandoObject>(new MyColumnNameProvider(split), new MyColumnDataProvider()), list);
    }

    private sealed class MyColumnNameProvider(IEnumerable<string[]> data) : IColumnNameProvider
    {
        private readonly IEnumerable<string[]> _data = data;

        public IReadOnlyCollection<string> Get<T>() where T : class
            => new List<string>(_data.First()).AsReadOnly();
    }

    private sealed class MyColumnDataProvider : IColumnDataProvider<ExpandoObject>
    {
        public IReadOnlyCollection<string> Get(ExpandoObject item, string escapeValue)
            => new List<string>((item as IDictionary<string, object>).Values.Select(x => x.ToString().EscapePipes(escapeValue).WhenNullOrEmpty(string.Empty))).AsReadOnly();
    }
}

public class ParseResult(DataTableDumper<ExpandoObject> dataTableDumper, IReadOnlyCollection<ExpandoObject> list)
{
    public DataTableDumper<ExpandoObject> DataTableDumper { get; } = dataTableDumper;
    public IReadOnlyCollection<ExpandoObject> List { get; } = list;
}
