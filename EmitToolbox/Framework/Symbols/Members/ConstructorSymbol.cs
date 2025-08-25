namespace EmitToolbox.Framework.Symbols.Members;

public class ConstructorSymbol<TTarget>(MethodBuildingContext context, ConstructorInfo constructor)
{
    public MethodBuildingContext Context { get; } = context;

    public ConstructorInfo Constructor { get; } = constructor;

    protected ParameterInfo[] Parameters { get; } = constructor.GetParameters();
    
    public ValueSymbol<TTarget> New(ValueSymbol[] parameters)
    {
        var result = Context.Variable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        foreach (var parameter in Parameters)
            parameters[parameter.Position].EmitLoadAsParameter(parameter);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreFromValue();
        }

        return result;
    }

    public void New(ValueSymbol<TTarget> target, ValueSymbol[] parameters)
    {
        target.EmitLoadAsTarget();
        
        foreach (var parameter in Parameters)
            parameters[parameter.Position].EmitLoadAsParameter(parameter);
        
        Context.Code.Emit(OpCodes.Call, Constructor);
    }
}

public class ConstructorSymbol<TTarget, TArg1>(MethodBuildingContext context, ConstructorInfo constructor)
    : ConstructorSymbol<TTarget>(context, constructor)
{
    public ValueSymbol<TTarget> New(ValueSymbol<TArg1> arg1)
    {
        var result = Context.Variable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreFromValue();
        }

        return result;
    }

    public void New(ValueSymbol<TTarget> target, ValueSymbol<TArg1> arg1)
    {
        target.EmitLoadAsTarget();
        
        arg1.EmitLoadAsParameter(Parameters[0]);
        
        Context.Code.Emit(OpCodes.Call, Constructor);
    }
}

public class ConstructorSymbol<TTarget, TArg1, TArg2>(MethodBuildingContext context, ConstructorInfo constructor)
    : ConstructorSymbol<TTarget>(context, constructor)
{
    public ValueSymbol<TTarget> New(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2)
    {
        var result = Context.Variable<TTarget>();

        if (typeof(TTarget).IsValueType)
            result.EmitLoadAsAddress();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);

        if (typeof(TTarget).IsValueType)
            Context.Code.Emit(OpCodes.Call, Constructor);
        else
        {
            Context.Code.Emit(OpCodes.Newobj, Constructor);
            result.EmitStoreFromValue();
        }

        return result;
    }

    public void New(ValueSymbol<TTarget> target, ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2)
    {
        target.EmitLoadAsTarget();
        
        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        
        Context.Code.Emit(OpCodes.Call, Constructor);
    }
}

public class ConstructorSymbol<TTarget, TArg1, TArg2, TArg3>(MethodBuildingContext context, ConstructorInfo constructor)
    : ConstructorSymbol<TTarget>(context, constructor)
{
    public ValueSymbol<TTarget> New(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3)
    {
        var result = Context.Variable<TTarget>();

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
            result.EmitStoreFromValue();
        }

        return result;
    }

    public void New(ValueSymbol<TTarget> target, ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);

        Context.Code.Emit(OpCodes.Call, Constructor);
    }
}

public class ConstructorSymbol<TTarget, TArg1, TArg2, TArg3, TArg4>(MethodBuildingContext context, ConstructorInfo constructor)
    : ConstructorSymbol<TTarget>(context, constructor)
{
    public ValueSymbol<TTarget> New(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4)
    {
        var result = Context.Variable<TTarget>();

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
            result.EmitStoreFromValue();
        }

        return result;
    }

    public void New(ValueSymbol<TTarget> target, ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);

        Context.Code.Emit(OpCodes.Call, Constructor);
    }
}

public class ConstructorSymbol<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5>(MethodBuildingContext context, ConstructorInfo constructor)
    : ConstructorSymbol<TTarget>(context, constructor)
{
    public ValueSymbol<TTarget> New(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5)
    {
        var result = Context.Variable<TTarget>();

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
            result.EmitStoreFromValue();
        }

        return result;
    }

    public void New(ValueSymbol<TTarget> target, ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);

        Context.Code.Emit(OpCodes.Call, Constructor);
    }
}

public class ConstructorSymbol<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6>(MethodBuildingContext context, ConstructorInfo constructor)
    : ConstructorSymbol<TTarget>(context, constructor)
{
    public ValueSymbol<TTarget> New(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6)
    {
        var result = Context.Variable<TTarget>();

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
            result.EmitStoreFromValue();
        }

        return result;
    }

    public void New(ValueSymbol<TTarget> target, ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);

        Context.Code.Emit(OpCodes.Call, Constructor);
    }
}

public class ConstructorSymbol<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7>(MethodBuildingContext context, ConstructorInfo constructor)
    : ConstructorSymbol<TTarget>(context, constructor)
{
    public ValueSymbol<TTarget> New(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6, ValueSymbol<TArg7> arg7)
    {
        var result = Context.Variable<TTarget>();

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
            result.EmitStoreFromValue();
        }

        return result;
    }

    public void New(ValueSymbol<TTarget> target, ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6, ValueSymbol<TArg7> arg7)
    {
        target.EmitLoadAsTarget();

        arg1.EmitLoadAsParameter(Parameters[0]);
        arg2.EmitLoadAsParameter(Parameters[1]);
        arg3.EmitLoadAsParameter(Parameters[2]);
        arg4.EmitLoadAsParameter(Parameters[3]);
        arg5.EmitLoadAsParameter(Parameters[4]);
        arg6.EmitLoadAsParameter(Parameters[5]);
        arg7.EmitLoadAsParameter(Parameters[6]);

        Context.Code.Emit(OpCodes.Call, Constructor);
    }
}

public class ConstructorSymbol<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8>(MethodBuildingContext context, ConstructorInfo constructor)
    : ConstructorSymbol<TTarget>(context, constructor)
{
    public ValueSymbol<TTarget> New(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6, ValueSymbol<TArg7> arg7, ValueSymbol<TArg8> arg8)
    {
        var result = Context.Variable<TTarget>();

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
            result.EmitStoreFromValue();
        }

        return result;
    }

    public void New(ValueSymbol<TTarget> target, ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6, ValueSymbol<TArg7> arg7, ValueSymbol<TArg8> arg8)
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

        Context.Code.Emit(OpCodes.Call, Constructor);
    }
}

public class ConstructorSymbol<TTarget, TArg1, TArg2, TArg3, TArg4, TArg5, TArg6, TArg7, TArg8, TArg9>(MethodBuildingContext context, ConstructorInfo constructor)
    : ConstructorSymbol<TTarget>(context, constructor)
{
    public ValueSymbol<TTarget> New(ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6, ValueSymbol<TArg7> arg7, ValueSymbol<TArg8> arg8, ValueSymbol<TArg9> arg9)
    {
        var result = Context.Variable<TTarget>();

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
            result.EmitStoreFromValue();
        }

        return result;
    }

    public void New(ValueSymbol<TTarget> target, ValueSymbol<TArg1> arg1, ValueSymbol<TArg2> arg2, ValueSymbol<TArg3> arg3, ValueSymbol<TArg4> arg4, ValueSymbol<TArg5> arg5, ValueSymbol<TArg6> arg6, ValueSymbol<TArg7> arg7, ValueSymbol<TArg8> arg8, ValueSymbol<TArg9> arg9)
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

        Context.Code.Emit(OpCodes.Call, Constructor);
    }
}
