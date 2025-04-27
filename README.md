# CrossCutting
Generic utilities and other stuff, usable in any layer of any solution. We're targeting .NET Standard 2.0, so you can even use .NET Framework 4.x if you want to.

This repository consists of the following packages:

| Package name                               | Description                                                                                                                                                                              |
| :----------------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| CrossCutting.Common                        | Provider for system date and user name, some useful extension methods on System.Object and System.String, and the Result class                                                           |
| CrossCutting.Common.Testing                | Helps you test constructors on null checks                                                                                                                                               |
| CrossCutting.Data.Abstractions             | Abstraction for executing database commands using System.Data namespace (IDbConnection and IDbCommand)                                                                                   |
| CrossCutting.Data.Core                     | Default implementation of database commands                                                                                                                                              |
| CrossCutting.Data.Sql                      | Extension methods for working with database commands in System.Data namespace (IDbConnection)                                                                                            |
| CrossCutting.DataTableDumper               | Produces flat-text data tables from objects                                                                                                                                              |
| CrossCutting.Utilities.ObjectDumper        | Produces readable flat-text representation from objects, for use in logging                                                                                                              |
| CrossCutting.Utilities.Parsers             | Parsers for pipe-delmited data table strings, TSQL INSERT INTO statements, expression strings, function strings, math expressions and formattable strings (dynamic interpolated strings) |
| CrossCutting.Utilities.ExpressionEvaluator | Expression evaluator to dynamically evaluate strings, with support for all kinds of stuff, like: operators, functions, mathematic expressions, and so on                                 |
| System.Data.Stub                           | Stubs for System.Data interfaces like IDbConnection, IDbCommand and IDataReader                                                                                                          |

# Using NSubstitute or Moq as mock factory for CrossCutting.Common.Testing

To use NSubstitute as a mock factory for the code in CrossCutting.Common.Testing, create a test helper project within your solution, and add the following code:

```C#
    public static void ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
        this Type type,
        Func<ParameterInfo, bool>? parameterPredicate = null,
        Func<ParameterInfo, object?>? parameterReplaceDelegate = null,
        Func<ConstructorInfo, bool>? constructorPredicate = null)
        => ShouldThrowArgumentNullExceptionsInConstructorsOnNullArguments(
            type,
            t => t.CreateInstance(parameterType => Substitute.For(new[] { parameterType }, Array.Empty<object>()), parameterReplaceDelegate, constructorPredicate),
            parameterPredicate,
            parameterReplaceDelegate,
            constructorPredicate);
```

You can easily replace NSubstitute with Moq, if you want, by changing the factory delegate argument to this:

```C#
t => t.CreateInstance(parameterType => ((Mock)Activator.CreateInstance(typeof(Mock<>).MakeGenericType(parameterType))).Object, parameterReplaceDelegate, constructorPredicate),
```

# Upgrade from 2.x to 3.0
There has been a breaking change in the Result class, which lead to a new major version.
To port the old code, you can fix most errors by replace this:
```regex
Result<([^>]+)>\.([^(]+)\(
```

With this:
```regex
Result.$2<$1>(
```

# Upgrade Parsers from 6.x to 7.0
There have been some breaking changes.

* FunctionParseResult has been renamed to FunctionCall, because that's what it is.
* IFunctionResultParser has been renamed to IFunction, and Parse has been renamed to Evaluate, because that's what it is.
* IPlaceholderProcessor has been renamed to IPlaceholder, and Process has been renamed to Evaluate, because that's what it is.
* Some parsers and evaluators now have a Validate method, to check whether the input is valid. This way, you can check validity without actually performing the action.
* IVariable and IVariableProcessor now have a method called Evaluate instead of Process, which is more descriptive.
* IExpressionParser has been renamed to IExpressionEvaluator and IExpressionParserProcessor to IExpression.
* IExpressionStringParser has been renamed to IExpressionStringEvaluator and IExpresionStringParserProcessor to IExpressionString.
* IMathematicExpressionParser has been renamed to IMathematicExpressionEvaluator and IMathematicExpressionProcessor to IMathematicExpression.
* FormattableStringParserResult has been renamed to GenericFormattableString.

# ExpressionEvaluator

The ExpressionEvaluator is a full rewrite of the Parsers project, where and Expression is the entry type for everything. An expression can contain the following things:
* Strings, like "hello world"
* Booleans "true" and "false"
* DateTime.Now and DateTime.Today, as well as the Date and DateTime functions to create a custom DateTime value
* Numeric values, both integer and floating point types like "1.3" and "12"
* Interpolated strings, like "hello {name}"
* Mathematic operators like +, -, * and / for example "(1 + 1) * 13"
* Binary operators && and || for example "true && true"
* Unary operator ! to inverse a boolean value
* Comparison operators <, <=, >, >=, == and != for example "true != false"
* Built-in functions for DateTime values: AddDays, AddHours, AddMinuts, AddMonths, AddSeconds, AddYears and DatePart (which removes the time)
* Built-in functions for String values: ToCamelCase, ToLowerCase, ToPascalCase and ToUpperCase, Left, Right, Substring
* Cast and Convert functions to cast and convert values to other types
* ToString function to convert objects to string
* IsNull function to check for null values
* Optional support for reflection to get property values, or invoke methods
* Support for context using delegates, in order to allow lazy evaluation
* Support for custom functions, using dependency injection
* Support for adding custom expression components, using dependency injection
