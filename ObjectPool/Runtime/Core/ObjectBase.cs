using System;

namespace GameFramework.Toolkit.Runtime
{
    public abstract class ObjectBase : IReference
    {
        private string m_Name;
        private object m_Target;
        private bool m_Locked;
        private int m_Priority;
        private DateTime m_LastUseTime;

        public ObjectBase()
        {
            m_Name = null;
            m_Target = null;
            m_Locked = false;
            m_Priority = 0;
            m_LastUseTime = default(DateTime);
        }

        /// <summary>
        /// ��ȡ�������ơ�
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        public object Target
        {
            get
            {
                return m_Target;
            }
        }

        /// <summary>
        /// ��ȡ�����ö����Ƿ񱻼�����
        /// </summary>
        public bool Locked
        {
            get
            {
                return m_Locked;
            }
            set
            {
                m_Locked = value;
            }
        }

        /// <summary>
        /// ��ȡ�����ö�������ȼ���
        /// </summary>
        public int Priority
        {
            get
            {
                return m_Priority;
            }
            set
            {
                m_Priority = value;
            }
        }

        /// <summary>
        /// ��ȡ�Զ����ͷż���ǡ�
        /// </summary>
        public virtual bool CustomCanReleaseFlag
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// ��ȡ�����ϴ�ʹ��ʱ�䡣
        /// </summary>
        public DateTime LastUseTime
        {
            get
            {
                return m_LastUseTime;
            }
            internal set
            {
                m_LastUseTime = value;
            }
        }

        /// <summary>
        /// ��ʼ��������ࡣ
        /// </summary>
        /// <param name="target">����</param>
        protected void Initialize(object target)
        {
            Initialize(null, target, false, 0);
        }

        /// <summary>
        /// ��ʼ��������ࡣ
        /// </summary>
        /// <param name="name">�������ơ�</param>
        /// <param name="target">����</param>
        protected void Initialize(string name, object target)
        {
            Initialize(name, target, false, 0);
        }

        /// <summary>
        /// ��ʼ��������ࡣ
        /// </summary>
        /// <param name="name">�������ơ�</param>
        /// <param name="target">����</param>
        /// <param name="locked">�����Ƿ񱻼�����</param>
        protected void Initialize(string name, object target, bool locked)
        {
            Initialize(name, target, locked, 0);
        }

        /// <summary>
        /// ��ʼ��������ࡣ
        /// </summary>
        /// <param name="name">�������ơ�</param>
        /// <param name="target">����</param>
        /// <param name="priority">��������ȼ���</param>
        protected void Initialize(string name, object target, int priority)
        {
            Initialize(name, target, false, priority);
        }

        /// <summary>
        /// ��ʼ��������ࡣ
        /// </summary>
        /// <param name="name">�������ơ�</param>
        /// <param name="target">����</param>
        /// <param name="locked">�����Ƿ񱻼�����</param>
        /// <param name="priority">��������ȼ���</param>
        protected void Initialize(string name, object target, bool locked, int priority)
        {
            if (target == null)
            {
                throw new Exception(string.Format("Target '{0}' is invalid.", name));
            }

            m_Name = name ?? string.Empty;
            m_Target = target;
            m_Locked = locked;
            m_Priority = priority;
            m_LastUseTime = DateTime.UtcNow;
        }

        public void Clear()
        {
            m_Name = null;
            m_Target = null;
            m_Locked = false;
            m_Priority = 0;
            m_LastUseTime = default(DateTime);
        }

        /// <summary>
        /// ��ȡ����ʱ���¼���
        /// </summary>
        public virtual void OnSpawn()
        {
        }

        /// <summary>
        /// ���ն���ʱ���¼���
        /// </summary>
        public virtual void OnUnspawn()
        {
        }

        /// <summary>
        /// �ͷŶ���
        /// </summary>
        /// <param name="isShutdown">�Ƿ��ǹرն����ʱ������</param>
        public abstract void Release(bool isShutdown);
    }
}

