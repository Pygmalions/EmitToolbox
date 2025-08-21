using System.Runtime.CompilerServices;

namespace EmitToolbox.Framework;

public partial class AssemblyBuildingContext
{
    public static Builder DefineExecutable(AssemblyName name)
        => new (name, false);
    
    public static Builder DefinePersistent(AssemblyName name)
        => new (name, false);
    
    public class Builder
    {
        private readonly AssemblyName _name;

        private readonly bool _persistent;
        
        private readonly List<CustomAttributeBuilder> _attributes = [];

        private bool _disposed;
        
        internal Builder(AssemblyName name, bool persistent)
        {
            _name = name;
            _persistent = persistent;
        }
        
        /// <summary>
        /// Add a custom attribute to the assembly.
        /// </summary>
        /// <param name="attributeBuilder">Attribute builder of the attribute to add.</param>
        public Builder MarkAttribute(CustomAttributeBuilder attributeBuilder)
        {
            _attributes.Add(attributeBuilder);
            return this;
        }
        
        /// <summary>
        /// Allow code in this assembly to ignore access checks to the specified assembly.
        /// </summary>
        /// <param name="targetAssembly">Assembly whose access checks will be ignored.</param>
        public Builder IgnoreAccessToAssembly(Assembly targetAssembly)
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
            ObjectDisposedException.ThrowIf(_disposed, nameof(Builder));
            _disposed = true;

            if (_persistent)
                return new PersistentAssemblyBuildingContext(AssemblyBuilder.DefineDynamicAssembly(
                    _name, AssemblyBuilderAccess.RunAndCollect, _attributes));
            return new ExecutableAssemblyBuildingContext(new PersistedAssemblyBuilder(
                _name, typeof(object).Assembly, _attributes));
        }
    }
}