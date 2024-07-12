using System;

namespace GameFramework.Toolkit.Runtime
{
    public abstract class SunFrameworkEventArgs : EventArgs, IReference
    {
        public SunFrameworkEventArgs()
        {
            
        }

        public abstract void Clear();
    }
}

