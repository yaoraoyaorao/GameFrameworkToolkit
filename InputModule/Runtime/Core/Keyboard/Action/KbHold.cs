using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class KbHold : KbAction
    {
        private KeyCode[] m_Code;
        private float m_HoldTime;

        public KeyCode[] Code => m_Code;
        public float HoldTime => m_HoldTime;

        private float m_CurrentTime;

        public override void ChangeActionData(InputActionData actionData)
        {
            m_Enable = actionData.Enable;
            m_Code = actionData.KeyCodes;
            m_HoldTime = actionData.Time1;
        }

        public override void Create(string name, InputActionData actionData)
        {
            m_Name = name;
            m_Enable = actionData.Enable;
            m_Code = actionData.KeyCodes;
            m_HoldTime = actionData.Time1;
        }

        public override bool Trigger()
        {
            if (m_Code == null || m_Code.Length == 0 || m_HoldTime <= 0)
            {
                return false;
            }

            if (!m_Enable) return false;

            foreach (var code in m_Code)
            {
                if (Input.GetKey(code))
                {
                    m_CurrentTime += Time.deltaTime;

                    if (m_CurrentTime >= m_HoldTime)
                    {
                        m_CurrentTime = 0;
                        return true;
                    }
                }
                else
                {
                    m_CurrentTime = 0;
                }
            }
            return false;
        }
    }
}

