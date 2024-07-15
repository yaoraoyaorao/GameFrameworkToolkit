using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 输入模块
    /// </summary>
    public class InputModule : MonoBehaviour
    {
        [SerializeField]
        private TextAsset m_InputConfig;

        private InputGroupData[] groupDatas;
        private Dictionary<string, InputDeviceGroup> m_InputDeviceGroups;
         
        private void Awake()
        {
            Build();
        }

        /// <summary>
        /// 获取输入设备
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupName"></param>
        public T GetDevice<T>(string groupName) where T : InputDevice
        {
            if (m_InputDeviceGroups.ContainsKey(groupName))
            {
                return m_InputDeviceGroups[groupName].GetDevice<T>();
            }

            return default;
        }

        /// <summary>
        /// 获取输入设备
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public InputDevice GetDevice(string groupName,Type type)
        {
            if (m_InputDeviceGroups.ContainsKey(groupName))
            {
                return m_InputDeviceGroups[groupName].GetDevice(type);
            }

            return null;
        }

        /// <summary>
        /// 获取行为
        /// </summary>
        /// <typeparam name="TDevice"></typeparam>
        /// <typeparam name="TAction"></typeparam>
        /// <param name="groupName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public TAction GetAction<TDevice, TAction>(string groupName, string actionName)
            where TDevice : InputDevice
            where TAction : InputAction
        {
            if (m_InputDeviceGroups.ContainsKey(groupName))
            {
                return m_InputDeviceGroups[groupName].GetAction<TDevice, TAction>(actionName);
            }

            return default;
        }

        /// <summary>
        /// 获取行为
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="actionName"></param>
        /// <param name="deviceType"></param>
        /// <returns></returns>
        public IInputAction GetAction(string groupName, string actionName, Type deviceType)
        {
            if (m_InputDeviceGroups.ContainsKey(groupName))
            {
                return m_InputDeviceGroups[groupName].GetAction(actionName, deviceType);
            }

            return null;
        }

        /// <summary>
        /// 获取组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public InputDeviceGroup GetGroup(string groupName)
        {
            if (m_InputDeviceGroups.ContainsKey(groupName))
            {
                return m_InputDeviceGroups[groupName];
            }

            return null;
        }

        /// <summary>
        /// 切换输入行为数据
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="actionName"></param>
        /// <param name="data"></param>
        public void ChangeInputAction(string groupName, string actionName, Type deviceType, InputActionData data)
        {
            var action = GetAction(groupName, actionName, deviceType);

            if (action != null)
            {
                action.ChangeActionData(data);
            }
        }

        /// <summary>
        /// 开启或关闭组
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="enable"></param>
        /// <exception cref="Exception"></exception>
        public void EnableGroup(string groupName, bool enable)
        {
            if (m_InputDeviceGroups.ContainsKey(groupName))
            {
                m_InputDeviceGroups[groupName].Enable = enable;
            }
        }

        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="path"></param>
        public async void SaveAsync(string path)
        {
            try
            {
                JsonHelper.SetJsonHelper(new NewtonsoftJsonHelper());

                var jsonStr = JsonHelper.ToJson(groupDatas);

                await System.IO.File.WriteAllTextAsync(path, jsonStr);
            }
            catch (Exception e)
            {
                Debug.Log("输入配置失败:" + e.Message);
            }

        }

        /// <summary>
        /// 构建
        /// </summary>
        private void Build()
        {
            m_InputDeviceGroups = new Dictionary<string, InputDeviceGroup>();

            string inputConfig = m_InputConfig.text;

            JsonHelper.SetJsonHelper(new NewtonsoftJsonHelper());

            groupDatas = JsonHelper.ToObject<InputGroupData[]>(inputConfig);

            foreach (var group in groupDatas)
            {
                InputDeviceGroup inputDeviceGroup = new InputDeviceGroup(group.Name);
                inputDeviceGroup.Enable = group.Enable;

                foreach (var device in group.Devices)
                {
                    Type deviceType = Type.GetType(device.TypeName);

                    InputAction[] actions = new InputAction[device.Actions.Length];

                    for (int i = 0; i < device.Actions.Length; i++)
                    {
                        Type actionType = Type.GetType(device.Actions[i].TypeName);

                        actions[i] = InputActionCreate.Create(actionType, device.Actions[i]);
                    }

                    inputDeviceGroup.AddDevice((InputDevice)Activator.CreateInstance(deviceType, device.Enable, actions));
                }

                m_InputDeviceGroups.Add(group.Name, inputDeviceGroup);
            }
        }
    }
}
