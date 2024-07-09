using System.Diagnostics.Contracts;
using System.Reflection;
using System.Reflection.Emit;

namespace EmitToolbox.Framework.Contexts;

public class ClassContext
{
    public readonly Type ProxiedClass;

    public readonly TypeBuilder Builder;

    public readonly MethodBuilder Initializer;

    public ClassContext(ModuleBuilder module, Type proxiedClass, string? name = null)
    {
        ProxiedClass = proxiedClass;
        Builder = module.DefineType(name ?? proxiedClass.Name,
            TypeAttributes.Public | TypeAttributes.Class |
            TypeAttributes.AnsiClass | TypeAttributes.AutoClass |
            TypeAttributes.BeforeFieldInit);
        Builder.SetParent(proxiedClass);
        Builder.SetCustomAttribute(ProxyGenerator.GeneratedAttributeBuilder);

        Initializer = Builder.DefineMethod("_Initializer",
            MethodAttributes.Private | MethodAttributes.HideBySig,
            CallingConventions.Standard,
            null, Type.EmptyTypes);
        Initializer.SetCustomAttribute(ProxyGenerator.GeneratedAttributeBuilder);
    }

    private readonly Dictionary<MethodInfo, MethodContext> _methodContexts = new();

    [Pure]
    public MethodContext OverrideMethod(MethodInfo baseMethod)
    {
        if (_methodContexts.TryGetValue(baseMethod, out var context))
            return context;
        context = new MethodContext(this, baseMethod);
        _methodContexts[baseMethod] = context;
        return context;
    }

    private void OverrideConstructor(ConstructorInfo baseConstructor)
    {
        var parameters = baseConstructor.GetParameters();
        var constructorFlags =
            MethodAttributes.HideBySig |
            MethodAttributes.SpecialName |
            MethodAttributes.RTSpecialName;
        
        // Set visibility of the proxy constructor as the same as the base constructor.
        if (baseConstructor.IsPublic)
            constructorFlags |= MethodAttributes.Public;
        else if (baseConstructor.IsFamily)
            constructorFlags |= MethodAttributes.Family;
        else if (baseConstructor.IsAssembly)
            constructorFlags |= MethodAttributes.Assembly;
        else if (baseConstructor.IsFamilyAndAssembly)
            constructorFlags |= MethodAttributes.FamANDAssem;
        else if (baseConstructor.IsFamilyOrAssembly)
            constructorFlags |= MethodAttributes.FamORAssem;
        else
            constructorFlags |= MethodAttributes.Private;

        var constructor = Builder.DefineConstructor(
            constructorFlags, CallingConventions.Standard,
            parameters.Select(info => info.ParameterType).ToArray());
        constructor.SetCustomAttribute(ProxyGenerator.GeneratedAttributeBuilder);
        var code = constructor.GetILGenerator();
        for (var argumentIndex = 0; argumentIndex <= parameters.Length; ++argumentIndex)
            // Load constructor arguments to the stack.
            code.Emit(OpCodes.Ldarg, argumentIndex);
        // Call base constructor.
        code.Emit(OpCodes.Call, baseConstructor);

        // Invoke the initializer method.
        code.Emit(OpCodes.Ldarg_0);
        code.Emit(OpCodes.Call, Initializer);

        code.Emit(OpCodes.Ret);
    }
    
    /// <summary>
    /// Build the proxy type.
    /// </summary>
    /// <returns>Proxy type.</returns>
    internal Type Build()
    {
        // Complete initializer method.
        Initializer.GetILGenerator().Emit(OpCodes.Ret);
        
        // Generate proxy constructors.
        foreach (var constructor in ProxiedClass.GetConstructors())
            OverrideConstructor(constructor);
        
        // Generate proxy methods.
        foreach (var (_, proxyMethod) in _methodContexts)
            proxyMethod.Build();

        return Builder.CreateType();
    }
}