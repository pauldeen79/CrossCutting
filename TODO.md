TODO

- Create MathematicExpressionParser in FunctionParser project
  (1+1)/2 HMVDWOA splitting, using delegates to process the two parts (left and right of the operator) and support for text delimiter
  Empty result/NotSupported or something when no delimiters are found. Or just success with an empty list. Should have recursion in the result.
- Create FormattableStringParser in FunctionParser project
  Hello {Name}, Your age is {{0}}
  { } splitting using double {{ }} to escape, and using delegates to process the formattable parts
  Should always return a string. When no formatters were found, then just the same. Otherwise, the formatted string.
- Create integration test to combine MathematicExpressionParser, FormattableStringParser and FunctionParser.
  When necessary, create convenience composition class or something (in other words: it should be easy to use)
  @"Hello {1+1}" should be processed as Hello {mathematic expression result}
  @"Hello {Name}" should be processed as Hello {get Name property of context}
  "Hello {1+1}" should be processed as a simple string - no formattable/interpolated string because it does not start with @
  "FUNCTION(\"a\")" should be processed as a function
  order: FormattableString, Function, MathematicExpression