using System.Reflection;
using System.Reflection.Emit;
using EmitToolbox.Framework.Contexts;

namespace EmitToolbox.Framework;

public class ProxyGenerator
{
    internal static readonly CustomAttributeBuilder GeneratedAttributeBuilder = new(
        typeof(GeneratedAttribute).GetConstructor(Type.EmptyTypes)!, Array.Empty<object>());

    private readonly ModuleBuilder _module;

    public ProxyGenerator(string assemblyName, string moduleName = "Proxies", 
        AssemblyBuilderAccess access = AssemblyBuilderAccess.RunAndCollect)
    {
        _module = AssemblyBuilder
            .DefineDynamicAssembly(new AssemblyName(assemblyName), access)
            .DefineDynamicModule(moduleName);
        _module.SetCustomAttribute(GeneratedAttributeBuilder);
    }

    public readonly LinkedList<IProxyHandler> Handlers = new();

    /// <summary>
    /// Create a proxy class for the specific class.
    /// </summary>
    /// <param name="proxiedClass">Class to generate proxy for.</param>
    /// <param name="className">
    /// Class name. If it is null, then this name rather than the name of the proxied class will be used
    /// as the name of the proxy class.
    /// </param>
    /// <returns>Generated proxy class.</returns>
    public Type Create(Type proxiedClass, string? className = null)
    {
        var context = new ClassContext(_module, proxiedClass, className);
        foreach (var handler in Handlers)
        {
            handler.Process(context);
        }
        return context.Build();
    }
}