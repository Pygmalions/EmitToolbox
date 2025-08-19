using System.Linq.Expressions;

namespace EmitToolbox.Framework.Symbols.Members;

public class ActionMethodSymbol : MethodSymbol
{
    public ActionMethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
        : base(context, target, method)
    {
        if (method.ReturnType != typeof(void) || method.GetParameters().Length != 0)
        {
            throw new ArgumentException(
                "Method must be a parameterless void method.", nameof(method));
        }
    }

    public void Invoke()
    {
        Target?.EmitLoadAsTarget();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodSymbol<TArg1> : MethodSymbol
{
    public ActionMethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(void) || parameters.Length != 1 ||
            parameters[0].ParameterType != typeof(TArg1))
        {
            throw new ArgumentException(
                $"Method must have signature 'void Action({typeof(TArg1).Name})'.", nameof(method));
        }
    }

    public void Invoke(ValueSymbol<TArg1> arg1)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodSymbol<TArg1, TArg2> : MethodSymbol
{
    public ActionMethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(void) || parameters.Length != 2 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2))
        {
            throw new ArgumentException(
                $"Method must have signature 'void Action({typeof(TArg1).Name}, {typeof(TArg2).Name})'.",
                nameof(method));
        }
    }

    public void Invoke(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodSymbol<TArg1, TArg2, TArg3> : MethodSymbol
{
    public ActionMethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(void) || parameters.Length != 3 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3))
        {
            throw new ArgumentException(
                $"Method must have signature 'void Action({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name})'.",
                nameof(method));
        }
    }

    public void Invoke(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4> : MethodSymbol
{
    public ActionMethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(void) || parameters.Length != 4 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3) ||
            parameters[3].ParameterType != typeof(TArg4))
        {
            throw new ArgumentException(
                $"Method must have signature 'void Action({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name})'.",
                nameof(method));
        }
    }

    public void Invoke(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3,
        ValueSymbol<TArg4> arg4)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5> : MethodSymbol
{
    public ActionMethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(void) || parameters.Length != 5 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3) ||
            parameters[3].ParameterType != typeof(TArg4) ||
            parameters[4].ParameterType != typeof(TArg5))
        {
            throw new ArgumentException(
                $"Method must have signature 'void Action({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name}, {typeof(TArg5).Name})'.",
                nameof(method));
        }
    }

    public void Invoke(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3,
        ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : MethodSymbol
{
    public ActionMethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(void) || parameters.Length != 6 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3) ||
            parameters[3].ParameterType != typeof(TArg4) ||
            parameters[4].ParameterType != typeof(TArg5) ||
            parameters[5].ParameterType != typeof(TArg6))
        {
            throw new ArgumentException(
                $"Method must have signature 'void Action({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name}, {typeof(TArg5).Name}, {typeof(TArg6).Name})'.",
                nameof(method));
        }
    }

    public void Invoke(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3,
        ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : MethodSymbol
{
    public ActionMethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(void) || parameters.Length != 7 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3) ||
            parameters[3].ParameterType != typeof(TArg4) ||
            parameters[4].ParameterType != typeof(TArg5) ||
            parameters[5].ParameterType != typeof(TArg6) ||
            parameters[6].ParameterType != typeof(TArg7))
        {
            throw new ArgumentException(
                $"Method must have signature 'void Action({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name}, {typeof(TArg5).Name}, {typeof(TArg6).Name}, {typeof(TArg7).Name})'.",
                nameof(method));
        }
    }

    public void Invoke(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3,
        ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6, ValueSymbol<TArg7> arg7)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : MethodSymbol
{
    public ActionMethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(void) || parameters.Length != 8 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3) ||
            parameters[3].ParameterType != typeof(TArg4) ||
            parameters[4].ParameterType != typeof(TArg5) ||
            parameters[5].ParameterType != typeof(TArg6) ||
            parameters[6].ParameterType != typeof(TArg7) ||
            parameters[7].ParameterType != typeof(TArg8))
        {
            throw new ArgumentException(
                $"Method must have signature 'void Action({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name}, {typeof(TArg5).Name}, {typeof(TArg6).Name}, {typeof(TArg7).Name}, {typeof(TArg8).Name})'.",
                nameof(method));
        }
    }

    public void Invoke(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3,
        ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6, ValueSymbol<TArg7> arg7,
        ValueSymbol<TArg8> arg8)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);
        arg8.EmitLoadAsParameter(Parameters[7]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : MethodSymbol
{
    public ActionMethodSymbol(MethodBuildingContext context, ValueSymbol? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(void) || parameters.Length != 9 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3) ||
            parameters[3].ParameterType != typeof(TArg4) ||
            parameters[4].ParameterType != typeof(TArg5) ||
            parameters[5].ParameterType != typeof(TArg6) ||
            parameters[6].ParameterType != typeof(TArg7) ||
            parameters[7].ParameterType != typeof(TArg8) ||
            parameters[8].ParameterType != typeof(TArg9))
        {
            throw new ArgumentException(
                $"Method must have signature 'void Action({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name}, {typeof(TArg5).Name}, {typeof(TArg6).Name}, {typeof(TArg7).Name}, {typeof(TArg8).Name}, {typeof(TArg9).Name})'.",
                nameof(method));
        }
    }

    public void Invoke(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3,
        ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6, ValueSymbol<TArg7> arg7,
        ValueSymbol<TArg8> arg8, ValueSymbol<TArg9> arg9)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);
        arg8.EmitLoadAsParameter(Parameters[7]);
        arg9.EmitLoadAsParameter(Parameters[8]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public static class ActionMethodElementExtensions
{
    public static ActionMethodSymbol GetMethod<TTarget>(this ValueSymbol<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodSymbol(target.Context, target, call.Method);
    }

    public static ActionMethodSymbol<TArg1> GetMethod<TTarget, TArg1>(this ValueSymbol<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodSymbol<TArg1>(target.Context, target, call.Method);
    }

    public static ActionMethodSymbol<TArg1, TArg2> GetMethod<TTarget, TArg1, TArg2>(this ValueSymbol<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodSymbol<TArg1, TArg2>(target.Context, target, call.Method);
    }

    public static ActionMethodSymbol<TArg1, TArg2, TArg3> GetMethod<TTarget, TArg1, TArg2, TArg3>(
        this ValueSymbol<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodSymbol<TArg1, TArg2, TArg3>(target.Context, target, call.Method);
    }

    public static ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4> GetMethod<TTarget, TArg1, TArg2, TArg3, TArg4>(
        this ValueSymbol<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4>(target.Context, target, call.Method);
    }

    public static ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5> GetMethod<TTarget, TArg1, TArg2, TArg3, TArg4,
        TArg5>(this ValueSymbol<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5>(target.Context, target, call.Method);
    }

    public static ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> GetMethod<TTarget, TArg1, TArg2, TArg3,
        TArg4, TArg5, TArg6>(this ValueSymbol<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(target.Context, target, call.Method);
    }

    public static ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> GetMethod<TTarget, TArg1, TArg2,
        TArg3, TArg4, TArg5, TArg6, TArg7>(this ValueSymbol<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(target.Context, target,
                call.Method);
    }

    public static ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> GetMethod<TTarget, TArg1,
        TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
        this ValueSymbol<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(target.Context, target,
                call.Method);
    }

    public static ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> GetMethod<TTarget,
        TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
        this ValueSymbol<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodSymbol<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(target.Context,
                target, call.Method);
    }
}