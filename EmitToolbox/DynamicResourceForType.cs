using System.Runtime.CompilerServices;
using EmitToolbox.Framework;

namespace EmitToolbox;

public class DynamicResourceForType<TResource>(
    Func<DynamicAssembly, Type, TResource> factory,
    string moduleNamePrefix = "Dynamic.", string moduleNamePostfix = "")
{
    private readonly ConditionalWeakTable<Assembly, Entry> _modules = new();

    private class Entry(AssemblyName name, Assembly assembly)
    {
        public DynamicAssembly Assembly { get; } =
            DynamicAssembly
                .DefineExecutable(name)
                .IgnoreVisibilityChecksToAssembly(assembly);

        public Dictionary<Type, TResource> Resources { get; } = [];
    }

    public TResource this[Type type]
    {
        get
        {
            var assembly = type.Assembly;
            var entry = _modules.GetValue(assembly,
                targetAssembly => new Entry(
                    new AssemblyName($"{moduleNamePrefix}" +
                                     $"{assembly.GetName().Name
                                        ?? Guid.CreateVersion7().ToString("D").Replace('-', '_')}" +
                                     $"{moduleNamePostfix}"),
                    targetAssembly));
            if (entry.Resources.TryGetValue(type, out var resource))
                return resource;
            resource = factory(entry.Assembly, type);
            entry.Resources[type] = resource;
            return resource;
        }
    }
}