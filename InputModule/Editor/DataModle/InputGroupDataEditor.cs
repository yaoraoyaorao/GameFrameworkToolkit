using GameFramework.Toolkit.Runtime;
using System;
using System.Collections.Generic;

namespace GameFramework.Toolkit.Editor
{
    /// <summary>
    /// 输入组数据
    /// </summary>
    [System.Serializable]
    public class InputGroupDataEditor
    {
        public string GroupName;

        public bool Enable;

        public List<InputDeviceDataEditor> InputDevices = new List<InputDeviceDataEditor>();

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

        /// <summary>
        /// 是否存在当前类型设备
        /// </summary>
        /// <param name="type"></param> 
        /// <returns></returns>
        public bool HasDevice(Type type)
        {
            if (InputDevices == null || InputDevices.Count == 0)
            {
                return false;
            }

            foreach (var item in InputDevices)
            {
                if (item.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取设备
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public InputDeviceDataEditor GetDevice(Type type)
        {
            foreach (var item in InputDevices)
            {
                if (item.GetType() == type)
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// 添加设备
        /// </summary>
        /// <param name="type"></param>
        public void AddDevice(Type type)
        {
            if (HasDevice(type))
            {
                return;
            }

            var device = (InputDeviceDataEditor)Activator.CreateInstance(type);

            InputDevices.Add(device);
        }

        /// <summary>
        /// 移除设备
        /// </summary>
        /// <param name="deviceData"></param>
        public void RemoveDevice(InputDeviceDataEditor deviceData)
        {
            if (deviceData == null)
            {
                return;
            }

            if (InputDevices.Contains(deviceData))
            {
                InputDevices.Remove(deviceData);
            }
        }

        /// <summary>
        /// 存储数据
        /// </summary>
        /// <returns></returns>
        public InputGroupData Export()
        {
            InputGroupData group = new InputGroupData();
            group.Name = GroupName;
            group.Enable = Enable;
            group.Devices = new InputDeviceData[InputDevices.Count];

            for (int i = 0; i < InputDevices.Count; i++)
            {
                group.Devices[i] = InputDevices[i].Save();
            }

            return group;
        }


    }
}
