using GameFramework.Toolkit.Runtime;

namespace GameFramework.Toolkit.Editor
{
    public class KeyboardDeviceData : InputDeviceData<KeyboardActionData>
    {
        public override string TypeName => typeof(KeyboardDevice).FullName;
    }
}
