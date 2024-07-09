# EmitToolbox Framework

This library provides an extensible framework which allows developers to
develop plugins to integrate code of different aspects into one proxy class.

## Concepts

### Proxy Handler

Implement this interface with your code of AOP.
`Process` method will be invoked when this handler participate the creation process of a proxy class.

### Proxy Generator

A proxy generator is a set of handlers with code driving the process of generating proxy class.
After adding handlers to a proxy generator through its `Handlers` list, 
you can use `Create` method to generate proxy classes.

### Class Context

A context contains information about the proxy class to generate.

#### Initializer Method
It contains a dynamic type builder and a dynamic method named `Initializer`.
An initializer is a method which will be invoked at the tail of every overriden constructors.
You can emit your own initializer code to `Initializer`.

#### Get Method Context
By invoking the `OverrideMethod` method, you can get the method context according to the `MethodInfo` of the method.
Be aware, only if the overriden method is abstract or virtual, the method built by the method context will override it.

### MethodContext

A method context contains the information to generate a proxy method.
Proxy code is wrapping the proxied method, therefore, the proxy code is separated into two parts:
1. Preprocess: before the proxied code is invoked.
2. Postprocess: after the proxied code is invoked.

In the handler, you can await `EmittingInvocation` to separate the code before and after the invocation of 
proxied method.
Code after `await methodContext.EmittingInvocation` will be invoked after the
invocation code of proxied method is emitted.

#### Local Variable `Skipped`

In the preprocess part, you can set this variable to be false to skip the proxied method.
And in the postprocess part, the value of this variable indicates that whether the proxied method is invoked.