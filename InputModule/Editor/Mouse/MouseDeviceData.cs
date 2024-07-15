using GameFramework.Toolkit.Runtime;

namespace GameFramework.Toolkit.Editor
{
    public class MouseDeviceData : InputDeviceData<MouseActionData>
    {
        public override string TypeName => typeof(MouseDevice).FullName;
    }

}
