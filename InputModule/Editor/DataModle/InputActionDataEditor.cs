using GameFramework.Toolkit.Runtime;
using System;

namespace GameFramework.Toolkit.Editor
{
    /// <summary>
    /// 输入行为数据
    /// </summary>
    [System.Serializable]
    public class InputActionDataEditor
    {

        public string ActionName;

        public string Name;

        public bool Enable;

        private string m_CacheName;
        public string CacheName
        {
            get
            {
                return m_CacheName;
            }
            set
            {
                m_CacheName = value;
            }
        }

        private bool m_Foldout;
        public bool Foldout
        {
            get
            {
                return m_Foldout;
            }
            set
            {
                m_Foldout = value;
            }
        }

        public virtual string TargetTypeName
        {
            get;
        }

        public virtual Type BaseType
        {
            get
            {
                return GetType();
            }
        }

        public virtual InputActionData Save()
        {
            InputActionData data = new InputActionData();

            data.Name = Name;
            data.Enable = Enable;
            data.TypeName = TargetTypeName;

            return data;   
        }
    }
}
