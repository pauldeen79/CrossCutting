TODO

- Create MathematicExpressionParser in FunctionParser project
  (1+1)/2 HMVDWOA splitting, using delegates to process the two parts (left and right of the operator) and support for text delimiter
  Empty result/NotSupported or something when no delimiters are found. Or just success with an empty list. Should have recursion in the result.
- Create integration test to combine MathematicExpressionParser, FormattableStringParser and FunctionParser.
  When necessary, create convenience composition class or something (in other words: it should be easy to use) --> ExpressionParser
  "Hello {1+1}" should be processed as Hello {mathematic expression result}
  "Hello {Name}" should be processed as Hello {get Name property of context}
  "Hello {MYFUNCTION("Name")}" should be processed as Hello {call MYFUNTION name with "Name" as argument}
  "Hello {1+1}" should be processed as a simple string - no formattable/interpolated string because it does not start with @
  "FUNCTION(\"a\")" should be processed as a function
  order: FormattableString, MathematicExpression, Function
  "Hello {1+MYFUNCTION(argument)}" -> calculate mathematical expression from the formattable string, each part should be parsed as a function. if no function is found, try parsing as decimal/int/long.