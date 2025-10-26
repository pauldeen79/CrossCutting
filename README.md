# CrossCutting
Generic utilities and other stuff, usable in any layer of any solution. We're targeting .NET Standard 2.0, so you can even use .NET Framework 4.x if you want to.

This repository consists of the following packages:

| Package name                               | Description                                                                                                                                                                              |
| :----------------------------------------- | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| CroddCutting.Commands                      | Generic command handler                                                                                                                                                                  |
| CrossCutting.Common                        | Provider for system date and user name, some useful extension methods on System.Object and System.String, and the Result class                                                           |
| CrossCutting.Common.Testing                | Helps you test constructors on null checks, and construct instances with mocks                                                                                                           |
| CrossCutting.Data.Abstractions             | Abstraction for executing database commands using System.Data namespace (IDbConnection and IDbCommand)                                                                                   |
| CrossCutting.Data.Core                     | Default implementation of database commands                                                                                                                                              |
| CrossCutting.Data.Sql                      | Extension methods for working with database commands in System.Data namespace (IDbConnection)                                                                                            |
| CrossCutting.DataTableDumper               | Produces flat-text data tables from objects                                                                                                                                              |
| CrossCutting.ProcessingPipeline            | Generic pipeline with dependency injection-feeded multiple components                                                                                                                    |
| CrossCutting.Utilities.ObjectDumper        | Produces readable flat-text representation from objects, for use in logging                                                                                                              |
| CrossCutting.Utilities.Parsers             | Parsers for pipe-delmited data table strings, TSQL INSERT INTO statements, expression strings, function strings, math expressions and formattable strings (dynamic interpolated strings) |
| CrossCutting.Utilities.ExpressionEvaluator | Expression evaluator to dynamically evaluate strings, with support for all kinds of stuff, like: operators, functions, mathematic expressions, and so on                                 |
| CrossCutting.Utilities.QueryEvaluator      | Query evaluator to dynamically evaluate queries, similar to dynamic LINQ                                                                                                                 |
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

# Using actual ServiceCollection with CrossCutting.Common.Testing

You can use your real ServiceCollection extension method to fill the class factories, and then replace some types for testability.

Example:
```
_dataProviderMock = new DataProviderMock();
var excludedTypes = new Type[] { typeof(IDateTimeProvider) };
var classFactories = new ServiceCollection()
    .AddExpressionEvaluator()                       // Dependency
    .AddQueryEvaluatorInMemory()                    // System Under Test
    .AddSingleton<IDataProvider>(_dataProviderMock) // Test stub
    .Where(sd => !excludedTypes.Contains(sd.ServiceType))
    .GroupBy(sd => sd.ServiceType)
    .ToDictionary(
        g => g.Key,
        g => g.Count() == 1
            ? g.First().ImplementationInstance ?? g.First().ImplementationType
            : g.Select(t => t.ImplementationInstance ?? t.ImplementationType).ToArray());
```

If you want to, you can also remove items you want to mock afterwards:
```
classFactories
    .Where(x => excludedTypes.Contains(x.Key))
    .ToList()
    .ForEach(x => classFactories.Remove(x.Key));
```

Or, you can replace a specific type with a custom mock instance:
```
classFactories[typeof(IExpressionEvaluator)] = new ExpressionEvaluatorMock(Expression);

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

The ExpressionEvaluator is a complete rewrite of the Parsers project, where and Expression is the entry type for everything. An expression can contain the following things:
* Strings, like "hello world"
* Interpolated strings, like $"hello {name}"
* Booleans "true" and "false"
* Constructors like "new DateTime(2025, 1 ,1)"
* DateTime.Now and DateTime.Today, as well as the Date and DateTime functions to create a custom DateTime value
* Numeric values, both integer and floating point types like "1.3" and "12"
* Mathematic operators like +, -, * and /, including brackets, for example "(1 + 1) * 13"
* Binary operators && and || for example "true && true"
* Unary operator ! to inverse a boolean value like "!true"
* Comparison operators <, <=, >, >=, == and != for example "true != false"
* Indexers like MyArray[1]
* Built-in methods for DateTime values: AddDays, AddHours, AddMinuts, AddMonths, AddSeconds and AddYears
* Built-in functions for String values: Left, Right, Mid
* Built-in methods for String values: ToCamelCase, ToLower, ToPascalCase and ToUpper
* Cast and Convert functions to cast and convert values to other types
* Coalesce function, to get the first value that is not null
* In language function: "A" in ("A", "B", "C")
* ToString method to convert objects to string
* IsNull function to check for null values
* Optional support for reflection to get property values or invoke methods
* Type-safe properties, without reflection: Array.Length, String.Length, ICollection.Count, and Year/Month/Day/Hour/Minute/Second/Date for DateTime
* Support for context using delegates, in order to allow lazy evaluation
* Support for adding custom functions and generic functions, using dependency injection
* Support for adding custom expression components, using dependency injection
* Support for adding custom instance methods, constructors and properties, using dependency injection

Design decisions:
* Extendable through Dependency Injection
* Result-based, exception free. When an exception occurs, just check the Status property of the result, and you can see the details using the Exception property if you want
* Free of reflection by default, but you can enable it
* Allow duck-typing for properties and methods, with or without reflection
* Without reflection, you have to write DotExpressionComponents to implement the members
* Basic support for discoverability and validation, using a Parse method. This will return the status (Ok/Invalid) and the return type of the expression, as well as some details like the inner results, and the type that handles this expression

# QueryEvaluator

Queries are data transfer objects (C# classes) which you can fill with filters, paging information and sorting information.
These queries can then be processed by a query processor, which can use any source like an in-memory one, RDBMS or external API.

# Upgrade ProcessingPipeline from 10.x to 11.x

* IPipeline<TRequest, TResponse> is gone, you can only use IPipeline<TRequest>.
* The result extension method ProcessResult is gone, you need to add a command decorator to validate the response builder in the request.

Note that if you want to get a response, but you don't want to construct the reponse from outside (because the pipeline needs to create the response), then wrap the request and the response in a context.
This way, you have control where the response is constructed. (maybe by the context class itelf, or by the caller)