using EmitToolbox.Extensions;
using EmitToolbox.Symbols;

namespace EmitToolbox.Builders;

public static class MethodBuilderFacade
{
    internal static MethodBuilder CreateMethodBuilder(
        TypeBuilder typeBuilder,
        string name, MethodAttributes attributes,
        ParameterDefinition[] parameters,
        Type resultType, Type[] resultAttributes)
    {
        return typeBuilder.DefineMethod(name, attributes, CallingConventions.Standard,
            resultType,
            null, resultAttributes,
            parameters.SelectTypes().ToArray(),
            parameters.ToRequiredCustomModifiers().ToArray(),
            parameters.ToOptionalCustomModifiers().ToArray());
    }

    internal static MethodBuilder CreateMethodBuilder(
        TypeBuilder typeBuilder, string name, MethodInfo declaration)
    {
        var parameters = declaration.GetParameters();
        var parameterDefinitions = parameters.ToDefinitions().ToArray();
        var methodBuilder = typeBuilder.DefineMethod(
            name,
            declaration.Attributes & ~MethodAttributes.Abstract, // Cancel the abstract flag if present.
            CallingConventions.Standard,
            declaration.ReturnType, null, null,
            parameterDefinitions.SelectTypes().ToArray(),
            parameterDefinitions.ToRequiredCustomModifiers().ToArray(),
            parameterDefinitions.ToOptionalCustomModifiers().ToArray()
        );

        // Set the same parameter names and attributes.
        foreach (var (index, parameter) in parameters.Index())
            methodBuilder.DefineParameter(index + 1, parameter.Attributes, parameter.Name);

        return methodBuilder;
    }

    internal static Action CreateReturnResultDelegate(ILGenerator code)
    {
        return () => code.Emit(OpCodes.Ret);
    }

    internal static Action<TSymbol> CreateReturnResultDelegate<TSymbol>(
        ILGenerator code, Type returnType) where TSymbol : ISymbol
    {
        return symbol =>
        {
            if (!symbol.BasicType.IsAssignableTo(returnType.BasicType))
                throw new InvalidOperationException(
                    "Specified return value symbol is not assignable to the return type of this method.");
            if (returnType.IsByRef)
                symbol.LoadAsReference();
            else
                symbol.LoadAsValue();
            code.Emit(OpCodes.Ret);
        };
    }
}