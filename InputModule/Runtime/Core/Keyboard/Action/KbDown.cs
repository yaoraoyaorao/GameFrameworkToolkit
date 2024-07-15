using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 键盘按下行为
    /// </summary>
    public class KbDown : KbAction
    {
        private KeyCode[] m_Code;

        public KeyCode[] Code => m_Code;

        public override void ChangeActionData(InputActionData actionData)
        {
            m_Code = actionData.KeyCodes;
            m_Enable = actionData.Enable;
        }

        public override void Create(string name, InputActionData actionData)
        {
            m_Name = name;
            m_Enable = actionData.Enable;
            m_Code = actionData.KeyCodes;
        }

        public override bool Trigger()
        {
            if (m_Code == null || m_Code.Length == 0)
            {
                return false;
            }

            if (!m_Enable) return false;

            foreach (var keyCode in Code)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
