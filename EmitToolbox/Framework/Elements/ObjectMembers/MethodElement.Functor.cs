using System.Linq.Expressions;

namespace EmitToolbox.Framework.Elements.ObjectMembers;

public class FunctorMethodElement<TResult> : MethodElement
{
    public FunctorMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
        : base(context, target, method)
    {
        if (method.ReturnType != typeof(TResult) || method.GetParameters().Length != 0)
        {
            throw new ArgumentException(
                $"Method must have signature '{typeof(TResult).Name} Functor()'.", nameof(method));
        }
    }

    public VariableElement<TResult> Invoke()
    {
        Target?.EmitLoadAsTarget();
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);
        var result = Context.DefineVariable<TResult>();
        result.EmitStoreValue();
        return result;
    }
}

public class FunctorMethodElement<TResult, TArg1> : MethodElement
{
    public FunctorMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(TResult) || parameters.Length != 1 ||
            parameters[0].ParameterType != typeof(TArg1))
        {
            throw new ArgumentException(
                $"Method must have signature '{typeof(TResult).Name} Functor({typeof(TArg1).Name} arg1)'.",
                nameof(method));
        }
    }

    public VariableElement<TResult> Invoke(ValueElement<TArg1> arg1)
    {
        var result = Context.DefineVariable<TResult>();

        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);

        result.EmitStoreValue();
        return result;
    }
}

public class FunctorMethodElement<TResult, TArg1, TArg2> : MethodElement
{
    public FunctorMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(TResult) || parameters.Length != 2 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2))
        {
            throw new ArgumentException(
                $"Method must have signature " +
                $"'{typeof(TResult).Name} Functor({typeof(TArg1).Name}, {typeof(TArg2).Name})'.", nameof(method));
        }
    }

    public VariableElement<TResult> Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2)
    {
        var result = Context.DefineVariable<TResult>();

        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);

        result.EmitStoreValue();
        return result;
    }
}

public class FunctorMethodElement<TResult, TArg1, TArg2, TArg3> : MethodElement
{
    public FunctorMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(TResult) || parameters.Length != 3 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3))
        {
            throw new ArgumentException(
                $"Method must have signature '{typeof(TResult).Name} Functor({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name})'.",
                nameof(method));
        }
    }

    public VariableElement<TResult> Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3)
    {
        var result = Context.DefineVariable<TResult>();

        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);

        result.EmitStoreValue();
        return result;
    }
}

public class FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4> : MethodElement
{
    public FunctorMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(TResult) || parameters.Length != 4 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3) ||
            parameters[3].ParameterType != typeof(TArg4))
        {
            throw new ArgumentException(
                $"Method must have signature '{typeof(TResult).Name} Functor({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name})'.",
                nameof(method));
        }
    }

    public VariableElement<TResult> Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4)
    {
        var result = Context.DefineVariable<TResult>();

        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);

        result.EmitStoreValue();
        return result;
    }
}

public class FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5> : MethodElement
{
    public FunctorMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(TResult) || parameters.Length != 5 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3) ||
            parameters[3].ParameterType != typeof(TArg4) ||
            parameters[4].ParameterType != typeof(TArg5))
        {
            throw new ArgumentException(
                $"Method must have signature '{typeof(TResult).Name} Functor({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name}, {typeof(TArg5).Name})'.",
                nameof(method));
        }
    }

    public VariableElement<TResult> Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4, ValueElement<TArg5> arg5)
    {
        var result = Context.DefineVariable<TResult>();

        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);

        result.EmitStoreValue();
        return result;
    }
}

public class FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> : MethodElement
{
    public FunctorMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(TResult) || parameters.Length != 6 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3) ||
            parameters[3].ParameterType != typeof(TArg4) ||
            parameters[4].ParameterType != typeof(TArg5) ||
            parameters[5].ParameterType != typeof(TArg6))
        {
            throw new ArgumentException(
                $"Method must have signature '{typeof(TResult).Name} Functor({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name}, {typeof(TArg5).Name}, {typeof(TArg6).Name})'.",
                nameof(method));
        }
    }

    public VariableElement<TResult> Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6)
    {
        var result = Context.DefineVariable<TResult>();

        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);

        result.EmitStoreValue();
        return result;
    }
}

public class FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> : MethodElement
{
    public FunctorMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(TResult) || parameters.Length != 7 ||
            parameters[0].ParameterType != typeof(TArg1) ||
            parameters[1].ParameterType != typeof(TArg2) ||
            parameters[2].ParameterType != typeof(TArg3) ||
            parameters[3].ParameterType != typeof(TArg4) ||
            parameters[4].ParameterType != typeof(TArg5) ||
            parameters[5].ParameterType != typeof(TArg6) ||
            parameters[6].ParameterType != typeof(TArg7))
        {
            throw new ArgumentException(
                $"Method must have signature '{typeof(TResult).Name} Functor({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name}, {typeof(TArg5).Name}, {typeof(TArg6).Name}, {typeof(TArg7).Name})'.",
                nameof(method));
        }
    }

    public VariableElement<TResult> Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7)
    {
        var result = Context.DefineVariable<TResult>();

        Target?.EmitLoadAsTarget();
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);
        Context.Code.Emit(Method.IsVirtual && EnableVirtualCalling ? OpCodes.Callvirt : OpCodes.Call, Method);

        result.EmitStoreValue();
        return result;
    }
}

public class FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> : MethodElement
{
    public FunctorMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(TResult) || parameters.Length != 8 ||
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
                $"Method must have signature '{typeof(TResult).Name} Functor({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name}, {typeof(TArg5).Name}, {typeof(TArg6).Name}, {typeof(TArg7).Name}, {typeof(TArg8).Name})'.",
                nameof(method));
        }
    }

    public VariableElement<TResult> Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7,
        ValueElement<TArg8> arg8)
    {
        var result = Context.DefineVariable<TResult>();

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

        result.EmitStoreValue();
        return result;
    }
}

public class FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9> : MethodElement
{
    public FunctorMethodElement(MethodContext context, ValueElement? target, MethodInfo method)
        : base(context, target, method)
    {
        var parameters = method.GetParameters();

        if (method.ReturnType != typeof(TResult) || parameters.Length != 9 ||
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
                $"Method must have signature '{typeof(TResult).Name} Functor({typeof(TArg1).Name}, {typeof(TArg2).Name}, {typeof(TArg3).Name}, {typeof(TArg4).Name}, {typeof(TArg5).Name}, {typeof(TArg6).Name}, {typeof(TArg7).Name}, {typeof(TArg8).Name}, {typeof(TArg9).Name})'.",
                nameof(method));
        }
    }

    public VariableElement<TResult> Invoke(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3,
        ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7,
        ValueElement<TArg8> arg8, ValueElement<TArg9> arg9)
    {
        var result = Context.DefineVariable<TResult>();

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

        result.EmitStoreValue();
        return result;
    }
}

public static class FunctorMethodElementExtensions
{
    public static FunctorMethodElement<TResult> GetMethod<TTarget, TResult>(this ValueElement<TTarget> target,
        Expression<Func<TTarget, TResult>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new FunctorMethodElement<TResult>(target.Context, target, call.Method);
    }

    public static FunctorMethodElement<TResult, TArg1> GetMethod<TTarget, TResult, TArg1>(
        this ValueElement<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new FunctorMethodElement<TResult, TArg1>(target.Context, target, call.Method);
    }

    public static FunctorMethodElement<TResult, TArg1, TArg2> GetMethod<TTarget, TResult, TArg1, TArg2>(
        this ValueElement<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new FunctorMethodElement<TResult, TArg1, TArg2>(target.Context, target, call.Method);
    }

    public static FunctorMethodElement<TResult, TArg1, TArg2, TArg3> GetMethod<TTarget, TResult, TArg1, TArg2, TArg3>(
        this ValueElement<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new FunctorMethodElement<TResult, TArg1, TArg2, TArg3>(target.Context, target, call.Method);
    }

    public static FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4> GetMethod<TTarget, TResult, TArg1, TArg2,
        TArg3, TArg4>(this ValueElement<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4>(target.Context, target, call.Method);
    }

    public static FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5> GetMethod<TTarget, TResult, TArg1,
        TArg2, TArg3, TArg4, TArg5>(this ValueElement<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5>(target.Context, target, call.Method);
    }

    public static FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6> GetMethod<TTarget, TResult,
        TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(this ValueElement<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(target.Context, target,
                call.Method);
    }

    public static FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7> GetMethod<TTarget,
        TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(this ValueElement<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(target.Context, target,
                call.Method);
    }

    public static FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8> GetMethod<
        TTarget, TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(this ValueElement<TTarget> target,
        Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(target.Context,
                target, call.Method);
    }

    public static FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>
        GetMethod<TTarget, TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
            this ValueElement<TTarget> target,
            Expression<Action<TTarget>> expression)
    {
        return expression.Body is not MethodCallExpression call
            ? throw new ArgumentException("Expression must be a method call.", nameof(expression))
            : new FunctorMethodElement<TResult, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(
                target.Context, target, call.Method);
    }
}