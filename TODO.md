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

Function evaluator:
-Has all functions function descriptor provider injected in c'tor.
-In there, provide mapping between fuction descriptor and function, and put it in a dictionary. Throw if functions could not be found.
-Mapping from function descriptor to function: Probably the same as how we use reflection to create the function definition from a function. Maybe add an Id to make it unique?
-Also add Validate method

-Evaluate/Validate:
-When FunctionCall arrives, first checks if it's not null
-After that, check how many function descriptors are there with that function name (case insensitive)
-When 0, return invalid: Function {name} is unknown
-Check argument count of all valid functions
-When 0, return invalid: No overload of function {name} takes {count} arguments
-When >1, return invalid: Function {name} has multiple overloads with {count} arguments defined
-When 1, search for the function that corresponds to that function descriptor (found in dictionary)

After this, in ExpressionFramework and in unit tests, you don't have to check for the function name anymore, as the function evaluator takes care of this.
