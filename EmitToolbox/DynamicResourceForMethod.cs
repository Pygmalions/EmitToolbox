// using System.Runtime.CompilerServices;
// using EmitToolbox.Framework;
//
// namespace EmitToolbox;
//
// public class DynamicResourceForMethod<TResource>(
//     Func<DynamicAssembly, MethodInfo, TResource> factory,
//     string moduleNamePrefix = "Dynamic.", string moduleNamePostfix = "")
// {
//     private readonly ConditionalWeakTable<Assembly, Entry> _modules = new();
//
//     private class Entry(AssemblyName name, Assembly assembly)
//     {
//         public DynamicAssembly Assembly { get; } =
//             DynamicAssembly
//                 .DefineExecutable(name)
//                 .IgnoreVisibilityChecksToAssembly(assembly);
//
//         public Dictionary<MethodInfo, TResource> Resources { get; } = [];
//     }
//
//     public TResource this[MethodInfo method]
//     {
//         get
//         {
//             var assembly = method.DeclaringType?.Assembly
//                            ?? throw new ArgumentException(
//                                "Method to create a dynamic cache must have a declaring type.",
//                                nameof(method));
//             var entry = _modules.GetValue(assembly,
//                 targetAssembly => new Entry(
//                     new AssemblyName($"{moduleNamePrefix}" +
//                                      $"{assembly.GetName().Name 
//                                         ?? Guid.CreateVersion7().ToString("D").Replace('-', '_')}" +
//                                      $"{moduleNamePostfix}"),
//                     targetAssembly));
//             if (entry.Resources.TryGetValue(method, out var resource))
//                 return resource;
//             resource = factory(entry.Assembly, method);
//             entry.Resources[method] = resource;
//             return resource;
//         }
//     }
// }