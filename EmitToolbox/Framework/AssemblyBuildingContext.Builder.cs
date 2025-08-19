using System.Runtime.CompilerServices;

namespace EmitToolbox.Framework;

public partial class AssemblyBuildingContext
{
    public class DynamicAssemblyBuilder(AssemblyName name, bool persistent)
    {
        private bool _disposed;
        
        private readonly List<CustomAttributeBuilder> _attributes = [];

        /// <summary>
        /// Add a custom attribute to the assembly.
        /// </summary>
        /// <param name="attributeBuilder">Attribute builder of the attribute to add.</param>
        public DynamicAssemblyBuilder MarkAttribute(CustomAttributeBuilder attributeBuilder)
        {
            _attributes.Add(attributeBuilder);
            return this;
        }
        
        /// <summary>
        /// Allow code in this assembly to ignore access checks to the specified assembly.
        /// </summary>
        /// <param name="targetAssembly">Assembly whose access checks will be ignored.</param>
        public DynamicAssemblyBuilder IgnoreAccessToAssembly(Assembly targetAssembly)
        {
            _attributes.Add(IgnoresAccessChecksToAttribute.Create(targetAssembly));
            return this;
        }
        
        /// <summary>
        /// Build this assembly context.
        /// </summary>
        /// <returns>Built assembly context.</returns>
        public AssemblyBuildingContext Build()
        {
            ObjectDisposedException.ThrowIf(_disposed, nameof(DynamicAssemblyBuilder));
            _disposed = true;

            if (persistent)
                return new PersistentAssemblyBuildingContext(AssemblyBuilder.DefineDynamicAssembly(
                    name, AssemblyBuilderAccess.RunAndCollect, _attributes));
            return new ExecutableAssemblyBuildingContext(new PersistedAssemblyBuilder(
                name, typeof(object).Assembly, _attributes));
        }
    }
}