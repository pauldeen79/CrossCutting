TODO
- Low priority: Refactor ParseResult and ProcessResult (used by some older parsers in this project) to use the regular Result class
- Low priority: Check whether Validate methods should fill validation results with IEnumerable<ValidationError>. You could still use Result<Type> to return the type...

- Add a setting in FunctionEvaluator to enable or disable type argument checking