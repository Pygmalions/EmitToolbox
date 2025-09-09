using System.Runtime.InteropServices;

namespace EmitToolbox.Framework;

public partial class DynamicType
{
    private MethodBuilder BuildMethodBuilder(
        string name, MethodAttributes attributes,
        ParameterDefinition[] parameters,
        ResultDefinition result)
    {
        return TypeBuilder.DefineMethod(name, attributes, CallingConventions.Standard,
            result.Type,
            null, result.Attributes?.ToArray(),
            parameters.Select(parameter => parameter.Type).ToArray(),
            parameters.Select(parameter => parameter.Modifier.ToCustomAttributes()).ToArray(),
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

    public class DynamicActionBuilder
    {
        private readonly DynamicType _context;

        internal DynamicActionBuilder(DynamicType context)
        {
            _context = context;
        }

        public InstanceDynamicAction DefineInstance(
            string name, ParameterDefinition[] parameters,
            VisibilityLevel visibility = VisibilityLevel.Public, 
            MethodModifier modifier = MethodModifier.None,
            bool hasSpecialName = false)
        {
            var attributes = visibility.ToMethodAttributes() | modifier.ToMethodAttributes(hasSpecialName);
            var methodBuilder = _context.BuildMethodBuilder(name, attributes, parameters, ResultDefinition.None);
            return new InstanceDynamicAction(_context, methodBuilder);
        }

        public StaticDynamicAction DefineStatic(
            string name, ParameterDefinition[] parameters,
            VisibilityLevel visibility = VisibilityLevel.Public, bool hasSpecialName = false)
        {
            var attributes = MethodAttributes.HideBySig | MethodAttributes.Static |
                             visibility.ToMethodAttributes();
            if (hasSpecialName)
                attributes |= MethodAttributes.SpecialName;
            var methodBuilder = _context.BuildMethodBuilder(name, attributes, parameters, ResultDefinition.None);
            return new StaticDynamicAction(_context, methodBuilder);
        }

        public InstanceDynamicAction Override(MethodInfo method, string? name = null)
        {
            return new InstanceDynamicAction(
                _context, _context.BuildOverridenMethodBuilder(name ?? method.Name, method));
        }
    }

    public class DynamicFunctorBuilder
    {
        private readonly DynamicType _context;

        internal DynamicFunctorBuilder(DynamicType context)
        {
            _context = context;
        }

        public InstanceDynamicFunctor DefineInstance(
            string name, ParameterDefinition[] parameters, ResultDefinition result,
            VisibilityLevel visibility = VisibilityLevel.Public, 
            MethodModifier modifier = MethodModifier.None,
            bool hasSpecialName = false)
        {
            var attributes = visibility.ToMethodAttributes() | modifier.ToMethodAttributes(hasSpecialName);
            var methodBuilder = _context.BuildMethodBuilder(name, attributes, parameters, result);
            return new InstanceDynamicFunctor(_context, methodBuilder);
        }

        public StaticDynamicFunctor DefineStatic(
            string name, ParameterDefinition[] parameters, ResultDefinition result,
            VisibilityLevel visibility = VisibilityLevel.Public, bool hasSpecialName = false)
        {
            var attributes = MethodAttributes.HideBySig | MethodAttributes.Static |
                             visibility.ToMethodAttributes();
            if (hasSpecialName)
                attributes |= MethodAttributes.SpecialName;
            var methodBuilder = _context.BuildMethodBuilder(name, attributes, parameters, result);
            return new StaticDynamicFunctor(_context, methodBuilder);
        }

        public InstanceDynamicFunctor Override(MethodInfo method, string? name = null)
        {
            return new InstanceDynamicFunctor(_context,
                _context.BuildOverridenMethodBuilder(name ?? method.Name, method));
        }
    }

    public DynamicFunctorBuilder FunctorBuilder { get; }

    public DynamicActionBuilder ActionBuilder { get; }

    public DynamicConstructor Constructor(VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                         MethodAttributes.RTSpecialName | visibility.ToMethodAttributes();
        var constructorBuilder = TypeBuilder.DefineDefaultConstructor(attributes);
        return new DynamicConstructor(this, constructorBuilder);
    }

    public DynamicConstructor Constructor(
        ParameterDefinition[] parameters, VisibilityLevel visibility = VisibilityLevel.Public)
    {
        var attributes = MethodAttributes.HideBySig | MethodAttributes.SpecialName |
                         MethodAttributes.RTSpecialName | visibility.ToMethodAttributes();
        var constructorBuilder = TypeBuilder.DefineConstructor(attributes, CallingConventions.Standard,
            parameters.Select(parameter => parameter.Type).ToArray(),
            parameters.Select(parameter => parameter.Modifier.ToCustomAttributes()).ToArray(),
            parameters.Select(parameter => parameter.Attributes ?? Type.EmptyTypes).ToArray());
        return new DynamicConstructor(this, constructorBuilder);
    }
}