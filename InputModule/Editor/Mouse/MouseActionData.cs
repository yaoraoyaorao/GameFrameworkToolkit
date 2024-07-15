using GameFramework.Toolkit.Runtime;

namespace GameFramework.Toolkit.Editor
{
    public class MouseActionData : InputActionDataEditor
    {
       
    }

    [InputDataName("抬起")]
    public class MouseUpData : MouseActionData
    {
        public override string TargetTypeName => typeof(MouseUp).FullName;

        public MouseButton Button;

        public override InputActionData Save()
        {
            var data = base.Save();
            data.MouseButton = Button;
            return data;
        }
    }

    [InputDataName("按下")]
    public class MouseDownData : MouseActionData
    {
        public override string TargetTypeName => typeof(MouseDown).FullName;

        public MouseButton Button;

        public override InputActionData Save()
        {
            var data = base.Save();
            data.MouseButton = Button;
            return data;
        }
    }

    [InputDataName("持续")]
    public class MousePressData : MouseActionData
    {
        public override string TargetTypeName => typeof(MousePress).FullName;

        public MouseButton Button;

        public override InputActionData Save()
        {
            var data = base.Save();
            data.MouseButton = Button;
            return data;
        }
    }

    [InputDataName("多击")]
    public class MouseMultiClickData : MouseActionData
    {
        public override string TargetTypeName => typeof(MouseMultiClick).FullName;

        public int Count;
        public float Interval;
        public override InputActionData Save()
        {
            var data = base.Save();
            data.Count = Count;
            data.Time1 = Interval;
            return data;
        }
    }

    [InputDataName("位置")]
    public class MousePositionData : MouseActionData
    {
        public override string TargetTypeName => typeof(MousePosition).FullName;
    }    
    
    [InputDataName("滚轮")]
    public class MouseScrollDeltanData : MouseActionData
    {
        public override string TargetTypeName => typeof(MouseScrollDelta).FullName;
    }
}
