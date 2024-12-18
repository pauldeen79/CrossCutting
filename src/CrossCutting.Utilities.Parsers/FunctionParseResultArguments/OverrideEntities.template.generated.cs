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
namespace CrossCutting.Utilities.Parsers.FunctionParseResultArguments
{
    public partial record FunctionArgument : CrossCutting.Utilities.Parsers.FunctionParseResultArgument
    {
        [System.ComponentModel.DataAnnotations.RequiredAttribute]
        [CrossCutting.Common.DataAnnotations.ValidateObjectAttribute]
        public CrossCutting.Utilities.Parsers.FunctionParseResult Function
        {
            get;
        }

        public FunctionArgument(CrossCutting.Utilities.Parsers.FunctionParseResult function) : base()
        {
            this.Function = function;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        public override CrossCutting.Utilities.Parsers.Builders.FunctionParseResultArgumentBuilder ToBuilder()
        {
            return ToTypedBuilder();
        }

        public CrossCutting.Utilities.Parsers.Builders.FunctionParseResultArguments.FunctionArgumentBuilder ToTypedBuilder()
        {
            return new CrossCutting.Utilities.Parsers.Builders.FunctionParseResultArguments.FunctionArgumentBuilder(this);
        }
    }
    public partial record LiteralArgument : CrossCutting.Utilities.Parsers.FunctionParseResultArgument
    {
        public string Value
        {
            get;
        }

        public LiteralArgument(string value) : base()
        {
            this.Value = value;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }

        public override CrossCutting.Utilities.Parsers.Builders.FunctionParseResultArgumentBuilder ToBuilder()
        {
            return ToTypedBuilder();
        }

        public CrossCutting.Utilities.Parsers.Builders.FunctionParseResultArguments.LiteralArgumentBuilder ToTypedBuilder()
        {
            return new CrossCutting.Utilities.Parsers.Builders.FunctionParseResultArguments.LiteralArgumentBuilder(this);
        }
    }
}
#nullable disable
