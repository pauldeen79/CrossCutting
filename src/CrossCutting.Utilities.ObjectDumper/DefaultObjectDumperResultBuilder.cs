using CrossCutting.Utilities.ObjectDumper.Contracts;
using CrossCutting.Utilities.ObjectDumper.Extensions;
using System;
using System.Text;

namespace CrossCutting.Utilities.ObjectDumper
{
    public class DefaultObjectDumperResultBuilder : IObjectDumperResultBuilder
    {
        private readonly StringBuilder _builder = new StringBuilder();

        public void AddEnumerableItem(bool first, int indent, bool isComplexType)
        {
            if (first)
            {
                return;
            }

            if (isComplexType)
            {
                // complex type:
                _builder.AppendLine(",");
            }
            else
            {
                // enumerable:
                _builder
                    .AppendLine(",")
                    .Append(new string(' ', indent));
            }
        }

        public void AddException(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            _builder
                .Append(exception.ToString().JsonQuote())
                .Append(exception.GetType().FullName.DumpTypeName());
        }

        public void AddName(int indent, string name)
        {
            _builder
                .Append(new string(' ', indent))
                .Append("\"")
                .Append(name)
                .Append("\"")
                .Append(": ");
        }

        public void AddSingleValue(object value, Type instanceType)
        {
            if (value == null)
            {
                _builder.Append("NULL");
            }
            else if (value is string)
            {
                _builder.Append(value.ToString().JsonQuote());
            }
            else
            {
                _builder.Append(value);
            }

            _builder.Append(instanceType?.FullName.DumpTypeName());
        }

        public void BeginNesting(int indent, Type instanceType)
        {
            if (indent > 4 && !_builder.ToString().TrimEnd().EndsWithAny(Environment.NewLine, "],", "["))
            {
                _builder
                    .AppendLine()
                    .Append(new string(' ', indent - 4));
            }
        }

        public void BeginComplexType(int indent, Type instanceType)
        {
            _builder.AppendLine("{");
        }

        public void BeginEnumerable(int indent, Type instanceType)
        {
            _builder
                .AppendLine("[")
                .Append(new string(' ', indent));
        }

        public void EndComplexType(bool first, int indent, Type instanceType)
        {
            if (instanceType == null)
            {
                throw new ArgumentNullException(nameof(instanceType));
            }

            if (!first)
            {
                _builder.AppendLine();
            }

            if (indent > 4)
            {
                _builder.Append(new string(' ', indent - 4));
            }

            _builder
                .Append("}")
                .Append(instanceType.FullName.DumpTypeName());
        }

        public void EndEnumerable(int indent, Type instanceType)
        {
            if (instanceType == null)
            {
                throw new ArgumentNullException(nameof(instanceType));
            }

            _builder.AppendLine();

            if (indent > 4)
            {
                _builder.Append(new string(' ', indent - 4));
            }

            _builder
                .Append("]")
                .Append(instanceType.FullName.DumpTypeName());
        }

        public override string ToString()
            => _builder.ToString();
    }
}
