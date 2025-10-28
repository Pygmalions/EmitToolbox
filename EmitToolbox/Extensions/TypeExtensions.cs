using System.Text;

namespace EmitToolbox.Extensions;

public static class TypeExtensions
{
    extension(Type self)
    {
        /// <summary>
        /// Convert the full name of the specified type into a unique and friendly name
        /// which can be used as the name for dynamic type,
        /// and then add optional prefix and postfix to it.
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

            if (!self.IsGenericType) return 
                builder.ToString();
        
            foreach (var genericArgument in self.GetGenericArguments())
            {
                builder.Append('`');
                builder.Append(GetFriendlyTypeNameForDynamic(genericArgument));
            }

            return builder.ToString();
        }
    }
}