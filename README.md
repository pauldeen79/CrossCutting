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
