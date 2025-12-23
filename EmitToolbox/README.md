# EmitToolbox

Using `System.Reflection.Emit` to dynamically create types and methods at runtime is prone to errors and 
can be quite complex.
To effectively emit dynamic IL code,
one must have a deep understanding of the Common Intermediate Language (CIL) and the .NET runtime.
The `EmitToolbox` library aims to simplify this process by providing a set of high-level abstractions and utilities.

## Concepts

- 'DynamicAssembly': Builder for defining dynamic assemblies. It can define classes and structs. 
- 'DynamicType': Builder for defining dynamic types. Use it to create `DynamicMethod`, `DynamicField` and `DynamicProperty` instances.
- 'DynamicFunction': Builder for defining the code of dynamic methods. This framework provides rick extension methods to it.
- 'DynamicField': Builder for defining dynamic fields.
- 'DynamicProperty': Builder for defining dynamic properties and corresponding setters and getters.

## Usage

### Create a Dynamic Assembly

Use the following code to create an executable dynamic assembly:
```csharp
var assembly = DynamicAssembly.DefineExecutable("SampleAssembly");
```

Or you can create an assembly that can be exported to a file but cannot be executed directly:
```csharp
var assembly = DynamicAssembly.DefineExportable("SampleAssembly");

// ...

assemly.Export("./MyDynamicAssembly.dll");

```

### Basic Example

```csharp
using EmitToolbox.Framework;
using EmitToolbox.Framework.Extensions;
using EmitToolbox.Framework.Symbols;

var assembly = DynamicAssembly.DefineExecutable("SampleAssembly");
var type = assembly.DefineClass("SampleClass");

// Define an instance field named 'Backing' of type 'int'
var backingField = type.FieldFactory.DefineInstance(typeof(int), "Backing");
// Define an instance property named 'Value' of type 'int'
var valueProperty = type.PropertyFactory.DefineInstance<int>("Value");

// Define and bind getter accessor for the property
var getter = type.MethodFactory.Instance.DefineFunctor<int>(
    "get_Value", [], hasSpecialName: true);

// Get the 'this' instance symbol.
// This symbol is used to access the instance field in the method body.
var thisSymbol = getter.This();
// Convert the dynamic field into a symbol than can be used in the method building context
// by binding it to the 'getter' method and 'this' instance.
var fieldSymbol = backingField.SymbolOf<int>(getter, thisSymbol);

// Return the value of the field + 1.
// Here, extension operator '+' for 'ISymbol<int>' is used.
// Use the namespace 'EmitToolbox.Framework.Extensions' to access these extension methods.
// 'getter.Value(1)' is to create a literal symbol that represents the value 1.
getter.Return(fieldSymbol + getter.Value(1));

// Bind the 'getter' method as the getter accessor of this property.
valueProperty.BindGetter(getter);
// After building this type, the type and its methods, fields, and properties can be used.
type.Build();

// Following example is about to use this type throw reflection.
// Usually, the built types implement interfaces, and they should be used as interfaces for better performance.
// This part is only for demonstration purpose.

// Instantiate an instance of the built type.
var instance = Activator.CreateInstance(type.BuildingType)!;
// Create a functor from the getter accessor of the property.
var functor = valueProperty.Getter!.BuildingMethod.CreateDelegate<Func<int>>(testInstance);

// Using reflection to set the value of the backing field to 1.
backingField.BuildingField.SetValue(instance, 1);
// Then invoke the functor, the result should be 3.
var result = functor(2);
```

### Define an Async Method

Suppose we have async methods like this:

```csharp
public static Task<int> ReturnCompletedTask1()
{
    return Task.FromResult(1);
}

public static async Task<int> ReturnDelayedTask1()
{
    await Task.Delay(10);
    return 1;
}
```
And we want to define an async method that has the same behavior as the following code:

```csharp
public async Task<int> DynamicMethod()
{
    return (await ReturnCompletedTask1()) + (await ReturnDelayedTask1());
}
```

```csharp

var assembly = DynamicAssembly.DefineExecutable("SampleAssembly");
var type = assembly.DefineClass("SampleClass");

var method = type.MethodFactory.Static.DefineFunctor<Task<int>>(
    nameof("DynamicMethod"));

var asyncBuilder = method.DefineAsyncStateMachine();
// Note that the async part is defined in another method context.
// It is actually the 'MoveNext' method of the state machine.
var asyncMethod = asyncBuilder.Method;
// Define a new async step with 'AwaitResult' method, which can await task-like objects.
var symbolNumber1 = asyncBuilder.AwaitResult(
    asyncMethod.Invoke(() => ReturnCompletedValueTask1()));
// Note that the 'MoveNext' methods may be called multiple times for different steps;
// therefore, temporary variables will be lost and need to be retained.
// Results of the 'AwaitResult' are stored in fields of the state machine,
// and other variables need to be manually retained through the 'Retain(ISymbol)' method.
var symbolNumber2 = asyncBuilder.AwaitResult(
    asyncMethod.Invoke(() => ReturnDelayedValueTask1()));
var result = symbolNumber1 + symbolNumber2;
// Set the result value as the result of the task.
asyncBuilder.Complete(result);

// The method returns the task of the state machine.
method.Return(asyncBuilder.Invoke().AsSymbol<Task<int>>());

type.Build();

// Use the method:
var functor = method.BuildingMethod.CreateDelegate<Func<Task<int>>>();
var result = await functor(); // Result is 2.
```

## Related Resources
- [CLI Specification (ECMA-335)](https://ecma-international.org/publications-and-standards/standards/ecma-335/)