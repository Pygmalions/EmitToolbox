using System.Runtime.CompilerServices;

namespace EmitToolbox.Framework;

public partial class AssemblyBuildingContext
{
    public static ExecutableContextBuilder CreateExecutableContextBuilder(AssemblyName name)
        => new(name);

    public static ExecutableContextBuilder CreateExecutableContextBuilder(string name)
        => new(new AssemblyName(name));

    public static PersistentContextBuilder CreatePersistentContextBuilder(AssemblyName name)
        => new(name);

    public static PersistentContextBuilder CreatePersistentContextBuilder(string name)
        => new(new AssemblyName(name));

    public abstract class Builder<TBuilder> where TBuilder : Builder<TBuilder>
    {
        protected readonly AssemblyName Name;

        protected readonly List<CustomAttributeBuilder> Attributes = [];

        protected bool Disposed;

        internal Builder(AssemblyName name)
        {
            Name = name;
        }

        /// <summary>
        /// Add a custom attribute to the assembly.
        /// </summary>
        /// <param name="attributeBuilder">Attribute builder of the attribute to add.</param>
        public TBuilder MarkAttribute(CustomAttributeBuilder attributeBuilder)
        {
            ObjectDisposedException.ThrowIf(Disposed, typeof(TBuilder).Name);
            Attributes.Add(attributeBuilder);
            return (TBuilder)this;
        }

        /// <summary>
        /// Allow code in this assembly to ignore access checks to the specified assembly.
        /// </summary>
        /// <param name="targetAssembly">Assembly whose access checks will be ignored.</param>
        public TBuilder IgnoreAccessToAssembly(Assembly targetAssembly)
        {
            ObjectDisposedException.ThrowIf(Disposed, typeof(TBuilder).Name);
            Attributes.Add(IgnoresAccessChecksToAttribute.Create(targetAssembly));
            return (TBuilder)this;
        }
    }

    public class ExecutableContextBuilder : Builder<ExecutableContextBuilder>
    {
        internal ExecutableContextBuilder(AssemblyName name) : base(name)
        {
        }

        public ExecutableAssemblyBuildingContext Build()
        {
            return new ExecutableAssemblyBuildingContext(
                AssemblyBuilder.DefineDynamicAssembly(
                    Name, AssemblyBuilderAccess.RunAndCollect, Attributes));
        }
    }

    public class PersistentContextBuilder : Builder<PersistentContextBuilder>
    {
        internal PersistentContextBuilder(AssemblyName name) : base(name)
        {
        }

        public PersistentAssemblyBuildingContext Build()
        {
            ObjectDisposedException.ThrowIf(Disposed, nameof(PersistentContextBuilder));
            Disposed = true;
            return new PersistentAssemblyBuildingContext(
                new PersistedAssemblyBuilder(
                    Name, typeof(object).Assembly, Attributes));
        }
    }
}