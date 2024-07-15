using System;
using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    [DefaultExecutionOrder(-110)]
    public class EventComponent : MonoBehaviour
    {
        private EventPool<GameEventArgs> m_EventPool;

        /// <summary>
        /// ��ȡ�¼���������������
        /// </summary>
        public int EventHandlerCount
        {
            get
            {
                return m_EventPool.EventHandlerCount;
            }
        }

        /// <summary>
        /// ��ȡ�¼�������
        /// </summary>
        public int EventCount
        {
            get
            {
                return m_EventPool.EventCount;
            }
        }

        private void Start()
        {
            m_EventPool = new EventPool<GameEventArgs>(EventPoolMode.AllowNoHandler | EventPoolMode.AllowMultiHandler);
        }

        private void Update()
        {
            m_EventPool.Update();
        }

        private void OnDestroy()
        {
            m_EventPool.Shutdown();
        }

        /// <summary>
        /// ��ȡ�¼���������������
        /// </summary>
        /// <param name="id">�¼����ͱ�š�</param>
        /// <returns>�¼���������������</returns>
        public int Count(int id)
        {
            return m_EventPool.Count(id);
        }

        /// <summary>
        /// ����Ƿ�����¼���������
        /// </summary>
        /// <param name="id">�¼����ͱ�š�</param>
        /// <param name="handler">Ҫ�����¼���������</param>
        /// <returns>�Ƿ�����¼���������</returns>
        public bool Check(int id, EventHandler<GameEventArgs> handler)
        {
            return m_EventPool.Check(id, handler);
        }

        /// <summary>
        /// �����¼���������
        /// </summary>
        /// <param name="id">�¼����ͱ�š�</param>
        /// <param name="handler">Ҫ���ĵ��¼���������</param>
        public void Subscribe(int id, EventHandler<GameEventArgs> handler)
        {
            m_EventPool.Subscribe(id, handler);
        }
        /// <summary>
        /// ȡ�������¼���������
        /// </summary>
        /// <param name="id">�¼����ͱ�š�</param>
        /// <param name="handler">Ҫȡ�����ĵ��¼���������</param>
        public void Unsubscribe(int id, EventHandler<GameEventArgs> handler)
        {
            m_EventPool.Unsubscribe(id, handler);
        }
        /// <summary>
        /// ����Ĭ���¼���������
        /// </summary>
        /// <param name="handler">Ҫ���õ�Ĭ���¼���������</param>
        public void SetDefaultHandler(EventHandler<GameEventArgs> handler)
        {
            m_EventPool.SetDefaultHandler(handler);
        }

        /// <summary>
        /// �׳��¼�������������̰߳�ȫ�ģ���ʹ�������߳����׳���Ҳ�ɱ�֤�����߳��лص��¼������������¼������׳������һ֡�ַ���
        /// </summary>
        /// <param name="sender">�¼�Դ��</param>
        /// <param name="e">�¼�������</param>
        public void Fire(object sender, GameEventArgs e)
        {
            m_EventPool.Fire(sender, e);
        }

        /// <summary>
        /// �׳��¼�����ģʽ��������������̰߳�ȫ�ģ��¼������̷ַ���
        /// </summary>
        /// <param name="sender">�¼�Դ��</param>
        /// <param name="e">�¼�������</param>
        public void FireNow(object sender, GameEventArgs e)
        {
            m_EventPool.FireNow(sender, e);
        }
    }
}
