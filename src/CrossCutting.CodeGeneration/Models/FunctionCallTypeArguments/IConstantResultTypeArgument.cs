﻿namespace CrossCutting.CodeGeneration.Models.FunctionCallTypeArguments;

internal interface IConstantResultTypeArgument : IFunctionCallTypeArgumentBase
{
    [Required] Result<Type> Value { get; }
}
