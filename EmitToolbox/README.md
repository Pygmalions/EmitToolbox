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