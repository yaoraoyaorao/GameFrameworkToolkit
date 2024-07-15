using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class KbMultiClick : KbAction
    {
        private KeyCode[] m_Code;
        private int m_Count;
        private float m_Interval;

        private float m_LastClickTime;
        private int m_ClickCount;

        public KeyCode[] Code => m_Code;
        public int Count => m_Count;
        public float Interval => m_Interval;

        public override void ChangeActionData(InputActionData actionData)
        {
            m_Count = actionData.Count;
            m_Enable = actionData.Enable;
            m_Interval = actionData.Time1;
            m_Code = actionData.KeyCodes;
        }

        public override void Create(string name, InputActionData actionData)
        {
            m_Name = name;
            m_Enable = actionData.Enable;
            m_Count = actionData.Count;
            m_Interval = actionData.Time1;
            m_Code = actionData.KeyCodes;
        }

        public override bool Trigger()
        {
            if (m_Code == null || m_Code.Length == 0 || m_Count <= 0 || m_Interval <= 0)
            {
                return false;
            }

            if (!m_Enable) return false;

            foreach (KeyCode keyCode in m_Code)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    float timeSinceLastClick = Time.time - m_LastClickTime;

                    if (timeSinceLastClick <= m_Interval)
                    {
                        m_ClickCount++;
                        m_LastClickTime = Time.time;

                        if (m_ClickCount == m_Count)
                        {
                            m_ClickCount = 0;
                            return true;
                        }
                    }
                    else
                    {
                        m_ClickCount = 1;
                        m_LastClickTime = Time.time;
                    }
                }
            }

            return false;
        }
    }

}
