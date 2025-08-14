using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace EmitToolbox;

public class DynamicTypeCache<TResource>(
    Func<ModuleBuilder, Type, TResource> factory,
    AssemblyBuilderAccess access = AssemblyBuilderAccess.RunAndCollect,
    string moduleNamePrefix = "DynamicModule_",
    string moduleNamePostfix = "")
{
    private readonly ConditionalWeakTable<Assembly, Entry> _modules = new();

    private class Entry
    {
        public ModuleBuilder Module { get; }

        public Dictionary<Type, TResource> Resources { get; } = [];

        public Entry(AssemblyBuilderAccess access, AssemblyName name, Assembly assembly)
        {
            var dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(name, access);
            dynamicAssembly.MarkAsGeneratedCompanion(assembly);
            // Allow the dynamic assembly to access the private and internal members of the specified assembly.
            dynamicAssembly.IgnoreAccessChecksTo(assembly);
            foreach (var attribute in assembly.GetCustomAttributes<GeneratedCompanionAssemblyAttribute>())
                dynamicAssembly.IgnoreAccessChecksTo(attribute.AssemblyName);
            
            Module = dynamicAssembly.DefineDynamicModule("Manifest");
        }
    }

    public TResource this[Type type]
    {
        get
        {
            var assembly = type.Assembly;
            var entry = _modules.GetValue(assembly,
                targetAssembly => new Entry(access,
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