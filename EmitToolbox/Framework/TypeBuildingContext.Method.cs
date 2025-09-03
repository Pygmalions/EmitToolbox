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
        return TypeBuilder.DefineMethod(name, attributes, CallingConventions.Standard,
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
        
        var methodBuilder = TypeBuilder.DefineMethod(
            name, 
            method.Attributes & ~MethodAttributes.Abstract, // Cancel the abstract flag if present.
            CallingConventions.Standard,
            method.ReturnType, null, null,
            parameters.Select(parameter => parameter.ParameterType).ToArray(),
            parameterModifiers, null
        );
        TypeBuilder.DefineMethodOverride(methodBuilder, method);

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

    public class ActionBuilder
    {
        private readonly TypeBuildingContext _context;
        
        internal ActionBuilder(TypeBuildingContext context)
        {
            _context = context;
        }
        
        public InstanceActionBuildingContext Instance(
            string name, ParameterDefinition[] parameters, 
            VisibilityLevel visibility = VisibilityLevel.Public, MethodModifier modifier = MethodModifier.None,
            bool hasSpecialName = false)
        {
            var attributes = MethodAttributes.HideBySig |
                             _context.BuildMethodVisibility(visibility);
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
            if (hasSpecialName)
                attributes |= MethodAttributes.SpecialName;
            var methodBuilder = _context.BuildMethodBuilder(name, attributes, parameters, ResultDefinition.None);
            return new InstanceActionBuildingContext(_context, methodBuilder);
        }
        
        public StaticActionBuildingContext Static(
            string name, ParameterDefinition[] parameters,
            VisibilityLevel visibility = VisibilityLevel.Public, bool hasSpecialName = false)
        {
            var attributes = MethodAttributes.HideBySig | MethodAttributes.Static |
                             _context.BuildMethodVisibility(visibility);
            if (hasSpecialName)
                attributes |= MethodAttributes.SpecialName;
            var methodBuilder = _context.BuildMethodBuilder(name, attributes, parameters, ResultDefinition.None);
            return new StaticActionBuildingContext(_context, methodBuilder);
        }
        
        public InstanceActionBuildingContext Override(MethodInfo method, string? name = null)
        {
            return new InstanceActionBuildingContext(
                _context, _context.BuildOverridenMethodBuilder(name ?? method.Name, method));
        }
    }
    
    public class FunctorBuilder
    {
        private readonly TypeBuildingContext _context;
        
        internal FunctorBuilder(TypeBuildingContext context)
        {
            _context = context;
        }
        
        public InstanceFunctorBuildingContext Instance(
            string name, ParameterDefinition[] parameters, ResultDefinition result,
            VisibilityLevel visibility = VisibilityLevel.Public, MethodModifier modifier = MethodModifier.None,
            bool hasSpecialName = false)
        {
            var attributes = MethodAttributes.HideBySig |
                             _context.BuildMethodVisibility(visibility);
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
            if (hasSpecialName)
                attributes |= MethodAttributes.SpecialName;
            var methodBuilder = _context.BuildMethodBuilder(name, attributes, parameters, result);
            return new InstanceFunctorBuildingContext(_context, methodBuilder);
        }

        public StaticFunctorBuildingContext Static(
            string name, ParameterDefinition[] parameters, ResultDefinition result,
            VisibilityLevel visibility = VisibilityLevel.Public, bool hasSpecialName = false)
        {
            var attributes = MethodAttributes.HideBySig | MethodAttributes.Static |
                             _context.BuildMethodVisibility(visibility);
            if (hasSpecialName)
                attributes |= MethodAttributes.SpecialName;
            var methodBuilder = _context.BuildMethodBuilder(name, attributes, parameters, result);
            return new StaticFunctorBuildingContext(_context, methodBuilder);
        }
        
        public InstanceFunctorBuildingContext Override(MethodInfo method, string? name = null)
        {
            return new InstanceFunctorBuildingContext(_context,
                _context.BuildOverridenMethodBuilder(name ?? method.Name, method));
        }
    }

    public FunctorBuilder Functors { get; }
    
    public ActionBuilder Actions { get; }
    
    public ConstructorMethodBuildingContext Constructor(VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName | 
                         MethodAttributes.RTSpecialName | BuildMethodVisibility(visibility);
        var constructorBuilder = TypeBuilder.DefineDefaultConstructor(attributes);
        return new ConstructorMethodBuildingContext(this, constructorBuilder);
    }
    
    public ConstructorMethodBuildingContext Constructor(
        ParameterDefinition[] parameters, VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName | 
                         MethodAttributes.RTSpecialName | BuildMethodVisibility(visibility);
        var constructorBuilder = TypeBuilder.DefineConstructor(attributes, CallingConventions.Standard,
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
        return new ConstructorMethodBuildingContext(this, constructorBuilder);
    }
}