using System.Runtime.CompilerServices;
using EmitToolbox.Framework;

namespace EmitToolbox;

public class DynamicMethodCache<TResource>(
    Func<AssemblyBuildingContext, MethodInfo, TResource> factory,
    string moduleNamePrefix = "DynamicModule_", string moduleNamePostfix = "")
{
    private readonly ConditionalWeakTable<Assembly, Entry> _modules = new();

    private class Entry(AssemblyName name, Assembly assembly)
    {
        public AssemblyBuildingContext Context { get; } =
            AssemblyBuildingContext.DefineExecutable(name)
                .MarkCompanionToAssembly(assembly);

        public Dictionary<MethodInfo, TResource> Resources { get; } = [];
    }

    public TResource this[MethodInfo method]
    {
        get
        {
            var assembly = method.DeclaringType?.Assembly
                           ?? throw new ArgumentException(
                               "Method to create a dynamic cache must have a declaring type.",
                               nameof(method));
            var entry = _modules.GetValue(assembly,
                targetAssembly => new Entry(
                    new AssemblyName($"{moduleNamePrefix}{assembly.GetName().Name}{moduleNamePostfix}"),
                    targetAssembly));
            if (entry.Resources.TryGetValue(method, out var resource))
                return resource;
            resource = factory(entry.Context, method);
            entry.Resources[method] = resource;
            return resource;
        }
    }
}