using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class MouseScrollDelta : MouseAction
    {
        private Vector2 m_ScrollDelta;
        public Vector2 ScrollDelta => m_ScrollDelta;

        public override void ChangeActionData(InputActionData actionData)
        {
            m_Enable = actionData.Enable;
        }

        public override void Create(string name, InputActionData actionData)
        {
            m_Name = name;
            m_Enable = actionData.Enable;
            m_ScrollDelta = Vector2.zero;
        }

        public override bool Trigger()
        {

            if (!m_Enable)
            {
                m_ScrollDelta = Vector2.zero;

                return false;
            }

            m_ScrollDelta = Input.mouseScrollDelta;
            return true;
        }
    }

}

