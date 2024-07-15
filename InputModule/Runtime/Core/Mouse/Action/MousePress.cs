using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class MousePress : MouseAction
    {
        private int m_Button;

        public override void ChangeActionData(InputActionData actionData)
        {
            m_Button = (int)actionData.MouseButton;
        }

        public override void Create(string name, InputActionData actionData)
        {
            m_Name = name;
            m_Enable = actionData.Enable;
            m_Button = (int)actionData.MouseButton;
        }

        public override bool Trigger()
        {
            if (!m_Enable) return false;

            return Input.GetMouseButton(m_Button);
        }
    }

}
