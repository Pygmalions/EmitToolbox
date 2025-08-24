namespace EmitToolbox.Framework;

public abstract partial class AssemblyBuildingContext
{
    protected abstract ModuleBuilder ModuleBuilder { get; }

    private TypeAttributes BuildTypeVisibility(VisibilityLevel visibility)
    {
        return visibility switch
        {
            VisibilityLevel.Public => TypeAttributes.Public,
            VisibilityLevel.Internal => TypeAttributes.NotPublic | TypeAttributes.NestedAssembly,
            VisibilityLevel.Protected => TypeAttributes.NestedFamily,
            VisibilityLevel.Private => TypeAttributes.NestedPrivate,
            VisibilityLevel.ProtectedInternal => TypeAttributes.NestedFamORAssem,
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        };
    }
    
    public TypeBuildingContext DefineClass(string name,
        VisibilityLevel visibility = VisibilityLevel.Public,
        Type? parent = null, 
        ClassModifier modifier = ClassModifier.None)
    {
        var attributes = BuildTypeVisibility(visibility) 
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
        return new TypeBuildingContext(typeBuilder);
    }
    
    public TypeBuildingContext DefineStruct(string name,
        VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = BuildTypeVisibility(visibility)
                         | TypeAttributes.AnsiClass
                         | TypeAttributes.BeforeFieldInit
                         | TypeAttributes.SequentialLayout;
        var typeBuilder = ModuleBuilder.DefineType(name, attributes);
        return new TypeBuildingContext(typeBuilder);
    }
}

public class ExecutableAssemblyBuildingContext(AssemblyBuilder assembly) : AssemblyBuildingContext
{
    protected override ModuleBuilder ModuleBuilder { get; } = assembly.DefineDynamicModule("Manifest");
}

public class PersistentAssemblyBuildingContext(PersistedAssemblyBuilder assembly): AssemblyBuildingContext
{
    protected override ModuleBuilder ModuleBuilder { get; } = assembly.DefineDynamicModule("Manifest");
    
    public void Save(string fileName) => assembly.Save(fileName);

    public void Save(Stream stream) => assembly.Save(stream);
}
