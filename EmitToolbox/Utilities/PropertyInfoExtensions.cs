namespace EmitToolbox.Utilities;

public static class PropertyInfoExtensions
{
    extension(PropertyInfo self)
    {
        /// <summary>
        /// Whether this property has a static getter or setter.
        /// </summary>
        public bool IsStatic => 
            self.GetGetMethod(true) is {IsStatic: true} || 
            self.GetSetMethod(true) is {IsStatic: true};

        /// <summary>
        /// Whether this property has a public getter or setter.
        /// </summary>
        public bool IsPublic =>
            self.GetGetMethod(false) != null ||
            self.GetSetMethod(false) != null;
    }
}