TODO
- Low priority: Refactor ParseResult and ProcessResult (used by some older parsers in this project) to use the regular Result class

VNEXT

* Add descriptors of functions, so you can see which functions are available
* Add provider for these descriptors. Default implementation just gets all descriptors injected, and returns these
* Add validator for a FunctionCall (FunctionParseResult), so you can check whether your call is valid (and not just execute it)
  Or, add it to function evaluator interface
* In the function evaluator (DefaultFunctionParseResultEvaluator), first perform the validation, and only if it succeeds, then execute/evaluate the function
* After changing this, check whether there is still a gap between function parsing and expression parsing (as in ExpressionFramework)
  Or, maybe each Expression implementation can have a (static?) method that provides a FunctionDescriptor, and a method to strongly-typed create a FunctionCall
  --> Test this by creating some a proof of concept inside the Parsers.Tests project

IFunctionDescriptor:
string Name
string Description //optional
IFunctionDescriptorArgument[] Arguments

IFunctionDescriptorArgument:
string Name
string Description //optional
bool IsRequired
string TypeName

IFunctionDescriptorProvider:
IFunctionDescriptor[] GetAll()

Maybe also add support for attributes (RequiredAttribute and DescriptionAttribute), so you can easily fill them from metadata on all registered functions.
