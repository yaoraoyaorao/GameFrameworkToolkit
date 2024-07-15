using System;
using System.Collections.Generic;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 输入设备实现
    /// </summary>
    public class InputDevice : IInputDevice
    {
        protected bool m_Enable;

        private Dictionary<string, InputAction> m_ActionDic;

        /// <summary>
        /// 是否启用设备
        /// </summary>
        public virtual bool Enable
        {
            get { return m_Enable; }
            set
            {
                m_Enable = value;

                foreach (var action in m_ActionDic.Values)
                {
                    action.Enable = value;
                }
            }
        }

        /// <summary>
        /// 行为组
        /// </summary>
        public virtual InputAction[] Actions => GetArray();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="actions"></param>
        public InputDevice(bool enable,params InputAction[] actions)
        {
            if (actions == null || actions.Length < 1)
            {
                throw new Exception("输入行为为空");
            }

            m_ActionDic = new Dictionary<string, InputAction>();

            foreach (var action in actions)
            {
                if (!m_ActionDic.ContainsKey(action.Name))
                {
                    action.Enable = enable;
                    m_ActionDic[action.Name] = action;
                }
                else
                {
                    throw new Exception("已经存在名为" + action.Name + "的行为");
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Shutdown()
        {
            foreach (var action in m_ActionDic.Values)
            {
                ReferencePool.Release(action);
            }

            m_ActionDic.Clear();
        }

        /// <summary>
        /// 开启或关闭行为
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="enable"></param>
        public void EnableAction(string actionName, bool enable)
        {
            if (m_ActionDic.ContainsKey(actionName))
            {
                m_ActionDic[actionName].Enable = enable;
            }
            else
            {
                throw new Exception("不存在名为" + actionName + "的行为");
            }
        }

        /// <summary>
        /// 是否存在行为
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool HasAction(string actionName)
        {
            return m_ActionDic.ContainsKey(actionName);
        }

        /// <summary>
        /// 获取行为数组
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private InputAction[] GetArray()
        {
            if (m_ActionDic == null || m_ActionDic.Count < 1)
            {
                throw new Exception("获取行为组失败：输入行为组为空");
            }

            InputAction[] actions = new KbAction[m_ActionDic.Count];

            int index = 0;

            foreach (var action in m_ActionDic.Values)
            {
                actions[index++] = action;
            }

            return actions;
        }

        /// <summary>
        /// 获取行为
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public virtual T GetAction<T>(string actionName) where T : class, IInputAction
        {
            if (m_ActionDic.ContainsKey(actionName))
            {
                var action = m_ActionDic[actionName];
                return action as T;
            }

            throw new Exception("不存在名为" + actionName + "的行为");
        }

        /// <summary>
        /// 获取行为
        /// </summary>
        /// <param name="actionName">行为名</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public virtual IInputAction GetAction(string actionName)
        {
            if (m_ActionDic.ContainsKey(actionName))
            {
                var action = m_ActionDic[actionName];
                return action;
            }

            throw new Exception("不存在名为" + actionName + "的行为");
        }
    }
}
