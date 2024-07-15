using GameFramework.Toolkit.Runtime;
using System;
using System.Collections.Generic;

namespace GameFramework.Toolkit.Editor
{
    /// <summary>
    /// 输入设备数据
    /// </summary>
    [System.Serializable]
    public class InputDeviceDataEditor
    {
        public bool Enable;

        public bool Foldout;

        public List<InputActionDataEditor> Actions = new List<InputActionDataEditor>();

        public virtual Type BaseDataType { get; }

        public virtual string TypeName { get; }

        private HashSet<string> nameSet = new HashSet<string>();

        /// <summary>
        /// 获取行为
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public InputActionDataEditor GetAction(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("行为名不能为空");
            }

            if (Actions == null)
            {
                throw new Exception("行为列表为空");
            }

            var action = Actions.Find((a) => a.Name == name);

            return action;
        }

        /// <summary>
        /// 更改行为标识
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool RenameAction(string oldName,string newName)
        {
            if (oldName == newName)
            {
                return false;
            }

            var old_action = GetAction(oldName);

            if (old_action == null)
            {
                throw new Exception($"行为'{oldName}'不存在");
            }
            
            //新行为标识已存在
            if (nameSet.Contains(newName))
            {
                old_action.CacheName = old_action.Name;
                throw new Exception("行为标识不能相同");
            }
            nameSet.Remove(oldName);
            nameSet.Add(newName);
            old_action.Name = newName;

            return true;
        }

        /// <summary>
        /// 添加行为
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="actionName"></param>
        public void AddAction(Type type,string name,string actionName)
        {
            name = GetUniqueName(name);

            //var action = ScriptableObject.CreateInstance<InputActionDataEditor>();
            var action = (InputActionDataEditor)Activator.CreateInstance(type);
            
            action.ActionName = actionName;

            action.Name = name;
                
            action.CacheName = name;

            nameSet.Add(name);

            Actions.Add(action);
        }

        /// <summary>
        /// 移除行为
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="Exception"></exception>
        public void RemoveAction(InputActionDataEditor action)
        {
            if (action == null)
                throw new Exception($"移除失败：行为不能为空");
            
            nameSet.Remove(action.Name);
            Actions.Remove(action);
        }

        /// <summary>
        /// 获取唯一名
        /// </summary>
        /// <param name="baseName"></param>
        /// <returns></returns>
        public string GetUniqueName(string baseName)
        {
            int index = 1;
            string uniqueName = baseName;

            while (nameSet.Contains(uniqueName))
            {
                uniqueName = baseName + index.ToString();
                index++;
            }

            return uniqueName;
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        public void ResetData()
        {
            Enable = false;
            Actions = new List<InputActionDataEditor>();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <returns></returns>
        public virtual InputDeviceData Save()
        {
            InputDeviceData data = new InputDeviceData();

            data.Enable = Enable;
            data.Actions = new InputActionData[Actions.Count];
            data.TypeName = TypeName;

            for (int i = 0; i < Actions.Count; i++)
            {
                data.Actions[i] = Actions[i].Save();
            }

            return data;
        }
    }

    public class InputDeviceData<T> : InputDeviceDataEditor where T : InputActionDataEditor
    {
        public override Type BaseDataType
        {
            get
            {
                return typeof(T);
            }
        }
    }
}
