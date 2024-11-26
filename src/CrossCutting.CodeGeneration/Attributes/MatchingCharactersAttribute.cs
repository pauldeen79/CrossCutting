namespace CrossCutting.CodeGeneration.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MatchingCharactersAttribute : ValidationAttribute
{
}
