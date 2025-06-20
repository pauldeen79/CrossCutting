﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 9.0.6
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
#nullable enable
namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallTypeArguments
{
    public partial class ConstantResultTypeArgumentBuilder : FunctionCallTypeArgumentBaseBuilder<ConstantResultTypeArgumentBuilder, CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ConstantResultTypeArgument>, CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder
    {
        private CrossCutting.Common.Results.Result<System.Type> _value;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public CrossCutting.Common.Results.Result<System.Type> Value
        {
            get
            {
                return _value;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<CrossCutting.Common.Results.Result<System.Type>>.Default.Equals(_value!, value!);
                _value = value ?? throw new System.ArgumentNullException(nameof(value));
                if (hasChanged) HandlePropertyChanged(nameof(Value));
            }
        }

        public ConstantResultTypeArgumentBuilder(CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ConstantResultTypeArgument source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _value = source.Value;
        }

        public ConstantResultTypeArgumentBuilder() : base()
        {
            _value = default(CrossCutting.Common.Results.Result<System.Type>)!;
            SetDefaultValues();
        }

        public override CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ConstantResultTypeArgument BuildTyped()
        {
            return new CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ConstantResultTypeArgument(Value);
        }

        CrossCutting.Utilities.Parsers.Abstractions.IFunctionCallTypeArgument CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder.Build()
        {
            return BuildTyped();
        }

        partial void SetDefaultValues();

        public CrossCutting.Utilities.Parsers.Builders.FunctionCallTypeArguments.ConstantResultTypeArgumentBuilder WithValue(CrossCutting.Common.Results.Result<System.Type> value)
        {
            if (value is null) throw new System.ArgumentNullException(nameof(value));
            Value = value;
            return this;
        }

        public static implicit operator CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ConstantResultTypeArgument(ConstantResultTypeArgumentBuilder entity)
        {
            return entity.BuildTyped();
        }
    }
    public partial class ConstantTypeArgumentBuilder : FunctionCallTypeArgumentBaseBuilder<ConstantTypeArgumentBuilder, CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ConstantTypeArgument>, CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder
    {
        private System.Type _value;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public System.Type Value
        {
            get
            {
                return _value;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.Type>.Default.Equals(_value!, value!);
                _value = value ?? throw new System.ArgumentNullException(nameof(value));
                if (hasChanged) HandlePropertyChanged(nameof(Value));
            }
        }

        public ConstantTypeArgumentBuilder(CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ConstantTypeArgument source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _value = source.Value;
        }

        public ConstantTypeArgumentBuilder() : base()
        {
            _value = default(System.Type)!;
            SetDefaultValues();
        }

        public override CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ConstantTypeArgument BuildTyped()
        {
            return new CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ConstantTypeArgument(Value);
        }

        CrossCutting.Utilities.Parsers.Abstractions.IFunctionCallTypeArgument CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder.Build()
        {
            return BuildTyped();
        }

        partial void SetDefaultValues();

        public CrossCutting.Utilities.Parsers.Builders.FunctionCallTypeArguments.ConstantTypeArgumentBuilder WithValue(System.Type value)
        {
            if (value is null) throw new System.ArgumentNullException(nameof(value));
            Value = value;
            return this;
        }

        public static implicit operator CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ConstantTypeArgument(ConstantTypeArgumentBuilder entity)
        {
            return entity.BuildTyped();
        }
    }
    public partial class DelegateResultTypeArgumentBuilder : FunctionCallTypeArgumentBaseBuilder<DelegateResultTypeArgumentBuilder, CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.DelegateResultTypeArgument>, CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder
    {
        private System.Func<CrossCutting.Common.Results.Result<System.Type>> _delegate;

        private System.Func<CrossCutting.Common.Results.Result<System.Type>>? _validationDelegate;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public System.Func<CrossCutting.Common.Results.Result<System.Type>> Delegate
        {
            get
            {
                return _delegate;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.Func<CrossCutting.Common.Results.Result<System.Type>>>.Default.Equals(_delegate!, value!);
                _delegate = value ?? throw new System.ArgumentNullException(nameof(value));
                if (hasChanged) HandlePropertyChanged(nameof(Delegate));
            }
        }

        public System.Func<CrossCutting.Common.Results.Result<System.Type>>? ValidationDelegate
        {
            get
            {
                return _validationDelegate;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.Func<CrossCutting.Common.Results.Result<System.Type>>?>.Default.Equals(_validationDelegate!, value!);
                _validationDelegate = value;
                if (hasChanged) HandlePropertyChanged(nameof(ValidationDelegate));
            }
        }

        public DelegateResultTypeArgumentBuilder(CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.DelegateResultTypeArgument source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _delegate = source.Delegate;
            _validationDelegate = source.ValidationDelegate;
        }

        public DelegateResultTypeArgumentBuilder() : base()
        {
            _delegate = default(System.Func<CrossCutting.Common.Results.Result<System.Type>>)!;
            SetDefaultValues();
        }

        public override CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.DelegateResultTypeArgument BuildTyped()
        {
            return new CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.DelegateResultTypeArgument(Delegate, ValidationDelegate);
        }

        CrossCutting.Utilities.Parsers.Abstractions.IFunctionCallTypeArgument CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder.Build()
        {
            return BuildTyped();
        }

        partial void SetDefaultValues();

        public CrossCutting.Utilities.Parsers.Builders.FunctionCallTypeArguments.DelegateResultTypeArgumentBuilder WithDelegate(System.Func<CrossCutting.Common.Results.Result<System.Type>> @delegate)
        {
            if (@delegate is null) throw new System.ArgumentNullException(nameof(@delegate));
            Delegate = @delegate;
            return this;
        }

        public CrossCutting.Utilities.Parsers.Builders.FunctionCallTypeArguments.DelegateResultTypeArgumentBuilder WithValidationDelegate(System.Func<CrossCutting.Common.Results.Result<System.Type>>? validationDelegate)
        {
            ValidationDelegate = validationDelegate;
            return this;
        }

        public static implicit operator CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.DelegateResultTypeArgument(DelegateResultTypeArgumentBuilder entity)
        {
            return entity.BuildTyped();
        }
    }
    public partial class DelegateTypeArgumentBuilder : FunctionCallTypeArgumentBaseBuilder<DelegateTypeArgumentBuilder, CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.DelegateTypeArgument>, CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder
    {
        private System.Func<System.Type> _delegate;

        private System.Func<System.Type>? _validationDelegate;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public System.Func<System.Type> Delegate
        {
            get
            {
                return _delegate;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.Func<System.Type>>.Default.Equals(_delegate!, value!);
                _delegate = value ?? throw new System.ArgumentNullException(nameof(value));
                if (hasChanged) HandlePropertyChanged(nameof(Delegate));
            }
        }

        public System.Func<System.Type>? ValidationDelegate
        {
            get
            {
                return _validationDelegate;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.Func<System.Type>?>.Default.Equals(_validationDelegate!, value!);
                _validationDelegate = value;
                if (hasChanged) HandlePropertyChanged(nameof(ValidationDelegate));
            }
        }

        public DelegateTypeArgumentBuilder(CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.DelegateTypeArgument source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _delegate = source.Delegate;
            _validationDelegate = source.ValidationDelegate;
        }

        public DelegateTypeArgumentBuilder() : base()
        {
            _delegate = default(System.Func<System.Type>)!;
            SetDefaultValues();
        }

        public override CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.DelegateTypeArgument BuildTyped()
        {
            return new CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.DelegateTypeArgument(Delegate, ValidationDelegate);
        }

        CrossCutting.Utilities.Parsers.Abstractions.IFunctionCallTypeArgument CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder.Build()
        {
            return BuildTyped();
        }

        partial void SetDefaultValues();

        public CrossCutting.Utilities.Parsers.Builders.FunctionCallTypeArguments.DelegateTypeArgumentBuilder WithDelegate(System.Func<System.Type> @delegate)
        {
            if (@delegate is null) throw new System.ArgumentNullException(nameof(@delegate));
            Delegate = @delegate;
            return this;
        }

        public CrossCutting.Utilities.Parsers.Builders.FunctionCallTypeArguments.DelegateTypeArgumentBuilder WithValidationDelegate(System.Func<System.Type>? validationDelegate)
        {
            ValidationDelegate = validationDelegate;
            return this;
        }

        public static implicit operator CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.DelegateTypeArgument(DelegateTypeArgumentBuilder entity)
        {
            return entity.BuildTyped();
        }
    }
    public partial class ExpressionTypeArgumentBuilder : FunctionCallTypeArgumentBaseBuilder<ExpressionTypeArgumentBuilder, CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ExpressionTypeArgument>, CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder
    {
        private string _expression;

        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        public string Expression
        {
            get
            {
                return _expression;
            }
            set
            {
                bool hasChanged = !System.Collections.Generic.EqualityComparer<System.String>.Default.Equals(_expression!, value!);
                _expression = value ?? throw new System.ArgumentNullException(nameof(value));
                if (hasChanged) HandlePropertyChanged(nameof(Expression));
            }
        }

        public ExpressionTypeArgumentBuilder(CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ExpressionTypeArgument source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _expression = source.Expression;
        }

        public ExpressionTypeArgumentBuilder() : base()
        {
            _expression = string.Empty;
            SetDefaultValues();
        }

        public override CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ExpressionTypeArgument BuildTyped()
        {
            return new CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ExpressionTypeArgument(Expression);
        }

        CrossCutting.Utilities.Parsers.Abstractions.IFunctionCallTypeArgument CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder.Build()
        {
            return BuildTyped();
        }

        partial void SetDefaultValues();

        public CrossCutting.Utilities.Parsers.Builders.FunctionCallTypeArguments.ExpressionTypeArgumentBuilder WithExpression(string expression)
        {
            if (expression is null) throw new System.ArgumentNullException(nameof(expression));
            Expression = expression;
            return this;
        }

        public static implicit operator CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.ExpressionTypeArgument(ExpressionTypeArgumentBuilder entity)
        {
            return entity.BuildTyped();
        }
    }
    public partial class FunctionTypeArgumentBuilder : FunctionCallTypeArgumentBaseBuilder<FunctionTypeArgumentBuilder, CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.FunctionTypeArgument>, CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder
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

        public FunctionTypeArgumentBuilder(CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.FunctionTypeArgument source) : base(source)
        {
            if (source is null) throw new System.ArgumentNullException(nameof(source));
            _function = source.Function.ToBuilder();
        }

        public FunctionTypeArgumentBuilder() : base()
        {
            _function = new CrossCutting.Utilities.Parsers.Builders.FunctionCallBuilder()!;
            SetDefaultValues();
        }

        public override CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.FunctionTypeArgument BuildTyped()
        {
            return new CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.FunctionTypeArgument(Function.Build());
        }

        CrossCutting.Utilities.Parsers.Abstractions.IFunctionCallTypeArgument CrossCutting.Utilities.Parsers.Builders.Abstractions.IFunctionCallTypeArgumentBuilder.Build()
        {
            return BuildTyped();
        }

        partial void SetDefaultValues();

        public CrossCutting.Utilities.Parsers.Builders.FunctionCallTypeArguments.FunctionTypeArgumentBuilder WithFunction(CrossCutting.Utilities.Parsers.Builders.FunctionCallBuilder function)
        {
            if (function is null) throw new System.ArgumentNullException(nameof(function));
            Function = function;
            return this;
        }

        public static implicit operator CrossCutting.Utilities.Parsers.FunctionCallTypeArguments.FunctionTypeArgument(FunctionTypeArgumentBuilder entity)
        {
            return entity.BuildTyped();
        }
    }
}
#nullable disable
