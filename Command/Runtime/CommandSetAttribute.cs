using System;

namespace GameFramework.Toolkit.Runtime
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandSetAttribute : Attribute
    {
        public readonly string Name;

        public CommandSetAttribute(string name)
        {
            Name = name;
        }

        public CommandSetAttribute()
        {
            
        }
    }
}

