namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// ����豸
    /// </summary>
    public class MouseDevice : InputDevice
    {
        public MouseDevice(bool enable, params InputAction[] actions) : base(enable, actions)
        {
        }
    }
}

