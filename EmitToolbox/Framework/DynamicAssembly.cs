using System.Runtime.CompilerServices;

namespace EmitToolbox.Framework;

public abstract class DynamicAssembly : IAttributeMarker
{
    internal DynamicAssembly(AssemblyBuilder builder)
    {
        AssemblyBuilder = builder;
        ModuleBuilder = builder.DefineDynamicModule("Main");
    }

    private readonly HashSet<string> _accessibleAssemblies = [];

    public AssemblyBuilder AssemblyBuilder { get; }

    public ModuleBuilder ModuleBuilder { get; }

    /// <summary>
    /// Names of assemblies whose internal and private members can be accessed by this assembly.
    /// </summary>
    public IReadOnlySet<string> UnrestrictedAccessAssemblies => _accessibleAssemblies;

    /// <summary>
    /// Add a custom attribute to the assembly.
    /// </summary>
    /// <param name="attributeBuilder">Attribute builder of the attribute to add.</param>
    public IAttributeMarker MarkAttribute(CustomAttributeBuilder attributeBuilder)
    {
        AssemblyBuilder.SetCustomAttribute(attributeBuilder);
        return this;
    }

    /// <summary>
    /// Allow code in this assembly to ignore access checks to the specified assembly.
    /// </summary>
    /// <param name="targetAssembly">Assembly whose access checks will be ignored.</param>
    public DynamicAssembly IgnoreVisibilityChecksToAssembly(Assembly targetAssembly)
        => IgnoreVisibilityChecksToAssembly(
            targetAssembly.GetName().Name
            ?? throw new ArgumentException(
                "Cannot skip access checks to an unnamed assembly.", nameof(targetAssembly)));

    /// <summary>
    /// Allow code in this assembly to ignore access checks to the specified assembly.
    /// </summary>
    /// <param name="targetAssembly">Assembly whose access checks will be ignored.</param>
    public DynamicAssembly IgnoreVisibilityChecksToAssembly(string targetAssembly)
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
        return new DynamicType(this, typeBuilder);
    }

    public DynamicType DefineStruct(string name, VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = visibility.ToTypeAttributes()
                         | TypeAttributes.AnsiClass
                         | TypeAttributes.BeforeFieldInit
                         | TypeAttributes.SequentialLayout;
        var typeBuilder = ModuleBuilder.DefineType(name, attributes);
        return new DynamicType(this, typeBuilder);
    }
    
    public static ExecutableDynamicAssembly DefineExecutable(
        AssemblyName name, AssemblyBuilderAccess access = AssemblyBuilderAccess.RunAndCollect) 
        => new (name, access);
    
    public static ExecutableDynamicAssembly DefineExecutable(
        string name, AssemblyBuilderAccess access = AssemblyBuilderAccess.RunAndCollect) 
        => new (new AssemblyName(name), access);
    
    public static ExportableDynamicAssembly DefineExportable(
        AssemblyName name, Assembly? runtime = null) =>
        new (name, runtime ?? typeof(object).Assembly);
    
    public static ExportableDynamicAssembly DefineExportable(
        string name, Assembly? runtime = null) =>
        new (new AssemblyName(name), runtime ?? typeof(object).Assembly);
}

public class ExecutableDynamicAssembly : DynamicAssembly
{
    internal ExecutableDynamicAssembly(AssemblyName name, AssemblyBuilderAccess access)
        : base(AssemblyBuilder.DefineDynamicAssembly(name, access))
    {
    }
}

public class ExportableDynamicAssembly : DynamicAssembly
{
    public new PersistedAssemblyBuilder AssemblyBuilder => (PersistedAssemblyBuilder)base.AssemblyBuilder;
    
    internal ExportableDynamicAssembly(AssemblyName name, Assembly runtime)
        : base(new PersistedAssemblyBuilder(name, runtime))
    {
    }

    public void Export(string fileName) => AssemblyBuilder.Save(fileName);
    
    public void Export(Stream fileStream) => AssemblyBuilder.Save(fileStream);
}