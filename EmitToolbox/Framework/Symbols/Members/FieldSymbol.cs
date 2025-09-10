using System.Linq.Expressions;
using EmitToolbox.Framework.Symbols.Extensions;

namespace EmitToolbox.Framework.Symbols.Members;

public class FieldSymbol : IAssignableSymbol, IAddressableSymbol
{
    public DynamicMethod Context { get; }
    
    public FieldInfo Field { get; }
    
    public Type ContentType { get; }

    public ISymbol? Target { get; }
    
    public FieldSymbol(DynamicMethod context, FieldInfo field, ISymbol? target)
    {
        Context = context;
        Field = field;
        ContentType = field.FieldType;
        Target = target;
        if (field.IsStatic)
            return;
        if (target == null)
            throw new ArgumentException("Cannot create a instance field symbol: target instance is null.", 
                nameof(target));
        if (!target.ContentType.WithoutByRef().IsAssignableTo(field.DeclaringType))
            throw new ArgumentException(
                "Cannot create a instance field symbol: " +
                "target instance cannot be assigned to the declaring type of the field.",
                nameof(target));
    }

    public void EmitLoadContent()
    {
        if (Target != null)
        {
            Target.EmitLoadAsTarget();
            Context.Code.Emit(OpCodes.Ldfld, Field);
            return;
        }
        
        Context.Code.Emit(OpCodes.Ldsfld, Field);
    }

    public void EmitStoreContent()
    {
        if (Target != null)
        {
            var temporary = Context.Code.DeclareLocal(ContentType.WithoutByRef());
            Context.Code.Emit(OpCodes.Stloc, temporary);
            Target.EmitLoadAsTarget();
            Context.Code.Emit(OpCodes.Ldloc, temporary);
            Context.Code.Emit(OpCodes.Stfld, Field);
            return;
        }
        
        Context.Code.Emit(OpCodes.Stsfld, Field);
    }

    public void EmitLoadAddress()
    {
        if (Target != null)
        {
            Target.EmitLoadAsTarget();
            Context.Code.Emit(OpCodes.Ldflda, Field);
            return;
        }
        
        Context.Code.Emit(OpCodes.Ldsflda, Field);
    }
}

public static class FieldSymbolExtensions
{
    public static FieldSymbol FieldOf(this ISymbol symbol, FieldInfo field)
        => new (symbol.Context, field, symbol);
    
    public static FieldSymbol FieldOf<TTarget, TField>(
        this ISymbol<TTarget> target, Expression<Func<TTarget, TField>> expression)
    {
        return expression.Body is not MemberExpression memberExpression
            ? throw new ArgumentException("Expression must be a field access expression.", nameof(expression))
            : new FieldSymbol(target.Context, (FieldInfo)memberExpression.Member, target);
    }
}