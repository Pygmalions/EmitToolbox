using System.Runtime.CompilerServices;


// ReSharper disable once CheckNamespace - This attribute only works in this namespace.
namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Apply this attribute to a dynamic assembly builder to
    /// bypass visibility checks for the specified assembly;
    /// so that this dynamic assembly can access the private and internal members of the specified assembly.
    /// </summary>
    /// <param name="assemblyName">name of the assembly to skip visibility check.</param>
    /// <code>
    /// var attribute = new CustomAttributeBuilder
    /// (
    ///    IgnoresAccessChecksToAttribute.Constructor,
    ///    new object[] { typeof(TARGET_TYPE).Assembly.GetName().Name }
    /// );
    /// assemblyBuilder.SetCustomAttribute(ignoresAccessChecksTo);
    /// </code>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class IgnoresAccessChecksToAttribute(string assemblyName) : Attribute
    {
        public string AssemblyName { get; } = assemblyName;

        /// <summary>
        /// Create an attribute builder to skip the visibility check of the specified assembly.
        /// </summary>
        /// <param name="assembly">Assembly whose visibility check should be skipped.</param>
        /// <returns>Attribute builder for skipping the visibility check of specified assembly.</returns>
        public static CustomAttributeBuilder Create(Assembly assembly)
            => new(typeof(IgnoresAccessChecksToAttribute).GetConstructor([typeof(string)])!,
                [assembly.GetName().Name]);
        
        /// <summary>
        /// Create an attribute builder to skip the visibility check of the specified assembly.
        /// </summary>
        /// <param name="assembly">Name of the assembly whose visibility check should be skipped.</param>
        /// <returns>Attribute builder for skipping the visibility check of specified assembly.</returns>
        public static CustomAttributeBuilder Create(string assembly)
            => new(typeof(IgnoresAccessChecksToAttribute).GetConstructor([typeof(string)])!,
                [assembly]);
    }
}

namespace EmitToolbox
{
    public static class IgnoreAccessCheckToAttributeExtensions
    {
        public static void IgnoreAccessChecksTo(this AssemblyBuilder assembly, Assembly targetAssembly)
        {
            assembly.SetCustomAttribute(IgnoresAccessChecksToAttribute.Create(targetAssembly));
        }
        
        public static void IgnoreAccessChecksTo(this AssemblyBuilder assembly, string targetAssembly)
        {
            assembly.SetCustomAttribute(IgnoresAccessChecksToAttribute.Create(targetAssembly));
        }
    }
}

