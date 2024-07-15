using GameFramework.Toolkit.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public class KeyboardActionData : InputActionDataEditor
    {

    }

    [InputDataName("抬起")]
    public class KeyboardUpData : KeyboardActionData 
    {
        public override string TargetTypeName => typeof(KbUp).FullName;

        public List<KeyCode> KeyCodes = new List<KeyCode>();

        public override InputActionData Save()
        {
            var data = base.Save();
            data.KeyCodes = KeyCodes.ToArray();
            return data;
        }
    }

    [InputDataName("持续")]
    public class KeyboardPressData : KeyboardActionData 
    {
        public override string TargetTypeName => typeof(KbPress).FullName;

        public List<KeyCode> KeyCodes = new List<KeyCode>();

        public override InputActionData Save()
        {
            var data = base.Save();
            data.KeyCodes = KeyCodes.ToArray();
            return data;
        }
    }

    [InputDataName("按下")]
    public class KeyboardDownData : KeyboardActionData
    {
        public override string TargetTypeName => typeof(KbDown).FullName;

        public List<KeyCode> KeyCodes = new List<KeyCode>();

        public override InputActionData Save()
        {
            var data = base.Save();
            data.KeyCodes = KeyCodes.ToArray();
            return data;
        }
    }

    [InputDataName("多击")]
    public class KeyboardMultiClickData : KeyboardActionData
    {
        public override string TargetTypeName => typeof(KbMultiClick).FullName;

        public int Count;

        public float Interval;

        public List<KeyCode> KeyCodes = new List<KeyCode>();

        public override InputActionData Save()
        {
            var data = base.Save();
            data.KeyCodes = KeyCodes.ToArray();
            data.Count = Count;
            data.Time1 = Interval;
            return data;
        }
    }

    [InputDataName("长按")]
    public class KeyboardHoldData : KeyboardActionData
    {
        public override string TargetTypeName => typeof(KbHold).FullName;

        public float HoldTime;

        public List<KeyCode> KeyCodes = new List<KeyCode>();

        public override InputActionData Save()
        {
            var data = base.Save();
            data.KeyCodes = KeyCodes.ToArray();
            data.Time1 = HoldTime;
            return data;
        }
    }

    [InputDataName("方向")]
    public class KeyboardDirData : KeyboardActionData
    {
        public override string TargetTypeName => typeof(KbDirection).FullName;

        public List<KeyCode> KeyCodes = new List<KeyCode>()
        {
            KeyCode.W,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D
        };

        public override InputActionData Save()
        {
            var data = base.Save();
            data.KeyCodes = KeyCodes.ToArray();
            return data;
        }
    }

    [InputDataName("方向-插值")]
    public class KeyboardDirLerpData : KeyboardActionData
    {
        public override string TargetTypeName => typeof(KbDirectionLerp).FullName;

        public float Sensitivity = 3.0f;
        public float Decay = 2.0f;

        public List<KeyCode> KeyCodes = new List<KeyCode>()
        {
            KeyCode.W,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D
        };

        public override InputActionData Save()
        {
            var data = base.Save();
            data.KeyCodes = KeyCodes.ToArray();
            data.Time1 = Sensitivity;
            data.Time2 = Decay;
            return data;
        }
    }
}
