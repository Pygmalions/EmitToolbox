using System.Runtime.CompilerServices;

namespace EmitToolbox.Framework;

public abstract class DynamicAssembly(AssemblyBuilder assemblyBuilder)
{
    private readonly HashSet<string> _accessibleAssemblies = [];

    public AssemblyBuilder AssemblyBuilder { get; } = assemblyBuilder;

    public ModuleBuilder ModuleBuilder { get; } = assemblyBuilder.DefineDynamicModule("Manifest");

    /// <summary>
    /// Names of assemblies whose internal and private members can be accessed by this assembly.
    /// </summary>
    public IReadOnlySet<string> UnrestrictedAccessAssemblies => _accessibleAssemblies;

    /// <summary>
    /// Add a custom attribute to the assembly.
    /// </summary>
    /// <param name="attributeBuilder">Attribute builder of the attribute to add.</param>
    public DynamicAssembly MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        AssemblyBuilder.SetCustomAttribute(attributeBuilder);
        return this;
    }

    /// <summary>
    /// Mark this assembly as a companion to the specified assembly.
    /// This assembly will ignore access checks to the specified assembly and its companion assemblies.
    /// </summary>
    /// <param name="targetAssembly">Target assembly to mark this assembly as a companion.</param>
    public DynamicAssembly MarkCompanionToAssembly(Assembly targetAssembly)
    {
        AssemblyBuilder.SetCustomAttribute(GeneratedCompanionAssemblyAttribute.Create(targetAssembly));

        IgnoreAccessChecksToAssembly(targetAssembly);
        // Allow the dynamic assembly to access the private and internal members of the specified assembly.
        foreach (var attribute in
                 targetAssembly.GetCustomAttributes<GeneratedCompanionAssemblyAttribute>())
            IgnoreAccessChecksToAssembly(attribute.AssemblyName);
        return this;
    }

    /// <summary>
    /// Allow code in this assembly to ignore access checks to the specified assembly.
    /// </summary>
    /// <param name="targetAssembly">Assembly whose access checks will be ignored.</param>
    public DynamicAssembly IgnoreAccessChecksToAssembly(Assembly targetAssembly)
        => IgnoreAccessChecksToAssembly(
            targetAssembly.GetName().Name
            ?? throw new ArgumentException(
                "Cannot skip access checks to an unnamed assembly.", nameof(targetAssembly)));

    /// <summary>
    /// Allow code in this assembly to ignore access checks to the specified assembly.
    /// </summary>
    /// <param name="targetAssembly">Assembly whose access checks will be ignored.</param>
    public DynamicAssembly IgnoreAccessChecksToAssembly(string targetAssembly)
    {
        if (_accessibleAssemblies.Add(targetAssembly))
            AssemblyBuilder.SetCustomAttribute(IgnoresAccessChecksToAttribute.Create(targetAssembly));
        return this;
    }
    
    public DynamicType DefineClass(string name,
        VisibilityLevel visibility = VisibilityLevel.Public,
        Type? parent = null,
        ClassModifier modifier = ClassModifier.None)
    {
        var attributes = visibility.ToTypeAttributes()
                         | TypeAttributes.AnsiClass
                         | TypeAttributes.AutoLayout
                         | TypeAttributes.BeforeFieldInit;
        switch (modifier)
        {
            case ClassModifier.None:
                break;
            case ClassModifier.Static:
                attributes |= TypeAttributes.Sealed | TypeAttributes.Abstract;
                break;
            case ClassModifier.Abstract:
                attributes |= TypeAttributes.Abstract;
                break;
            case ClassModifier.Sealed:
                attributes |= TypeAttributes.Sealed;
                break;
            case ClassModifier.Interface:
                attributes |= TypeAttributes.Interface | TypeAttributes.Abstract;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null);
        }

        var typeBuilder = ModuleBuilder.DefineType(name, attributes, parent);
        return new DynamicType(typeBuilder);
    }

    public DynamicType DefineStruct(string name, VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = visibility.ToTypeAttributes()
                         | TypeAttributes.AnsiClass
                         | TypeAttributes.BeforeFieldInit
                         | TypeAttributes.SequentialLayout;
        var typeBuilder = ModuleBuilder.DefineType(name, attributes);
        return new DynamicType(typeBuilder);
    }
    
    public static ExecutableDynamicAssembly
        DefineExecutable(string assemblyName) => new(assemblyName);
    
    public static ExecutableDynamicAssembly
        DefineExecutable(AssemblyName assemblyName) => new(assemblyName);
    
    public static PersistentDynamicAssembly
        DefinePersistent(string assemblyName) => new(new AssemblyName(assemblyName));
    
    public static PersistentDynamicAssembly
        DefinePersistent(AssemblyName assemblyName) => new(assemblyName);
}

public class ExecutableDynamicAssembly : DynamicAssembly
{
    internal ExecutableDynamicAssembly(AssemblyName assemblyName)
        : base(AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndCollect))
    {
    }
    
    internal ExecutableDynamicAssembly(string assemblyName)
        : this(new AssemblyName(assemblyName))
    {
    }
}

public class PersistentDynamicAssembly : DynamicAssembly
{
    internal PersistentDynamicAssembly(AssemblyName assemblyName)
        : base(new PersistedAssemblyBuilder(assemblyName, typeof(object).Assembly))
    {
    }
    
    public new PersistedAssemblyBuilder AssemblyBuilder => (PersistedAssemblyBuilder)base.AssemblyBuilder;

    public void Save(string fileName) => AssemblyBuilder.Save(fileName);

    public void Save(Stream stream) => AssemblyBuilder.Save(stream);
}