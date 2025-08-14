using System.Reflection;
using System.Reflection.Emit;

namespace EmitToolbox;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class GeneratedCompanionAssemblyAttribute(string name) : Attribute
{
    /// <summary>
    /// Name of the companion assembly that this assembly is generated for.
    /// </summary>
    public string AssemblyName { get; } = name;
    
    public static CustomAttributeBuilder Create(Assembly assembly)
        => new(typeof(GeneratedCompanionAssemblyAttribute).GetConstructor([typeof(string)])!,
            [assembly.GetName().Name]);
}

public static class GeneratedCompanionAssemblyAttributeExtensions
{
    public static void MarkAsGeneratedCompanion(this AssemblyBuilder assembly, Assembly targetAssembly)
    {
        assembly.SetCustomAttribute(GeneratedCompanionAssemblyAttribute.Create(targetAssembly));
    }
}
