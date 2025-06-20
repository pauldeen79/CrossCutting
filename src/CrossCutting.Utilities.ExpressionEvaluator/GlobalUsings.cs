﻿global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.Diagnostics.CodeAnalysis;
global using System.Globalization;
global using System.Linq;
global using System.Reflection;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Threading;
global using System.Threading.Tasks;
global using CrossCutting.Common;
global using CrossCutting.Common.Abstractions;
global using CrossCutting.Common.Default;
global using CrossCutting.Common.Extensions;
global using CrossCutting.Common.Results;
global using CrossCutting.Utilities.Aggregators;
global using CrossCutting.Utilities.ExpressionEvaluator.Abstractions;
global using CrossCutting.Utilities.ExpressionEvaluator.Builders;
global using CrossCutting.Utilities.ExpressionEvaluator.Builders.Extensions;
global using CrossCutting.Utilities.ExpressionEvaluator.Domains;
global using CrossCutting.Utilities.ExpressionEvaluator.DotExpressionComponents;
global using CrossCutting.Utilities.ExpressionEvaluator.ExpressionComponents;
global using CrossCutting.Utilities.ExpressionEvaluator.Expressions;
global using CrossCutting.Utilities.ExpressionEvaluator.Extensions;
global using CrossCutting.Utilities.ExpressionEvaluator.Functions;
global using CrossCutting.Utilities.ExpressionEvaluator.InstanceConstructors;
global using CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;
global using CrossCutting.Utilities.ExpressionEvaluator.InstanceProperties;
global using CrossCutting.Utilities.Operators;
global using Microsoft.Extensions.DependencyInjection;
