using System;
using System.Collections.Generic;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 输入设备组
    /// </summary>
    public class InputDeviceGroup
    {
        private Dictionary<Type, InputDevice> m_InputDeviceDic;
        private bool m_Enable;
        private string m_GroupName;

        /// <summary>
        /// 开启或关闭设备组
        /// </summary>
        public bool Enable
        {
            get { return m_Enable; }
            set 
            { 
                m_Enable = value;
                foreach (var devices in m_InputDeviceDic.Values)
                {
                    devices.Enable = value;
                }
            }
        }

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string GroupName
        {
            get { return m_GroupName; }
            set { m_GroupName = value; }
        }

        /// <summary>
        /// 获取设备组中的所有设备
        /// </summary>
        public InputDevice[] InputDevices
        {
            get { return new List<InputDevice>(m_InputDeviceDic.Values).ToArray(); }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">组名</param>
        public InputDeviceGroup(string name)
        {
            m_GroupName = name;
            m_InputDeviceDic = new Dictionary<Type, InputDevice>();
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        public void AddDevice(InputDevice device)
        {
            Type type = device.GetType();

            if (!m_InputDeviceDic.ContainsKey(type))
            {
                m_InputDeviceDic.Add(type, device);

                return;
            }

            throw new Exception(GroupName + "组中已经存在'" + type + "'设备输入");
        }

        /// <summary>
        /// 获取输入设备
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetDevice<T>() where T : InputDevice
        {
            Type type = typeof(T);

            if (m_InputDeviceDic.ContainsKey(type))
            {
                return (T)m_InputDeviceDic[type];
            }

            throw new Exception($"没有指定设备'{typeof(T)}'");
        }

        /// <summary>
        /// 获取输入设备
        /// </summary>
        /// <param name="type">输入类型</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public InputDevice GetDevice(Type type)
        {
            if (m_InputDeviceDic.ContainsKey(type))
            {
                return m_InputDeviceDic[type];
            }

            throw new Exception($"没有指定设备'{type}'");
        }

        /// <summary>
        /// 获取指定输入设备的所有行为
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public InputAction[] GetActions<T>() where T : InputDevice
        {
            Type type = typeof(T);

            if (m_InputDeviceDic.ContainsKey(type))
            {
                return m_InputDeviceDic[type].Actions;
            }

            throw new Exception("不存在指定类型的设备");
        }

        /// <summary>
        /// 获取指定输入设备的所有行为
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public InputAction[] GetActions(Type type)
        {
            if (m_InputDeviceDic.ContainsKey(type))
            {
                return m_InputDeviceDic[type].Actions;
            }

            throw new Exception("不存在指定类型的设备");
        }

        /// <summary>
        /// 是否包含设备
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool HasDevice<T>()
        {
            return m_InputDeviceDic.ContainsKey(typeof(T));
        }

        /// <summary>
        /// 是否包含设备
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasDevice(Type type)
        {
            return m_InputDeviceDic.ContainsKey(type);
        }

        /// <summary>
        /// 获取执行行为
        /// </summary>
        /// <typeparam name="TDevice"></typeparam>
        /// <typeparam name="TAction"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public TAction GetAction<TDevice, TAction>(string name)
            where TDevice : InputDevice
            where TAction : InputAction
        {
            var device = GetDevice<TDevice>();
            var action = device.GetAction<TAction>(name);
            return action;
        }

        /// <summary>
        /// 获取行为
        /// </summary>
        /// <param name="name"></param>
        /// <param name="inputDevice"></param>
        /// <param name="inputAction"></param>
        /// <returns></returns>
        public IInputAction GetAction(string name,Type inputDevice)
        {
            var device = GetDevice(inputDevice);
            var action = device.GetAction(name);
            return action;
        }
    }
}
