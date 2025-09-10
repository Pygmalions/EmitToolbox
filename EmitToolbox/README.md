# EmitToolbox

Using `System.Reflection.Emit` to dynamically create types and methods at runtime is
prone to errors and can be quite complex.
To effectively emit dynamic IL code,
one must have a deep understanding of the Common Intermediate Language (CIL) and the .NET runtime.
The `EmitToolbox` library aims to simplify this process by providing a set of high-level abstractions and utilities.

## Framework

Contexts:

- `DynamicAssembly` - Context for building dynamic assemblies.
- `DynamicType` - Context for building dynamic types.
- `DynamicMethod` - Context for building dynamic methods.
- ㇄ `DynamicAction` - Context for building dynamic methods that does not have a return value.
- ㇄ `DynamicFunctor` - Context for building dynamic methods that has a return value.
- `DynamicField` - Context for building fields of the dynamic types.
- `DynamicProperty` - Context for building properties of the dynamic types.

## Usage

### Basic Usage

```csharp
using EmitToolbox;

// Define an context for an executable (and cannot be saved) assembly.
var assemblyContext = DynamicAssembly.DefineExecutable("SampleDynamicAssembly");
            
// Define a context for a class type within the assembly.
var typeContext = assemblyContext.DefineClass("SampleClass"); 

// Define a field.
var fieldContext = typeContext.FieldBuilder.DefineInstance("_value", typeof(int), VisibilityLevel.Private);

// Define a method.
var methodContext = typeContext.FunctorBuilder.DefineInstance("AddAndSet", 
            [ParameterDefinition.Value<int>()], ResultDefinition.Value<int>());
var argumentSymbol = methodContext.Argument<int>(1)
var fieldSymbol = fieldContext.SymbolOf(methodContext.This);
var resultSymbol = fieldSymbol.Add(argumentSymbol);
fieldSymbol.AssignFrom(resultSymbol);
methodContext.Return(resultSymbol);

// Generate the type.
typeContext.Build();

// Create an instance of the generated type.
var instance = Activator.CreateInstance(typeContext.BuildingType);

var result1 = methodContext.BuildingMethod.Invoke(null, [1])
Console.WriteLine(result1); // Output: 1
var result2 = methodContext.BuildingMethod.Invoke(null, [2])
Console.WriteLine(result2); // Output: 3
```
Create a symbol for an argument: `methodContext.Argument<int>(0)`.

Create a local variable: `methodContext.Variable<int>()`.

Create a symbol for a literal value: `methodContext.Value(123)`.

Supported literal types:
- Integers: `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`;
- Floating points: `float`, `double`, `decimal`;
- `bool`, `char`, `string`;
- `null` for reference types.
- Enumeration values.
- Metadata: `Type`, `FieldInfo`, `PropertyInfo`, `ConstructorInfo`, `MethodInfo`;

### Array Facade

```csharp
// Use .AsArray() to convert a symbol of an array type to an array facade.
var array = methodContext.Argument<int[]>(0).AsArray();

var elementFromLiteralIndex = array[0]; // Get an element using a literal index.
var elementFromSymbolIndex = array[methodContext.Argument<int>(1)]; // Get an element using a index symbol.
```

### If-Else

Following code can generate a method which is equal to `(bool condition) => condition ? 1 : 0`:
```csharp
var methodContext = typeContext.FunctorBuilder.DefineStatic("Test",
    [ParameterDefinition.Value<bool>()], ResultDefinition.Value<int>());
var argument = methodContext.Argument<bool>(0);
methodContext.If(argument,
    () =>
    {
        methodContext.Return(methodContext.Value(1));
    },
    () =>
    {
        methodContext.Return(methodContext.Value(0)); 
    });
methodContext.Return(methodContext.Value(-1));
```
### Loop

For C# code like below:
```csharp
int method(int arg)
{
    while (arg != 0)
    {
        arg -= 1;
    }
    return arg;
}
```

Corresponding code to generate such method is:
```csharp
var methodContext = typeContext.FunctorBuilder.Static("Test",
    [ParameterDefinition.Value<int>()], ResultDefinition.Value<int>());
var argument = methodContext.Argument<int>(0);
methodContext.While(
    methodContext.Expression(() =>
        argument.IsEqualTo(methodContext.Value(0)).Negate()),
    () => argument.SelfSubtract(1));
methodContext.Return(argument);
```


