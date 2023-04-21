TODO

- Create convenience composition class or something (in other words: it should be easy to use) --> ExpressionParser
  Mimic Excel, so it should always start with an equals sign (=) for an expression.
  Support for string concatenation like this: ="Hello " & FUNCTION() & "!" --> "Hello [function outcome]!"
  ="Hello {1+1}" should be processed as Hello {mathematic expression result} -> Hello 2
  ="Hello {Name}" should be processed as Hello {get Name property of context}
  ="Hello {MYFUNCTION("Name")}" should be processed as Hello {call MYFUNTION name with "Name" as argument}
  ="Hello {1+1}" should be processed as a simple string - no formattable/interpolated string because it does not start with @
  =FUNCTION(\"a\") should be processed as a function
  =1+1 should be processed as mathematics
  order: ExpressionParser (outer), MathematicExpression, FormattableString (inner)
  ="Hello {1+MYFUNCTION(argument)}" -> calculate mathematical expression from the formattable string, each part should be parsed as a function. if no function is found, try parsing as decimal/int/long.
- Create default implementation for parsing string to int/long/decimal/etc? Can be used when using MathematicExpressionParser, FunctionParser or ExpressionStringParser