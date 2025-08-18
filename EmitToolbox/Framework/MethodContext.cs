using EmitToolbox.Framework.Elements;

namespace EmitToolbox.Framework;

public abstract partial class MethodContext(MethodBuilder builder, ILGenerator code)
{
    public ILGenerator Code { get; } = code;

    /// <summary>
    /// Reflection information about this constructing method.
    /// </summary>
    public MethodInfo BuildingMethod { get; } = builder;
    
    /// <summary>
    /// Describe a parameter for the method.
    /// </summary>
    /// <param name="position">Position of the parameter, starting from 1 as the first parameter.</param>
    /// <param name="attributes">Attributes of the parameter.</param>
    /// <param name="name">Name of the parameter.</param>
    public void DescribeParameter(int position, ParameterAttributes attributes, string? name = null)
    {
        builder.DefineParameter(position, attributes, name);
    }

    /// <summary>
    /// Get the element for the specified argument of this method.
    /// </summary>
    /// <param name="index">
    /// Index of this method, starting from 0;
    /// if this method is an instance method, then index-0 is the argument for `this`.
    /// </param>
    /// <param name="isReference">If true, this argument is a by-ref argument.</param>
    /// <typeparam name="TArgument">Type of the argument.</typeparam>
    /// <returns>Argument element.</returns>
    public ArgumentElement<TArgument> RetrieveArgument<TArgument>(int index, bool isReference = false)
    {
        return new ArgumentElement<TArgument>(this, index, isReference);
    }

    /// <summary>
    /// Define a local variable in the method context.
    /// </summary>
    /// <param name="isReference">If true, the type will be a by-ref type.</param>
    /// <typeparam name="TVariable">Type of the variable.</typeparam>
    /// <returns>Local variable element.</returns>
    public VariableElement<TVariable> DefineVariable<TVariable>(bool isReference = false)
    {
        return new LocalVariableElement<TVariable>(this, isReference);
    }

    public ArrayFacade<TElement> NewArray<TElement>(int length)
    {
        Code.Emit(OpCodes.Ldc_I4, length);
        Code.Emit(OpCodes.Newarr, typeof(TElement));
        var array = DefineVariable<TElement[]>();
        array.EmitStoreValue();
        return new ArrayFacade<TElement>(array);
    }

    public ArrayFacade<TElement> NewArray<TElement>(ValueElement<int> length)
    {
        length.EmitLoadAsValue();
        Code.Emit(OpCodes.Newarr, typeof(TElement));
        var array = DefineVariable<TElement[]>();
        array.EmitStoreValue();
        return new ArrayFacade<TElement>(array);
    }
}