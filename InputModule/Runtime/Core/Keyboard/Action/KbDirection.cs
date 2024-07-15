using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class KbDirection : KbAction
    {
        private KeyCode[] m_KeyCodes;
        private Vector2 m_Direction;

        public Vector2 Dir => m_Direction;
        public KeyCode[] KeyCodes => m_KeyCodes;

        public override void ChangeActionData(InputActionData actionData)
        {
            m_Enable = actionData.Enable;
            m_KeyCodes = actionData.KeyCodes;
        }

        public override void Create(string name, InputActionData actionData)
        {
            m_Name = name;
            m_Enable = actionData.Enable;
            m_KeyCodes = actionData.KeyCodes;
            m_Direction = Vector2.zero;
        }

        public override bool Trigger()
        {
            if (m_KeyCodes == null || m_KeyCodes.Length != 4)
            {
                return false;
            }

            if (!m_Enable)
            {
                return false;
            }
            float horizontal = 0f;
            float vertical = 0f;

            // ио
            if (Input.GetKey(m_KeyCodes[0]))
            {
                vertical += 1f;
            }

            // вС
            if (Input.GetKey(m_KeyCodes[1]))
            {
                horizontal -= 1f;
            }

            // об
            if (Input.GetKey(m_KeyCodes[2]))
            {
                vertical -= 1f;
            }

            // ср
            if (Input.GetKey(m_KeyCodes[3]))
            {
                horizontal += 1f;
            }

            m_Direction = new Vector2(horizontal, vertical);

            if (m_Direction.sqrMagnitude > 1)
            {
                m_Direction.Normalize();
            }

            return true;
        }
    }

}
