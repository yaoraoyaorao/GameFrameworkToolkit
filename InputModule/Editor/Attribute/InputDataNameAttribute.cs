using System;

namespace GameFramework.Toolkit.Editor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InputDataNameAttribute : Attribute
    {
        public string Name { get; }

        public InputDataNameAttribute(string name)
        {
            Name = name;
        }
    }
}
