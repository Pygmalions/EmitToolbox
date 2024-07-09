using System.Reflection;
using System.Reflection.Emit;
using AsyncToolbox;

namespace EmitToolbox.Framework.Contexts;

public class MethodContext
{
    public readonly MethodInfo ProxiedMethod;

    public readonly MethodBuilder Builder;

    private readonly PendingQueue _emittingInvocation = new();
    
    /// <summary>
    /// This awaitable property will return 
    /// </summary>
    public IAwaitable EmittingInvocation => _emittingInvocation;
    
    /// <summary>
    /// Variable which stores the returning value.
    /// </summary>
    public readonly LocalBuilder? Result;

    /// <summary>
    /// Whether the proxied method should be skipped or not.
    /// If its value is set to true, then the proxied method will be skipped.
    /// </summary>
    public readonly LocalBuilder Skipped;
    
    public MethodContext(ClassContext classContext, MethodInfo proxiedMethod)
    {
        ProxiedMethod = proxiedMethod;
        Builder = classContext.Builder.DefineMethod(
            $"_{proxiedMethod.Name}",
            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
            CallingConventions.Standard, proxiedMethod.ReturnType, 
            ProxiedMethod.GetParameters().Select(parameter => parameter.ParameterType).ToArray()
        );
        Builder.SetCustomAttribute(ProxyGenerator.GeneratedAttributeBuilder);
        if (proxiedMethod.IsVirtual || proxiedMethod.IsAbstract)
            classContext.Builder.DefineMethodOverride(Builder, proxiedMethod);

        var code = Builder.GetILGenerator();

        Result = proxiedMethod.ReturnType != typeof(void) ? code.DeclareLocal(proxiedMethod.ReturnType) : null;
        Skipped = code.DeclareLocal(typeof(bool));
        
        // Initialize skipping flag variable.
        code.Emit(OpCodes.Ldc_I4_0);
        code.Emit(OpCodes.Stloc, Skipped);
    }
    
    internal void Build()
    {
        var code = Builder.GetILGenerator();
        // Skip proxied method if Skipped is set to true.
        var labelInvoked = code.DefineLabel();
        code.Emit(OpCodes.Ldloc, Skipped);
        code.Emit(OpCodes.Brtrue, labelInvoked);
        
        // Load all arguments into the stack.
        for (var parameterIndex = 0; parameterIndex <= ProxiedMethod.GetParameters().Length; ++parameterIndex)
        {
            code.Emit(OpCodes.Ldarg, parameterIndex);
        }
        // Invoke the base method without looking up in the virtual function table.
        code.Emit(OpCodes.Call, ProxiedMethod);
        // Store the method returning value into a local variable.
        // The result variable will be overwritten here.
        if (Result != null)
            code.Emit(OpCodes.Stloc, Result);
        // Update skipping flag.
        code.Emit(OpCodes.Ldc_I4_0);
        code.Emit(OpCodes.Stloc, Skipped);
        
        code.MarkLabel(labelInvoked);
        
        // Apply pending modifying actions.
        _emittingInvocation.Process(actions =>
        {
            foreach (var action in actions.Reverse())
            {
                action.Execute();
            }
        });
        
        if (Result != null)
            code.Emit(OpCodes.Ldloc, Result);
        
        code.Emit(OpCodes.Ret);
    }
}