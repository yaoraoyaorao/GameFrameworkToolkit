using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class KbDirectionLerp : KbAction
    {
        private KeyCode[] m_KeyCodes;
        private Vector2 m_Direction;
        private float m_Sensitivity = 3.0f;
        private float m_Decay = 2.0f;

        public Vector2 Dir => m_Direction;
        public KeyCode[] KeyCodes => m_KeyCodes;
        public float Sensitivity => m_Sensitivity;

        private float horizontalAxis;
        private float verticalAxis;

        public override void ChangeActionData(InputActionData actionData)
        {
            m_Enable = actionData.Enable;
            m_KeyCodes = actionData.KeyCodes;
            m_Sensitivity = actionData.Time1;
            m_Decay = actionData.Time2;
        }

        public override void Create(string name, InputActionData actionData)
        {
            m_Name = name;
            m_Enable = actionData.Enable;
            m_KeyCodes = actionData.KeyCodes;
            m_Sensitivity = actionData.Time1;
            m_Decay = actionData.Time2;
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

            if (Input.GetKey(m_KeyCodes[0]))
            {
                verticalAxis += m_Sensitivity * Time.deltaTime;
            }
            if (Input.GetKey(m_KeyCodes[1]))
            {
                horizontalAxis -= m_Sensitivity * Time.deltaTime;
            }
            if (Input.GetKey(m_KeyCodes[2]))
            {
                verticalAxis -= m_Sensitivity * Time.deltaTime;
            }
            if (Input.GetKey(m_KeyCodes[3]))
            {
                horizontalAxis += m_Sensitivity * Time.deltaTime;
            }

            horizontalAxis = Mathf.Clamp(horizontalAxis, -1.0f, 1.0f);
            verticalAxis = Mathf.Clamp(verticalAxis, -1.0f, 1.0f);

            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                horizontalAxis = Mathf.MoveTowards(horizontalAxis, 0, m_Decay * Time.deltaTime);
            }
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
            {
                verticalAxis = Mathf.MoveTowards(verticalAxis, 0, m_Decay * Time.deltaTime);
            }

            m_Direction = new Vector2(horizontalAxis, verticalAxis);

            if (m_Direction.magnitude > 1)
            {
                m_Direction.Normalize();
                horizontalAxis = m_Direction.x;
                verticalAxis = m_Direction.y;
            }

            return true;
        }
    }

}
