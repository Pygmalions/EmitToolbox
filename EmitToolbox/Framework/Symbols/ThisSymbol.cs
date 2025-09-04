using System.Diagnostics.CodeAnalysis;
using EmitToolbox.Framework.Symbols.Utilities;

namespace EmitToolbox.Framework.Symbols;

public class ThisSymbol(MethodBuildingContext context, Type type) : ValueSymbol(context, type.IsValueType)
{
    public override Type ValueType { get; } = type;

    [field: MaybeNull]
    private Action<ILGenerator> ReferenceLoader =>
        field ??= ValueIndirectlyLoader.GetReferenceLoader(ValueType);
    
    public override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldarg_0);
    }

    public override void EmitDirectlyLoadAddress()
    {
        Context.Code.Emit(OpCodes.Ldarga_S, 0);
    }

    public override void EmitLoadAsValue()
    {
        EmitDirectlyLoadValue();
        if (IsReference)
            ReferenceLoader(Context.Code);
    }

    public override void EmitLoadAsAddress()
    {
        if (!IsReference)
            EmitDirectlyLoadAddress();
        else 
            EmitDirectlyLoadValue();
    }
    
    public ThisSymbol<TType> As<TType>()
    {
        return ValueType.IsAssignableTo(typeof(TType)) 
            ? new ThisSymbol<TType>(Context)
            : throw new InvalidCastException($"Type '{ValueType}' is not assignable to '{typeof(TType)}'.");
    }
}

public class ThisSymbol<TType>(MethodBuildingContext context) 
    : ValueSymbol<TType>(context, typeof(TType).IsValueType)
{
    [field: MaybeNull]
    private Action<ILGenerator> ReferenceLoader =>
        field ??= ValueIndirectlyLoader.GetReferenceLoader(typeof(TType));
    
    public override void EmitDirectlyLoadValue()
    {
        Context.Code.Emit(OpCodes.Ldarg_0);
    }

    public override void EmitDirectlyLoadAddress()
    {
        Context.Code.Emit(OpCodes.Ldarga_S, 0);
    }

    public override void EmitLoadAsValue()
    {
        EmitDirectlyLoadValue();
        if (IsReference)
            ReferenceLoader(Context.Code);
    }

    public override void EmitLoadAsAddress()
    {
        if (!IsReference)
            EmitDirectlyLoadAddress();
        else 
            EmitDirectlyLoadValue();
    }
}