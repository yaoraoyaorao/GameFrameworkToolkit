using System;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 行为构建
    /// </summary>
    public static class InputActionCreate
    {
        public static InputAction Create(Type type, InputActionData data)
        {
            InputAction action = (InputAction)ReferencePool.Acquire(type);

            action.Create(data.Name, data);

            return action;
        }
    }
}
