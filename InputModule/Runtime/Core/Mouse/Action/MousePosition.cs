using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    ///  Û±ÍŒª÷√
    /// </summary>
    public class MousePosition : MouseAction
    {
        private Vector3 m_Position;

        public Vector3 Position => m_Position;

        public override void ChangeActionData(InputActionData actionData)
        {
            m_Enable = actionData.Enable;
        }

        public override void Create(string name, InputActionData actionData)
        {
            m_Name = name;
            m_Enable = actionData.Enable;
            m_Position = Vector3.zero;
        }

        public override bool Trigger()
        {
            if (!m_Enable)
            {
                m_Position = Vector3.zero;

                return false;
            }

            m_Position = Input.mousePosition;
            
            return true;
        }
    }
}

