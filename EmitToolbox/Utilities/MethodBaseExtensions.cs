namespace EmitToolbox.Utilities;

public static class MethodBaseExtensions
{
    extension(MethodBase self)
    {
        public IEnumerable<Type> GetParameterTypes()
            => self.GetParameters().Select(parameter => parameter.ParameterType);
    }
}