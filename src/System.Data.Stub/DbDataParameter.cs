namespace System.Data.Stub;

public sealed class DbDataParameter : IDbDataParameter
{
    public byte Precision { get; set; }
    public byte Scale { get; set; }
    public int Size { get; set; }
    public DbType DbType { get; set; }
    public ParameterDirection Direction { get; set; }

    public bool IsNullable { get; set; }

    public string ParameterName { get; set; } = string.Empty;
    public string SourceColumn { get; set; } = string.Empty;
    public DataRowVersion SourceVersion { get; set; }
    public object? Value { get; set; }
}
