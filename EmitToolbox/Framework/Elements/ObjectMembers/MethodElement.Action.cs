using System.Linq.Expressions;

namespace EmitToolbox.Framework.Elements.ObjectMembers;

public class ActionMethodElement : MethodElement
{
    public ActionMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
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

public class ActionMethodElement<TArg1> : MethodElement
{
    public ActionMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
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

    public void Invoke(ValueElement<TArg1> arg1)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsValue();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodElement<TArg1, TArg2> : MethodElement
{
    public ActionMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
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

    public void Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsValue();
        arg2.EmitLoadAsValue();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodElement<TArg1, TArg2, TArg3> : MethodElement
{
    public ActionMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
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

    public void Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsValue();
        arg2.EmitLoadAsValue();
        arg3.EmitLoadAsValue();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodElement<TArg1, TArg2, TArg3, TArg4> : MethodElement
{
    public ActionMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
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

    public void Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsValue();
        arg2.EmitLoadAsValue();
        arg3.EmitLoadAsValue();
        arg4.EmitLoadAsValue();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5> : MethodElement
{
    public ActionMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
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

    public void Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4, ValueElement<TArg5> arg5)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsValue();
        arg2.EmitLoadAsValue();
        arg3.EmitLoadAsValue();
        arg4.EmitLoadAsValue();
        arg5.EmitLoadAsValue();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : MethodElement
{
    public ActionMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
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

    public void Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsValue();
        arg2.EmitLoadAsValue();
        arg3.EmitLoadAsValue();
        arg4.EmitLoadAsValue();
        arg5.EmitLoadAsValue();
        arg6.EmitLoadAsValue();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : MethodElement
{
    public ActionMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
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

    public void Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsValue();
        arg2.EmitLoadAsValue();
        arg3.EmitLoadAsValue();
        arg4.EmitLoadAsValue();
        arg5.EmitLoadAsValue();
        arg6.EmitLoadAsValue();
        arg7.EmitLoadAsValue();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : MethodElement
{
    public ActionMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
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

    public void Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7,
        ValueElement<TArg8> arg8)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsValue();
        arg2.EmitLoadAsValue();
        arg3.EmitLoadAsValue();
        arg4.EmitLoadAsValue();
        arg5.EmitLoadAsValue();
        arg6.EmitLoadAsValue();
        arg7.EmitLoadAsValue();
        arg8.EmitLoadAsValue();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public class ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : MethodElement
{
    public ActionMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
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

    public void Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7,
        ValueElement<TArg8> arg8, ValueElement<TArg9> arg9)
    {
        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsValue();
        arg2.EmitLoadAsValue();
        arg3.EmitLoadAsValue();
        arg4.EmitLoadAsValue();
        arg5.EmitLoadAsValue();
        arg6.EmitLoadAsValue();
        arg7.EmitLoadAsValue();
        arg8.EmitLoadAsValue();
        arg9.EmitLoadAsValue();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
    }
}

public static class ActionMethodElementExtensions
{
    public static ActionMethodElement GetMethod<TTarget>(this ValueElement<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodElement(target.Context, target, call.Method);
    }

    public static ActionMethodElement<TArg1> GetMethod<TTarget, TArg1>(this ValueElement<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodElement<TArg1>(target.Context, target, call.Method);
    }

    public static ActionMethodElement<TArg1, TArg2> GetMethod<TTarget, TArg1, TArg2>(this ValueElement<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodElement<TArg1, TArg2>(target.Context, target, call.Method);
    }

    public static ActionMethodElement<TArg1, TArg2, TArg3> GetMethod<TTarget, TArg1, TArg2, TArg3>(
        this ValueElement<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodElement<TArg1, TArg2, TArg3>(target.Context, target, call.Method);
    }

    public static ActionMethodElement<TArg1, TArg2, TArg3, TArg4> GetMethod<TTarget, TArg1, TArg2, TArg3, TArg4>(
        this ValueElement<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodElement<TArg1, TArg2, TArg3, TArg4>(target.Context, target, call.Method);
    }

    public static ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5> GetMethod<TTarget, TArg1, TArg2, TArg3, TArg4,
        TArg5>(this ValueElement<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5>(target.Context, target, call.Method);
    }

    public static ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> GetMethod<TTarget, TArg1, TArg2, TArg3,
        TArg4, TArg5, TArg6>(this ValueElement<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(target.Context, target, call.Method);
    }

    public static ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> GetMethod<TTarget, TArg1, TArg2,
        TArg3, TArg4, TArg5, TArg6, TArg7>(this ValueElement<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(target.Context, target,
                call.Method);
    }

    public static ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> GetMethod<TTarget, TArg1,
        TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(
        this ValueElement<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(target.Context, target,
                call.Method);
    }

    public static ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> GetMethod<TTarget,
        TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
        this ValueElement<TTarget> target,
        Expression<Action<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new ActionMethodElement<TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(target.Context,
                target, call.Method);
    }
}