using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class MouseMultiClick : MouseAction
    {
        private int m_Button;
        private int m_Count;
        private float m_Interval;

        private float m_LastClickTime;
        private int m_ClickCount;

        public int Button => m_Button;
        public int Count => m_Count;
        public float Interval => m_Interval;

        public override void ChangeActionData(InputActionData actionData)
        {
            m_Count = actionData.Count;
            m_Enable = actionData.Enable;
            m_Interval = actionData.Time1;
            m_Button = (int)actionData.MouseButton;
        }

        public override void Create(string name, InputActionData actionData)
        {
            m_Name = name;
            m_Count = actionData.Count;
            m_Enable = actionData.Enable;
            m_Interval = actionData.Time1;
            m_Button = (int)actionData.MouseButton;
        }

        public override bool Trigger()
        {
            if (!m_Enable) return false;

            if (Input.GetMouseButtonDown(m_Button))
            {
                float timeSinceLastClick = Time.time - m_LastClickTime;

                if (timeSinceLastClick <= m_Interval)
                {
                    m_ClickCount++;
                    m_LastClickTime = Time.time;

                    if (m_ClickCount == m_Count)
                    {
                        m_ClickCount = 1;
                        return true;
                    }
                }
                else
                {
                    m_ClickCount = 0;
                    m_LastClickTime = Time.time;
                }
            }

            return false;
        }
    }
}

