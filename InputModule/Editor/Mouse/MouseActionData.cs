using GameFramework.Toolkit.Runtime;

namespace GameFramework.Toolkit.Editor
{
    public class MouseActionData : InputActionDataEditor
    {
       
    }

    [InputDataName("̧��")]
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

    [InputDataName("����")]
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

    [InputDataName("����")]
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

    [InputDataName("���")]
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

    [InputDataName("λ��")]
    public class MousePositionData : MouseActionData
    {
        public override string TargetTypeName => typeof(MousePosition).FullName;
    }    
    
    [InputDataName("����")]
    public class MouseScrollDeltanData : MouseActionData
    {
        public override string TargetTypeName => typeof(MouseScrollDelta).FullName;
    }
}
