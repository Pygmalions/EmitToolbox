using System.Text;

namespace EmitToolbox.Framework.Utilities;

public static class TypeExtensions
{
    extension(Type self)
    {
        /// <summary>
        /// Convert the full name of the specified type into a unique and friendly name
        /// which can be used as the name for dynamic type,
        /// and then add an optional prefix and postfix to it.
        /// Generic arguments will be appended with a backtick (`) separator in their friendly forms.
        /// </summary>
        /// <param name="prefix">Optional prefix to add into the type name.</param>
        /// <param name="postfix">Optional postfix to add into the type name.</param>
        /// <returns>Name with the prefix and postfix.</returns>
        public string GetFriendlyTypeNameForDynamic(string? prefix = null, string? postfix = null)
        {
            var builder = new StringBuilder();
            if (self.Assembly != typeof(object).Assembly)
            {
                builder.Append(self.Namespace);
                builder.Append('.');
            }

            if (prefix != null)
                builder.Append(prefix);
            builder.Append(self.Name);
            if (postfix != null)
                builder.Append(postfix);

            if (!self.IsGenericType)
                return
                    builder.ToString();

            foreach (var genericArgument in self.GetGenericArguments())
            {
                builder.Append('`');
                builder.Append(GetFriendlyTypeNameForDynamic(genericArgument));
            }

            return builder.ToString();
        }
        
        public MethodInfo? GetMethodByReturnType(string name, Type returnType,
            IReadOnlyCollection<Type>? parameters = null,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance,
            bool? hasSpecialName = null)
        {
            var candidates = self.GetMethods(bindingFlags)
                .Where(method => method.Name == name && method.ReturnType == returnType);
            if (hasSpecialName != null)
                candidates = candidates.Where(method => method.IsSpecialName == hasSpecialName.Value);
            if (parameters != null)
                candidates = candidates
                    .Where(method =>
                        method.GetParameters().Length == parameters.Count &&
                        method.GetParameters()
                            .Select(parameter => parameter.ParameterType)
                            .SequenceEqual(parameters));
            return candidates.FirstOrDefault();
        }

        public MethodInfo RequireMethod(string name, Type[] parameters)
        {
            return self.GetMethod(name, parameters)
                ?? throw new Exception($"Cannot find the required method '{name}' on type '{self}'.");
        }
        
        public MethodInfo RequireMethod(string name, BindingFlags flags, Type[] parameters)
        {
            return self.GetMethod(name, flags, parameters)
                   ?? throw new Exception($"Cannot find the required method '{name}' on type '{self}'.");
        }
    }
}