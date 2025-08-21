using System.Runtime.InteropServices;

namespace EmitToolbox.Framework;

public partial class TypeBuildingContext
{
    private MethodAttributes BuildMethodVisibility(VisibilityLevel visibility)
    {
        return visibility switch
        {
            VisibilityLevel.Public => MethodAttributes.Public,
            VisibilityLevel.Private => MethodAttributes.Private,
            VisibilityLevel.Protected => MethodAttributes.Family,
            VisibilityLevel.Internal => MethodAttributes.Assembly,
            VisibilityLevel.ProtectedInternal => MethodAttributes.FamORAssem,
            _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
        };
    }
    
    private MethodBuilder BuildMethodBuilder(
        string name, MethodAttributes attributes,
        ParameterDefinition[] parameters,
        ResultDefinition result)
    {
        return _typeBuilder.DefineMethod(name, attributes, CallingConventions.Standard,
            result.Type,
            null, result.Attributes?.ToArray(),
            parameters.Select(parameter =>
                    parameter.Modifier == ParameterModifier.None
                        ? parameter.Type
                        : parameter.Type.MakeByRefType())
                .ToArray(),
            parameters.Select(parameter => parameter.Modifier switch
            {
                ParameterModifier.None or ParameterModifier.Ref => Type.EmptyTypes,
                ParameterModifier.In => [typeof(InAttribute)],
                ParameterModifier.Out => [typeof(OutAttribute)],
                _ => throw new Exception(
                    $"Unsupported parameter modifier '{parameter.Modifier}' on parameter {parameter.Name}.")
            }).ToArray(),
            parameters.Select(parameter => parameter.Attributes ?? Type.EmptyTypes).ToArray());
    }

    private MethodBuilder BuildOverridenMethodBuilder(string name, MethodInfo method)
    {
        var parameters = method.GetParameters();

        Type[][]? parameterModifiers = null;

        foreach (var (index, parameter) in parameters.Index())
        {
            if (parameter.IsIn)
            {
                parameterModifiers = InitializeParameterModifiers();
                parameterModifiers[index] = [typeof(InAttribute)];
            }
            else if (parameter.IsOut)
            {
                parameterModifiers = InitializeParameterModifiers();
                parameterModifiers[index] = [typeof(OutAttribute)];
            }
        }
        
        var methodBuilder = _typeBuilder.DefineMethod(
            name, method.Attributes, CallingConventions.Standard,
            method.ReturnType, null, null,
            parameters.Select(parameter => parameter.ParameterType).ToArray(),
            parameterModifiers, null
        );
        _typeBuilder.DefineMethodOverride(methodBuilder, method);

        return methodBuilder;

        Type[][] InitializeParameterModifiers()
        {
            var modifiers = new Type[parameters.Length][];
            for (var index = 0; index < parameters.Length; index++)
            {
                modifiers[index] = Type.EmptyTypes;
            }
            return modifiers;
        }
    }
    
    public ActionMethodBuildingContext DefineStaticAction(
        string name,
        ParameterDefinition[] parameters,
        VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.Static | 
                         BuildMethodVisibility(visibility);
        var methodBuilder = BuildMethodBuilder(name, attributes, parameters, ResultDefinition.None);
        return new ActionMethodBuildingContext(methodBuilder);
    }
    
    public ActionMethodBuildingContext DefineInstanceAction(
        string name,
        ParameterDefinition[] parameters,
        VisibilityLevel visibility = VisibilityLevel.Public,
        MethodModifier modifier = MethodModifier.None)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.Static |
                         BuildMethodVisibility(visibility);
        switch (modifier)
        {
            case MethodModifier.None:
                break;
            case MethodModifier.Virtual:
                attributes |= MethodAttributes.Virtual;
                break;
            case MethodModifier.Abstract:
                attributes |= MethodAttributes.Abstract;
                break;
            case MethodModifier.New:
                attributes |= MethodAttributes.NewSlot;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null);
        }
        var methodBuilder = BuildMethodBuilder(name, attributes, parameters, ResultDefinition.None);
        return new ActionMethodBuildingContext(methodBuilder);
    }
    
    public FunctorMethodBuildingContext DefineStaticFunctor(
        string name,
        ParameterDefinition[] parameters,
        ResultDefinition result,
        VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.Static | 
                         BuildMethodVisibility(visibility);
        var methodBuilder = BuildMethodBuilder(name, attributes, parameters, result);
        return new FunctorMethodBuildingContext(methodBuilder);
    }
    
    public FunctorMethodBuildingContext DefineInstanceFunctor(
        string name,
        ParameterDefinition[] parameters,
        ResultDefinition result,
        VisibilityLevel visibility = VisibilityLevel.Public,
        MethodModifier modifier = MethodModifier.None)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.Static |
                         BuildMethodVisibility(visibility);
        switch (modifier)
        {
            case MethodModifier.None:
                break;
            case MethodModifier.Virtual:
                attributes |= MethodAttributes.Virtual;
                break;
            case MethodModifier.Abstract:
                attributes |= MethodAttributes.Abstract;
                break;
            case MethodModifier.New:
                attributes |= MethodAttributes.NewSlot;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(modifier), modifier, null);
        }
        var methodBuilder = BuildMethodBuilder(name, attributes, parameters, result);
        return new FunctorMethodBuildingContext(methodBuilder);
    }

    public ActionMethodBuildingContext OverrideAction(string name, MethodInfo method)
    {
        return new ActionMethodBuildingContext(BuildOverridenMethodBuilder(name, method));
    }
    
    public FunctorMethodBuildingContext OverrideFunctor(string name, MethodInfo method)
    {
        return new FunctorMethodBuildingContext(BuildOverridenMethodBuilder(name, method));
    }
    
    public ConstructorMethodBuildingContext DefineConstructor(VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName | 
                         MethodAttributes.RTSpecialName | BuildMethodVisibility(visibility);
        var constructorBuilder = _typeBuilder.DefineDefaultConstructor(attributes);
        return new ConstructorMethodBuildingContext(constructorBuilder);
    }
    
    public ConstructorMethodBuildingContext DefineConstructor(
        ParameterDefinition[] parameters,
        VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName | 
                         MethodAttributes.RTSpecialName | BuildMethodVisibility(visibility);
        var constructorBuilder = _typeBuilder.DefineConstructor(attributes, CallingConventions.Standard,
            parameters.Select(parameter => parameter.Type).ToArray(),
            parameters.Select(parameter => parameter.Modifier switch
            {
                ParameterModifier.None or ParameterModifier.Ref => Type.EmptyTypes,
                ParameterModifier.In => [typeof(InAttribute)],
                ParameterModifier.Out => [typeof(OutAttribute)],
                _ => throw new Exception(
                    $"Unsupported parameter modifier '{parameter.Modifier}' on parameter {parameter.Name}.")
            }).ToArray(),
            parameters.Select(parameter => parameter.Attributes ?? Type.EmptyTypes).ToArray());
        return new ConstructorMethodBuildingContext(constructorBuilder);
    }
}