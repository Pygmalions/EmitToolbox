namespace EmitToolbox.Framework;

public abstract partial class AssemblyBuildingContext
{
    internal abstract ModuleBuilder ModuleBuilder { get; }
    
    public static DynamicAssemblyBuilder DefineExecutable(AssemblyName name)
    {
        return new DynamicAssemblyBuilder(name, false);
    }
    
    public static DynamicAssemblyBuilder DefinePersistent(AssemblyName name)
    {
        return new DynamicAssemblyBuilder(name, true);
    }
}

public class PersistentAssemblyBuildingContext(AssemblyBuilder assembly) : AssemblyBuildingContext
{
    internal override ModuleBuilder ModuleBuilder { get; } = assembly.DefineDynamicModule("Manifest");
}

public class ExecutableAssemblyBuildingContext(PersistedAssemblyBuilder assembly): AssemblyBuildingContext
{
    internal override ModuleBuilder ModuleBuilder { get; } = assembly.DefineDynamicModule("Manifest");
    
    public void Save(string fileName) => assembly.Save(fileName);

    public void Save(Stream stream) => assembly.Save(stream);
}