namespace CrossCutting.Utilities.ObjectDumper;

public class DynamicPropertyDescriptor<TTarget, TProperty>(
   string propertyName,
   Func<TTarget, TProperty> getter,
   Action<TTarget, TProperty> setter,
   Attribute[]? attributes) : PropertyDescriptor(propertyName, attributes ?? [])
{
    private readonly Func<TTarget, TProperty> getter = getter;
    private readonly Action<TTarget, TProperty> setter = setter;
    private readonly string propertyName = propertyName;

    public override bool Equals(object obj)
        => (obj as DynamicPropertyDescriptor<TTarget, TProperty>)?.propertyName.Equals(propertyName, StringComparison.Ordinal) == true;

    public override int GetHashCode() => propertyName.GetHashCode();

    public override bool CanResetValue(object component) => true;

    public override Type ComponentType => typeof(TTarget);

    public override object GetValue(object component) => getter((TTarget)component)!;

    public override bool IsReadOnly => setter is null;

    public override Type PropertyType => typeof(TProperty);

    public override void ResetValue(object component)
    {
    }

    public override void SetValue(object component, object value) => setter((TTarget)component, (TProperty)value);

    public override bool ShouldSerializeValue(object component) => true;
}
