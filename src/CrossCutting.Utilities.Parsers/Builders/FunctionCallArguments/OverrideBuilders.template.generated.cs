﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 9.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments
{
    public partial class EmptyArgumentBuilder : CrossCutting.Utilities.Parsers.Builders.FunctionCallArgumentBuilder<EmptyArgumentBuilder, CrossCutting.Utilities.Parsers.FunctionCallArguments.EmptyArgument>
    {
        public EmptyArgumentBuilder(CrossCutting.Utilities.Parsers.FunctionCallArguments.EmptyArgument source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
        }

        public EmptyArgumentBuilder() : base()
        {
            SetDefaultValues();
        }

        public override CrossCutting.Utilities.Parsers.FunctionCallArguments.EmptyArgument BuildTyped()
        {
            return new CrossCutting.Utilities.Parsers.FunctionCallArguments.EmptyArgument();
        }

        partial void SetDefaultValues();
    }
    public partial class LiteralArgumentBuilder : CrossCutting.Utilities.Parsers.Builders.FunctionCallArgumentBuilder<LiteralArgumentBuilder, CrossCutting.Utilities.Parsers.FunctionCallArguments.LiteralArgument>
    {
        private string _value;

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.String>.Default.Equals(_value!, value!);
                _value = value ?? throw new System.ArgumentNullException(nameof(value));
                if (hasChanged) HandlePropertyChanged(nameof(Value));
            }
        }

        public LiteralArgumentBuilder(CrossCutting.Utilities.Parsers.FunctionCallArguments.LiteralArgument source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _value = source.Value;
        }

        public LiteralArgumentBuilder() : base()
        {
            _value = string.Empty;
            SetDefaultValues();
        }

        public override CrossCutting.Utilities.Parsers.FunctionCallArguments.LiteralArgument BuildTyped()
        {
            return new CrossCutting.Utilities.Parsers.FunctionCallArguments.LiteralArgument(Value);
        }

        partial void SetDefaultValues();

        public CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments.LiteralArgumentBuilder WithValue(string value)
        {
            if (value is null) throw new System.ArgumentNullException(nameof(value));
            Value = value;
            return this;
        }
    }
    public partial class RecursiveArgumentBuilder : CrossCutting.Utilities.Parsers.Builders.FunctionCallArgumentBuilder<RecursiveArgumentBuilder, CrossCutting.Utilities.Parsers.FunctionCallArguments.RecursiveArgument>
    {
        private CrossCutting.Utilities.Parsers.Builders.FunctionCallBuilder _function;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public CrossCutting.Utilities.Parsers.Builders.FunctionCallBuilder Function
        {
            get
            {
                return _function;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<CrossCutting.Utilities.Parsers.Builders.FunctionCallBuilder>.Default.Equals(_function!, value!);
                _function = value ?? throw new System.ArgumentNullException(nameof(value));
                if (hasChanged) HandlePropertyChanged(nameof(Function));
            }
        }

        public RecursiveArgumentBuilder(CrossCutting.Utilities.Parsers.FunctionCallArguments.RecursiveArgument source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _function = source.Function.ToBuilder();
        }

        public RecursiveArgumentBuilder() : base()
        {
            _function = new CrossCutting.Utilities.Parsers.Builders.FunctionCallBuilder()!;
            SetDefaultValues();
        }

        public override CrossCutting.Utilities.Parsers.FunctionCallArguments.RecursiveArgument BuildTyped()
        {
            return new CrossCutting.Utilities.Parsers.FunctionCallArguments.RecursiveArgument(Function.Build());
        }

        partial void SetDefaultValues();

        public CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments.RecursiveArgumentBuilder WithFunction(CrossCutting.Utilities.Parsers.Builders.FunctionCallBuilder function)
        {
            if (function is null) throw new System.ArgumentNullException(nameof(function));
            Function = function;
            return this;
        }
    }
}
#nullable disable