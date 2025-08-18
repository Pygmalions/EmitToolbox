namespace EmitToolbox.Framework.Elements.ObjectMembers;

public class ConstructorElement<TTarget>(MethodContext context, ConstructorInfo constructor)
{
    public MethodContext Context { get; } = context;

    public ConstructorInfo Constructor { get; } = constructor;

    protected ParameterInfo[] Parameters { get; } = constructor.GetParameters();
    
    public ValueElement<TTarget> New(ValueElement[] parameters)
    {
        var result = Context.DefineVariable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        foreach (var parameter in Parameters)
            parameters[parameter.Position].EmitLoadAsParameter(parameter);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreValue();
        }

        return result;
    }

    public void New(ValueElement<TTarget> target, ValueElement[] parameters)
    {
        target.EmitLoadAsTarget();
        
        foreach (var parameter in Parameters)
            parameters[parameter.Position].EmitLoadAsParameter(parameter);
        
        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Initobj, typeof(TTarget));
        else
            Context.Code.Emit(OpCodes.Newobj, Constructor);
    }
}

public class ConstructorElement<TTarget, TArg1>(MethodContext context, ConstructorInfo constructor)
    : ConstructorElement<TTarget>(context, constructor)
{
    public ValueElement<TTarget> New(ValueElement<TArg1> arg1)
    {
        var result = Context.DefineVariable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreValue();
        }

        return result;
    }

    public void New(ValueElement<TTarget> target, ValueElement<TArg1> arg1)
    {
        target.EmitLoadAsTarget();
        
        arg1.EmitLoadAsParameter(Parameters[0]);
        
        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Initobj, typeof(TTarget));
        else
            Context.Code.Emit(OpCodes.Newobj, Constructor);
    }
}

public class ConstructorElement<TTarget, TArg1, TArg2>(MethodContext context, ConstructorInfo constructor)
    : ConstructorElement<TTarget>(context, constructor)
{
    public ValueElement<TTarget> New(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2)
    {
        var result = Context.DefineVariable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreValue();
        }

        return result;
    }

    public void New(ValueElement<TTarget> target, ValueElement<TArg1> arg1, ValueElement<TArg2> arg2)
    {
        target.EmitLoadAsTarget();
        
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        
        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Initobj, typeof(TTarget));
        else
            Context.Code.Emit(OpCodes.Newobj, Constructor);
    }
}

public class ConstructorElement<TTarget, TArg1, TArg2, TArg3>(MethodContext context, ConstructorInfo constructor)
    : ConstructorElement<TTarget>(context, constructor)
{
    public ValueElement<TTarget> New(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3)
    {
        var result = Context.DefineVariable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreValue();
        }

        return result;
    }

    public void New(ValueElement<TTarget> target, ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Initobj, typeof(TTarget));
        else
            Context.Code.Emit(OpCodes.Newobj, Constructor);
    }
}

public class ConstructorElement<TTarget, TArg1, TArg2, TArg3, TArg4>(MethodContext context, ConstructorInfo constructor)
    : ConstructorElement<TTarget>(context, constructor)
{
    public ValueElement<TTarget> New(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4)
    {
        var result = Context.DefineVariable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreValue();
        }

        return result;
    }

    public void New(ValueElement<TTarget> target, ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Initobj, typeof(TTarget));
        else
            Context.Code.Emit(OpCodes.Newobj, Constructor);
    }
}

public class ConstructorElement<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5>(MethodContext context, ConstructorInfo constructor)
    : ConstructorElement<TTarget>(context, constructor)
{
    public ValueElement<TTarget> New(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4, ValueElement<TArg5> arg5)
    {
        var result = Context.DefineVariable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreValue();
        }

        return result;
    }

    public void New(ValueElement<TTarget> target, ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4, ValueElement<TArg5> arg5)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Initobj, typeof(TTarget));
        else
            Context.Code.Emit(OpCodes.Newobj, Constructor);
    }
}

public class ConstructorElement<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(MethodContext context, ConstructorInfo constructor)
    : ConstructorElement<TTarget>(context, constructor)
{
    public ValueElement<TTarget> New(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6)
    {
        var result = Context.DefineVariable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreValue();
        }

        return result;
    }

    public void New(ValueElement<TTarget> target, ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Initobj, typeof(TTarget));
        else
            Context.Code.Emit(OpCodes.Newobj, Constructor);
    }
}

public class ConstructorElement<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(MethodContext context, ConstructorInfo constructor)
    : ConstructorElement<TTarget>(context, constructor)
{
    public ValueElement<TTarget> New(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7)
    {
        var result = Context.DefineVariable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreValue();
        }

        return result;
    }

    public void New(ValueElement<TTarget> target, ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Initobj, typeof(TTarget));
        else
            Context.Code.Emit(OpCodes.Newobj, Constructor);
    }
}

public class ConstructorElement<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(MethodContext context, ConstructorInfo constructor)
    : ConstructorElement<TTarget>(context, constructor)
{
    public ValueElement<TTarget> New(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7, ValueElement<TArg8> arg8)
    {
        var result = Context.DefineVariable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);
        arg8.EmitLoadAsParameter(Parameters[7]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreValue();
        }

        return result;
    }

    public void New(ValueElement<TTarget> target, ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7, ValueElement<TArg8> arg8)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);
        arg8.EmitLoadAsParameter(Parameters[7]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Initobj, typeof(TTarget));
        else
            Context.Code.Emit(OpCodes.Newobj, Constructor);
    }
}

public class ConstructorElement<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(MethodContext context, ConstructorInfo constructor)
    : ConstructorElement<TTarget>(context, constructor)
{
    public ValueElement<TTarget> New(ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7, ValueElement<TArg8> arg8, ValueElement<TArg9> arg9)
    {
        var result = Context.DefineVariable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);
        arg8.EmitLoadAsParameter(Parameters[7]);
        arg9.EmitLoadAsParameter(Parameters[8]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreValue();
        }

        return result;
    }

    public void New(ValueElement<TTarget> target, ValueElement<TArg1> arg1, ValueElement<TArg2> arg2, ValueElement<TArg3> arg3, ValueElement<TArg4> arg4, ValueElement<TArg5> arg5, ValueElement<TArg6> arg6, ValueElement<TArg7> arg7, ValueElement<TArg8> arg8, ValueElement<TArg9> arg9)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);
        arg8.EmitLoadAsParameter(Parameters[7]);
        arg9.EmitLoadAsParameter(Parameters[8]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Initobj, typeof(TTarget));
        else
            Context.Code.Emit(OpCodes.Newobj, Constructor);
    }
}
