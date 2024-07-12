using System;
using System.Collections.Generic;

namespace GameFramework.Toolkit.Runtime
{
    public partial class EventPool<T> where T : BaseEventArgs
    {
        private readonly Dictionary<int, LinkedList<EventHandler<T>>> m_EventHandlers;
        private readonly Queue<Event> m_Events;
        private readonly EventPoolMode m_EventPoolMode;
        private EventHandler<T> m_DefaultHandler;

        public EventPool(EventPoolMode mode)
        {
            m_EventHandlers = new Dictionary<int, LinkedList<EventHandler<T>>>();
            m_Events = new Queue<Event>();
            m_EventPoolMode = mode;
            m_DefaultHandler = null;
        }

        /// <summary>
        /// ��ȡ�¼���������������
        /// </summary>
        public int EventHandlerCount
        {
            get
            {
                return m_EventHandlers.Count;
            }
        }


        /// <summary>
        /// ��ȡ�¼�������
        /// </summary>
        public int EventCount
        {
            get
            {
                return m_Events.Count;
            }
        }

        /// <summary>
        /// ��ѯ�¼��ء�
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public void Update()
        {
            lock (m_Events)
            {
                while (m_Events.Count > 0)
                {
                    Event eventNode = m_Events.Dequeue();
                    HandleEvent(eventNode.Sender, eventNode.EventArgs);
                    ReferencePool.Release(eventNode);
                }
            }
        }

        /// <summary>
        /// �رղ������¼��ء�
        /// </summary>
        public void Shutdown()
        {
            Clear();
            m_EventHandlers.Clear();
            m_DefaultHandler = null;
        }

        /// <summary>
        /// �����¼���
        /// </summary>
        public void Clear()
        {
            lock (m_Events)
            {
                m_Events.Clear();
            }
        }

        /// <summary>
        /// ��ȡ�¼���������������
        /// </summary>
        /// <param name="id">�¼����ͱ�š�</param>
        /// <returns>�¼���������������</returns>
        public int Count(int id)
        {

            if (m_EventHandlers.ContainsKey(id))
            {
                return m_EventHandlers[id].Count;
            }

            return 0;
        }

        /// <summary>
        /// ����Ƿ�����¼�������
        /// </summary>
        /// <param name="id">�¼����ͱ��</param>
        /// <param name="handler">Ҫ�����¼�������</param>
        /// <returns>�Ƿ�����¼�������</returns>
        /// <exception cref="Exception"></exception>
        public bool Check(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            if (m_EventHandlers.ContainsKey(id))
            {
                return m_EventHandlers[id].Contains(handler);
            }

            return false;
        }

        /// <summary>
        /// �����¼�
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Event handler cannot be null.");
            }

            if (!m_EventHandlers.TryGetValue(id, out LinkedList<EventHandler<T>> list))
            {
                list = new LinkedList<EventHandler<T>>();
                list.AddLast(handler);
                m_EventHandlers.Add(id, list);
            }
            else if((m_EventPoolMode & EventPoolMode.AllowMultiHandler) != EventPoolMode.AllowMultiHandler)
            {
                throw new InvalidOperationException(string.Format("Event '{0}' not allow multi handler.", id));
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowDuplicateHandler) != EventPoolMode.AllowDuplicateHandler && Check(id, handler))
            {
                throw new InvalidOperationException(string.Format("Event '{0}' not allow duplicate handler.", id));
            }
            else
            {
                list.AddLast(handler);
            }
        }

        /// <summary>
        /// ȡ�������¼�
        /// </summary>
        /// <param name="id"></param>
        /// <param name="handler"></param>
        public void Unsubscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler), "Event handler cannot be null.");
            }

            if (m_EventHandlers.ContainsKey(id))
            {
                LinkedListNode<EventHandler<T>> currentNode = m_EventHandlers[id].Find(handler);

                if(currentNode != null)
                {
                    m_EventHandlers[id].Remove(currentNode);
                }
            }
        }

        /// <summary>
        /// ����Ĭ���¼���������
        /// </summary>
        /// <param name="handler">Ҫ���õ�Ĭ���¼���������</param>
        public void SetDefaultHandler(EventHandler<T> handler)
        {
            m_DefaultHandler = handler;
        }

        /// <summary>
        /// �׳��¼�������������̰߳�ȫ�ģ���ʹ�������߳����׳���Ҳ�ɱ�֤�����߳��лص��¼������������¼������׳������һ֡�ַ���
        /// </summary>
        /// <param name="sender">�¼�Դ��</param>
        /// <param name="e">�¼�������</param>
        public void Fire(object sender, T e)
        {
            if (e == null)
            {
                throw new Exception("Event is invalid.");
            }

            Event eventNode = Event.Create(sender, e);

            lock (m_Events)
            {
                m_Events.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// �׳��¼�����ģʽ��������������̰߳�ȫ�ģ��¼������̷ַ���
        /// </summary>
        /// <param name="sender">�¼�Դ��</param>
        /// <param name="e">�¼�������</param>
        public void FireNow(object sender, T e)
        {
            if (e == null)
            {
                throw new Exception("Event is invalid.");
            }

            HandleEvent(sender, e);
        }

        /// <summary>
        /// �����¼���㡣
        /// </summary>
        /// <param name="sender">�¼�Դ��</param>
        /// <param name="e">�¼�������</param>
        private void HandleEvent(object sender, T e)
        {
            bool noHandlerException = false;

            if (m_EventHandlers.ContainsKey(e.Id))
            {
                LinkedList<EventHandler<T>> eventHandlerList = m_EventHandlers[e.Id];

                foreach (EventHandler<T> handler in eventHandlerList)
                {
                    handler(sender, e);
                }
            }
            else if (m_DefaultHandler != null)
            {
                m_DefaultHandler(sender, e);
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowNoHandler) == 0)
            {
                noHandlerException = true;
            }

            ReferencePool.Release(e);

            if (noHandlerException)
            {
                throw new Exception(string.Format("Event '{0}' not allow no handler.", e.Id));
            }
        }
    }

}
