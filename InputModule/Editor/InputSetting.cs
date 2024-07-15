using GameFramework.Toolkit.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    /// <summary>
    /// 输入设置
    /// </summary>
    public static class InputSetting
    {
        public static InputEditorConfig EditorConfig;

        public static string Path;

        public const string PackageFullName = "com.rcy.gameframework-toolkit-inputmodule";
        public const string PackageDisplayName = "InputModule";

        static InputSetting()
        {
            Path = TypeUtility.GetConfigurationPath<InputConfigPathAttribute>();

            if (string.IsNullOrEmpty(Path))
            {
                //Path = Application.dataPath + "/InputEditorConfig.xml";
                Path = Utility.GetPackageRelativePath(PackageFullName, PackageDisplayName)
                       + "/Editor/InputEditorConfig.xml";
            }

            EditorConfig = InputConfig.ImportXmlConfig(Path);

            if (EditorConfig == null)
            {
                EditorConfig = new InputEditorConfig();
                EditorConfig.DeviceGroups = new List<InputGroupDataEditor>();
            }
        }

        /// <summary>
        /// 添加组
        /// </summary>
        /// <param name="groupName"></param>
        public static void AddGroup(string groupName)
        {
            if (EditorConfig.DeviceGroups == null)
            {
                Debug.LogError("DeviceGroups为空");
                return;
            }

            if (string.IsNullOrEmpty(groupName))
            {
                Debug.LogError("组名为空");
                return;
            }

            var group = GetGroup(groupName);

            if (group != null)
            {
                Debug.LogError("组名已存在");
                return;
            }

            var groupData = new InputGroupDataEditor();
            groupData.GroupName = groupName;
            groupData.CacheName = groupName;
            EditorConfig.DeviceGroups.Add(groupData);
        }

        /// <summary>
        /// 移除组
        /// </summary>
        /// <param name="group"></param>
        public static void RemoveGroup(InputGroupDataEditor group)
        {
            if (EditorConfig.DeviceGroups == null)
            {
                Debug.LogError("DeviceGroups为空");
                return;
            }

            if (group == null)
            {
                Debug.LogError("组不存在");
                return;
            }

            if(EditorConfig.DeviceGroups.Contains(group))
                EditorConfig.DeviceGroups.Remove(group);

        }

        /// <summary>
        /// 修改组名
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void ModifyGroupName(string oldName,string newName)
        {
            if (EditorConfig.DeviceGroups == null)
            {
                Debug.LogError("DeviceGroups为空");
                return;
            }

            var group = GetGroup(oldName);

            if (string.IsNullOrEmpty(newName))
            {
                Debug.LogError("组名为空");
                return;
            }

            if (group == null)
            {
                Debug.LogError("组名不存在");
                return;
            }

            var group2 = GetGroup(newName);

            if (group2 != null)
            {
                Debug.LogError("组名已存在");
                return;
            }

            group.GroupName = newName;

            Debug.Log("修改成功");
        }

        /// <summary>
        /// 导出输入配置
        /// </summary>
        /// <param name="path"></param>
        public static void ExportInputConfig(string path)
        {
            if (EditorConfig.DeviceGroups == null)
            {
                Debug.LogError("DeviceGroups为空");
                return;
            }

            JsonHelper.SetJsonHelper(new NewtonsoftJsonHelper());

            List<InputGroupData> groupData = new List<InputGroupData>();

            foreach (var group in EditorConfig.DeviceGroups)
            {
                groupData.Add(group.Export());
            }

            var json = JsonHelper.ToJson(groupData);

            File.WriteAllText(path, json);

            Debug.Log("输入配置导出成功");
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            InputConfig.ExportXmlConfig(Path, EditorConfig);

            Debug.Log("输入配置文件存储成功");
        }

        /// <summary>
        /// 获取组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static InputGroupDataEditor GetGroup(string name)
        {
            return EditorConfig.DeviceGroups.FirstOrDefault(g => g.GroupName == name);
        }
    }

    public class InputEditorConfig
    {
        public List<InputGroupDataEditor> DeviceGroups;
        public bool SaveOnExit;
    }
}
