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
        /// 获取事件处理函数的数量。
        /// </summary>
        public int EventHandlerCount
        {
            get
            {
                return m_EventHandlers.Count;
            }
        }


        /// <summary>
        /// 获取事件数量。
        /// </summary>
        public int EventCount
        {
            get
            {
                return m_Events.Count;
            }
        }

        /// <summary>
        /// 轮询事件池。
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
        /// 关闭并清理事件池。
        /// </summary>
        public void Shutdown()
        {
            Clear();
            m_EventHandlers.Clear();
            m_DefaultHandler = null;
        }

        /// <summary>
        /// 清理事件。
        /// </summary>
        public void Clear()
        {
            lock (m_Events)
            {
                m_Events.Clear();
            }
        }

        /// <summary>
        /// 获取事件处理函数的数量。
        /// </summary>
        /// <param name="id">事件类型编号。</param>
        /// <returns>事件处理函数的数量。</returns>
        public int Count(int id)
        {

            if (m_EventHandlers.ContainsKey(id))
            {
                return m_EventHandlers[id].Count;
            }

            return 0;
        }

        /// <summary>
        /// 检查是否存在事件处理函数
        /// </summary>
        /// <param name="id">事件类型编号</param>
        /// <param name="handler">要检查的事件处理函数</param>
        /// <returns>是否存在事件处理函数</returns>
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
        /// 订阅事件
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
        /// 取消订阅事件
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
        /// 设置默认事件处理函数。
        /// </summary>
        /// <param name="handler">要设置的默认事件处理函数。</param>
        public void SetDefaultHandler(EventHandler<T> handler)
        {
            m_DefaultHandler = handler;
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
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
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void FireNow(object sender, T e)
        {
            if (e == null)
            {
                throw new Exception("Event is invalid.");
            }

            HandleEvent(sender, e);
        }

        /// <summary>
        /// 处理事件结点。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
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
