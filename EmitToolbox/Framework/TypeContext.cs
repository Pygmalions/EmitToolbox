using System.Runtime.InteropServices;

namespace EmitToolbox.Framework;

public class TypeContext(TypeBuilder builder)
{
    /// <summary>
    /// Reflection information about this constructing type.
    /// </summary>
    public Type BuildingType { get; } = builder;

    public Type? BuiltType { get; private set; }

    public void Build()
    {
        if (BuiltType != null)
            throw new InvalidOperationException("The type has already been built.");

        BuiltType = builder.CreateType();
    }

    private Type[][]? BuildParameterRequiredModifiers(
        int parameterCount,
        int[]? inParameterIndices = null,
        int[]? outParameterIndices = null)
    {
        if (inParameterIndices == null && outParameterIndices == null)
            return null;

        var parameterRequiredModifiers = new Type[][parameterCount];
        for (var modifierIndex = 0; modifierIndex < parameterRequiredModifiers.Length; modifierIndex++)
        {
            parameterRequiredModifiers[modifierIndex] = Type.EmptyTypes;
        }

        if (inParameterIndices != null)
        {
            foreach (var parameterIndex in inParameterIndices)
            {
                parameterRequiredModifiers[parameterIndex] = [typeof(InAttribute)];
            }
        }

        if (outParameterIndices != null)
        {
            foreach (var parameterIndex in outParameterIndices)
            {
                parameterRequiredModifiers[parameterIndex] = [typeof(OutAttribute)];
            }
        }

        return parameterRequiredModifiers;
    }

    private MethodAttributes BuildInstanceMethodAttributes(VisibilityLevel visibility,
        bool isVirtual = false, bool isAbstract = false,
        bool isSealed = false, bool isNewSlot = false)
    {
        var attributes = MethodAttributes.HideBySig;

        attributes |= visibility switch
        {
            VisibilityLevel.Public => MethodAttributes.Public,
            VisibilityLevel.Private => MethodAttributes.Private,
            VisibilityLevel.Protected => MethodAttributes.Family,
            VisibilityLevel.Internal => MethodAttributes.Assembly,
            VisibilityLevel.ProtectedInternal => MethodAttributes.FamORAssem,
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        };

        if (isVirtual)
            attributes |= MethodAttributes.Virtual;
        if (isAbstract)
            attributes |= MethodAttributes.Abstract;
        if (isSealed)
            attributes |= MethodAttributes.Final;
        if (isNewSlot)
            attributes |= MethodAttributes.NewSlot;

        return attributes;
    }

    private MethodAttributes BuildStaticMethodAttributes(VisibilityLevel visibility)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.Static;

        attributes |= visibility switch
        {
            VisibilityLevel.Public => MethodAttributes.Public,
            VisibilityLevel.Private => MethodAttributes.Private,
            VisibilityLevel.Protected => MethodAttributes.Family,
            VisibilityLevel.Internal => MethodAttributes.Assembly,
            VisibilityLevel.ProtectedInternal => MethodAttributes.FamORAssem,
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        };

        return attributes;
    }

    public MethodContext DefineInstanceMethod(string name, Type[] parameterTypes, Type returnType,
        VisibilityLevel visibility = VisibilityLevel.Public,
        bool isVirtual = false, bool isAbstract = false,
        bool isSealed = false, bool isNewSlot = false,
        int[]? inParameterIndices = null, int[]? outParameterIndices = null)
    {
        var attributes = BuildInstanceMethodAttributes(visibility, isVirtual, isAbstract, isSealed, isNewSlot);

        var methodBuilder = builder.DefineMethod(name, attributes, CallingConventions.Standard,
            returnType, null, null,
            parameterTypes,
            BuildParameterRequiredModifiers(parameterTypes.Length, inParameterIndices, outParameterIndices), null);
        if (returnType == typeof(void))
            return new ActionMethodContext(methodBuilder, methodBuilder.GetILGenerator());
        return new FunctorMethodContext(methodBuilder, methodBuilder.GetILGenerator());
    }

    public MethodContext DefineStaticMethod(string name, Type[] parameterTypes, Type returnType,
        VisibilityLevel visibility = VisibilityLevel.Public,
        int[]? inParameterIndices = null, int[]? outParameterIndices = null)
    {
        var attributes = BuildStaticMethodAttributes(visibility);

        var methodBuilder = builder.DefineMethod(name, attributes, CallingConventions.Standard,
            returnType, null, null,
            parameterTypes,
            BuildParameterRequiredModifiers(parameterTypes.Length, inParameterIndices, outParameterIndices), null);
        if (returnType == typeof(void))
            return new ActionMethodContext(methodBuilder, methodBuilder.GetILGenerator());
        return new FunctorMethodContext(methodBuilder, methodBuilder.GetILGenerator());
    }

    public FunctorMethodContext<TResult> DefineInstanceMethod<TResult>(string name, Type[] parameterTypes,
        VisibilityLevel visibility = VisibilityLevel.Public,
        bool isVirtual = false, bool isAbstract = false,
        bool isSealed = false, bool isNewSlot = false,
        int[]? inParameterIndices = null, int[]? outParameterIndices = null)
    {
        var attributes = BuildInstanceMethodAttributes(visibility, isVirtual, isAbstract, isSealed, isNewSlot);

        var methodBuilder = builder.DefineMethod(name, attributes, CallingConventions.Standard,
            typeof(TResult), null, null,
            parameterTypes,
            BuildParameterRequiredModifiers(parameterTypes.Length, inParameterIndices, outParameterIndices), null);
        return new FunctorMethodContext<TResult>(methodBuilder, methodBuilder.GetILGenerator());
    }

    public FunctorMethodContext<TResult> DefineStaticMethod<TResult>(string name, Type[] parameterTypes,
        VisibilityLevel visibility = VisibilityLevel.Public,
        int[]? inParameterIndices = null, int[]? outParameterIndices = null)
    {
        var attributes = BuildStaticMethodAttributes(visibility);

        var methodBuilder = builder.DefineMethod(name, attributes, CallingConventions.Standard,
            typeof(TResult), null, null,
            parameterTypes,
            BuildParameterRequiredModifiers(parameterTypes.Length, inParameterIndices, outParameterIndices), null);
        return new FunctorMethodContext<TResult>(methodBuilder, methodBuilder.GetILGenerator());
    }
}

public static class ClassContextExtensions
{
    public static TypeContext DefineClass(this ModuleBuilder module, string name,
        bool isPublic = true, Type? parent = null)
    {
        var attributes = TypeAttributes.Class | TypeAttributes.AnsiClass | TypeAttributes.AutoLayout;
        if (isPublic)
            attributes |= TypeAttributes.Public;
        else
            attributes |= TypeAttributes.NotPublic;

        var builder = module.DefineType(name, attributes, parent);

        return new TypeContext(builder);
    }

    public static TypeContext DefineStruct(this ModuleBuilder module, string name, bool isPublic = true)
    {
        var attributes = TypeAttributes.SequentialLayout | TypeAttributes.AnsiClass;
        if (isPublic)
            attributes |= TypeAttributes.Public;
        else
            attributes |= TypeAttributes.NotPublic;

        var builder = module.DefineType(name, attributes);

        return new TypeContext(builder);
    }
}