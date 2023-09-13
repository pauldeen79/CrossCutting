# CrossCutting
Generic utilities and other stuff, usable in any layer of any solution. We're targeting .NET Standard 2.0, so you can even use .NET Framework 4.x if you want to.

This repository consists of the following packages:

| Package name                          | Description                                                                                                                                                         |
| :------------------------------------ | :------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| CrossCutting.Common                   | Provider for system date and user name, some useful extension methods on System.Object and System.String, and the Result class                                      |
| CrossCutting.Common.Testing           | Helps you test constructors on null checks                                                                                                                          |
| CrossCutting.Data.Abstractions        | Abstraction for executing database commands using System.Data namespace (IDbConnection and IDbCommand)                                                              |
| CrossCutting.Data.Core                | Default implementation of database commands                                                                                                                         |
| CrossCutting.Data.Sql                 | Extension methods for working with database commands in System.Data namespace (IDbConnection)                                                                       |
| CrossCutting.DataTableDumper          | Produces flat-text data tables from objects                                                                                                                         |
| CrossCutting.Utilities.ObjectDumper   | Produces readable flat-text representation from objects, for use in logging                                                                                         |
| CrossCutting.Utilities.Parsers        | Parser for pipe-delmited data table strings, TSQL INSERT INTO statements, function strings, math expressions and formattable strings (dynamic interpolated strings) |
| System.Data.Stub                      | Stubs for System.Data interfaces like IDbConnection, IDbCommand and IDataReader                                                                                     |

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
            t => CreateInstance(t, parameterType => Substitute.For(new[] { parameterType }, Array.Empty<object>()), parameterReplaceDelegate, constructorPredicate),
            parameterPredicate,
            parameterReplaceDelegate,
            constructorPredicate);
```

You can easily replace NSubstitute with Moq, if you want, by changing the factory delegate argument to this:

```C#
t => CreateInstance(t, parameterType => ((Mock)Activator.CreateInstance(typeof(Mock<>).MakeGenericType(parameterType))).Object, parameterReplaceDelegate, constructorPredicate),
```
