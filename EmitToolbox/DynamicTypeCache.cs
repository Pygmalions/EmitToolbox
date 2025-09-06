using System.Runtime.CompilerServices;
using EmitToolbox.Framework;

namespace EmitToolbox;

public class DynamicTypeCache<TResource>(
    Func<AssemblyBuildingContext, Type, TResource> factory,
    string moduleNamePrefix = "DynamicModule_", string moduleNamePostfix = "")
{
    private readonly ConditionalWeakTable<Assembly, Entry> _modules = new();

    private class Entry(AssemblyName name, Assembly assembly)
    {
        public AssemblyBuildingContext Module { get; } =
            AssemblyBuildingContext.DefineExecutable(name)
                .MarkCompanionToAssembly(assembly);

        public Dictionary<Type, TResource> Resources { get; } = [];
    }

    public TResource this[Type type]
    {
        get
        {
            var assembly = type.Assembly;
            var entry = _modules.GetValue(assembly,
                targetAssembly => new Entry(
                    new AssemblyName($"{moduleNamePrefix}{assembly.GetName().Name}{moduleNamePostfix}"),
                    targetAssembly));
            if (entry.Resources.TryGetValue(type, out var resource))
                return resource;
            resource = factory(entry.Module, type);
            entry.Resources[type] = resource;
            return resource;
        }
    }
}