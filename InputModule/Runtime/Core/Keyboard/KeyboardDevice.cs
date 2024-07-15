namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 键盘设备
    /// </summary>
    public class KeyboardDevice : InputDevice
    {
        public KeyboardDevice(bool enable, params InputAction[] actions) : base(enable, actions)
        {
        }
    }
}
