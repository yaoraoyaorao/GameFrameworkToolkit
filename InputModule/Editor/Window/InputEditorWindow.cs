using GameFramework.Toolkit.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public class InputEditorWindow : EditorWindow
    {
        private static InputEditorWindow m_Instance;

        private string m_GroupName;
        private Vector2 m_ScrollView;
        private List<InputGroupDataEditor> groupData;

        [MenuItem("GameFramework/输入模块")]
        public static void Open()
        {
            if (m_Instance == null)
            {
                m_Instance = GetWindow<InputEditorWindow>("输入模块");
            }
        }
         
        private void OnEnable()
        {
            groupData = InputSetting.EditorConfig.DeviceGroups;
        }

        private void OnDestroy()
        {
            if (InputSetting.EditorConfig.SaveOnExit)
            {
                InputSetting.Save();
            }
            
        }

        private void OnGUI()
        {
            if (groupData == null)
            {
                EditorGUILayout.HelpBox("配置文件缺失，请创建配置文件", MessageType.Error);
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                EditorGUILayout.LabelField("退出时保存", GUILayout.Width(65));
                InputSetting.EditorConfig.SaveOnExit = EditorGUILayout.Toggle(InputSetting.EditorConfig.SaveOnExit, GUILayout.Width(20));

                if (GUILayout.Button("导出", EditorStyles.miniButtonMid, GUILayout.Width(80)))
                {
                    string filePath = EditorUtility.SaveFilePanel("导出配置文件", Application.dataPath, "InputConfig.json", "json");

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        InputSetting.ExportInputConfig(filePath);
                    }
                    
                } 
                
                if (GUILayout.Button("保存", EditorStyles.miniButtonMid, GUILayout.Width(80)))
                {
                    InputSetting.Save();
                }
            }

            using (new EditorGUILayout.HorizontalScope("framebox"))
            {
                EditorGUILayout.LabelField("组名:", GUILayout.Width(40));
                m_GroupName = EditorGUILayout.TextField(m_GroupName);
                if (GUILayout.Button("添加",EditorStyles.miniButtonRight, GUILayout.Width(80)))
                {
                    AddGroup();
                }
            }

            if (groupData == null || groupData.Count == 0)
            {
                EditorGUILayout.HelpBox("设备组为空", MessageType.Info);
                return;
            }

            m_ScrollView = EditorGUILayout.BeginScrollView(m_ScrollView);
            {
                for (int i = 0; i < groupData.Count; i++)
                {
                    DrawGroup(groupData[i]);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// 绘制组
        /// </summary>
        /// <param name="deviceGroup"></param>
        /// <param name="index"></param>
        private void DrawGroup(InputGroupDataEditor deviceGroup)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("组名:", GUILayout.Width(40));
                    EditorGUILayout.LabelField(deviceGroup.GroupName, GUILayout.Width(100));

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField("修改组名:", GUILayout.Width(55));
                    deviceGroup.CacheName = EditorGUILayout.TextField(deviceGroup.CacheName, GUILayout.Width(100));

                    if (GUILayout.Button("修改", EditorStyles.miniButtonMid, GUILayout.Width(60)))
                    {
                        bool ok = EditorUtility.DisplayDialog("提示", "是否修改组名", "确定", "取消");
                        if (ok)
                        {
                            InputSetting.ModifyGroupName(deviceGroup.GroupName, deviceGroup.CacheName);
                        }
                    }

                    if (GUILayout.Button("移除", EditorStyles.miniButtonMid, GUILayout.Width(60)))
                    {
                        bool ok = EditorUtility.DisplayDialog("提示", "是否当前组", "确定", "取消");
                        if (ok)
                        {
                            InputSetting.RemoveGroup(deviceGroup);
                        }
                    }

                    if (GUILayout.Button(deviceGroup.Enable ? "禁用" : "启用", EditorStyles.miniButtonMid, GUILayout.Width(60)))
                    {
                        deviceGroup.Enable = !deviceGroup.Enable;
                    }

                    using (new EditorGUI.DisabledGroupScope(!deviceGroup.Enable))
                    {
                        if (GUILayout.Button("添加设备", EditorStyles.miniButtonMid, GUILayout.Width(60)))
                        {
                            var count = AssemblyUtility.GetAllTypesDerivedFrom<InputDeviceDataEditor>();
                            var menu = new GenericMenu();
                            foreach (var type in count)
                            {
                                if (type.Name == "InputDeviceData`1")
                                {
                                    continue;
                                }
                                var title = EditorUtilities.GetContent(type.Name);
                                bool exists = deviceGroup.HasDevice(type);
                                if (!exists)
                                    menu.AddItem(title, false, () => deviceGroup.AddDevice(type));
                                else
                                    menu.AddDisabledItem(title);
                            }
                            menu.ShowAsContext();
                        }
                    }

                }

                using (new EditorGUI.DisabledGroupScope(!deviceGroup.Enable))
                {

                    if (deviceGroup.InputDevices == null || deviceGroup.InputDevices.Count == 0)
                    {
                        EditorGUILayout.HelpBox("设备为空", MessageType.Info);
                        return;
                    }
                    EditorGUILayout.Space(10);
                    for (int i = 0; i < deviceGroup.InputDevices.Count; i++)
                    {
                        //绘制设备
                        DrawDevice(deviceGroup, deviceGroup.InputDevices[i]);
                    }
                }

            }
        }

        /// <summary>
        /// 绘制设备
        /// </summary>
        /// <param name="device"></param>
        private void DrawDevice(InputGroupDataEditor deviceGroup,InputDeviceDataEditor device)
        {
            if (device == null)
            {
                return;
            }

            bool fololdout = DrawHeader(deviceGroup, device);

            if (fololdout)
            {
                if (device.Actions == null || device.Actions.Count == 0)
                {
                    EditorGUILayout.HelpBox("行为为空", MessageType.Info);
                    return;
                }

                using (new EditorGUI.DisabledGroupScope(!device.Enable))
                {
                    for (int i = 0; i < device.Actions.Count; i++)
                    {
                        DrawAction(device.Actions[i], device);
                    }
                    
                }
            }
        }
         
        /// <summary>
        /// 绘制设备头
        /// </summary>
        /// <param name="deviceGroup">设备组</param>
        /// <param name="device">设备</param>
        /// <returns></returns>
        private bool DrawHeader(InputGroupDataEditor deviceGroup,InputDeviceDataEditor device)
        {
            var backgroundRect = GUILayoutUtility.GetRect(0f, 17f);

            //标题区域
            var labelRect = backgroundRect;
            labelRect.xMin += 32f;
            labelRect.xMax -= 20f;

            //折叠按钮区域
            var foldoutRect = backgroundRect;
            foldoutRect.y += 1f;
            foldoutRect.width = 13f;
            foldoutRect.height = 13f;

            //toggle区域
            var toggleRect = backgroundRect;
            toggleRect.x += 16f;
            toggleRect.y += 2f;
            toggleRect.width = 13f;
            toggleRect.height = 13f;

            //菜单图标
            var menuIcon = CustomEditorStyle.paneOptionsIcon;

            //菜单区域
            var menuRect = new Rect(labelRect.xMax + 4f, labelRect.y, menuIcon.width, menuIcon.height);

            backgroundRect.xMin = 0f;
            backgroundRect.width += 4f;

            EditorGUI.DrawRect(backgroundRect, CustomEditorStyle.headerBackground);
            EditorGUI.LabelField(labelRect, EditorUtilities.GetContent(device.GetType().Name), EditorStyles.boldLabel);

            device.Foldout = GUI.Toggle(foldoutRect, device.Foldout, GUIContent.none, EditorStyles.foldout);
            device.Enable = GUI.Toggle(toggleRect, device.Enable, GUIContent.none, CustomEditorStyle.smallTickbox);

            GUI.DrawTexture(menuRect, menuIcon);

            var e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                if (menuRect.Contains(e.mousePosition))
                {
                    ShowHeaderContextMenu(new Vector2(menuRect.x, menuRect.yMax), deviceGroup, device);
                    e.Use();
                }
                else if (labelRect.Contains(e.mousePosition))
                {
                    if (e.button == 0)
                    {
                        device.Foldout = !device.Foldout;
                    }
                    else
                    {
                        ShowHeaderContextMenu(e.mousePosition, deviceGroup, device);
                    }

                    e.Use();

                }
            }

            return device.Foldout;
        }

        /// <summary>
        /// 绘制行为
        /// </summary>
        /// <param name="action">行为</param>
        /// <param name="device">设备</param>
        private void DrawAction(InputActionDataEditor action, InputDeviceDataEditor device)
        {
            if (action == null)
            {
                return;
            }

            

            EditorGUILayout.BeginVertical("GroupBox");

            Rect backgroundRect = GUILayoutUtility.GetRect(0f, 20f);

            //标题区域
            var labelRect = backgroundRect;
            labelRect.xMin += 32f;
            labelRect.xMax -= 20f;

            //折叠按钮区域
            var foldoutRect = backgroundRect;
            foldoutRect.y += 1f;
            foldoutRect.width = 13f;
            foldoutRect.height = 13f;

            EditorGUI.DrawRect(backgroundRect, CustomEditorStyle.headerBackground);

            action.Foldout = GUI.Toggle(foldoutRect, action.Foldout, GUIContent.none, EditorStyles.foldout);

            if (action.Foldout)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("行为", GUILayout.Width(80));
                    EditorGUILayout.LabelField(action.ActionName);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("标识", GUILayout.Width(80));
                    EditorGUILayout.LabelField(action.Name);

                    action.CacheName = EditorGUILayout.TextField(action.CacheName, GUILayout.Width(150));
                    if (GUILayout.Button("修改", EditorStyles.miniButtonMid, GUILayout.Width(50)))
                    {
                        try
                        {
                            bool change = device.RenameAction(action.Name, action.CacheName);

                            if (change)
                                Debug.Log("修改成功");
                        }
                        catch (Exception ex)
                        {

                            Debug.LogError(ex.Message);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("启用", GUILayout.Width(80));
                    action.Enable = EditorGUILayout.Toggle(action.Enable);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.LabelField("Override", EditorStyles.boldLabel);

                FieldInfo[] fields = action.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    if (field.Name == "ActionName" ||
                        field.Name == "Name" ||
                        field.Name == "Enable")
                    {
                        continue;
                    }

                    EditorGUI.BeginDisabledGroup(!action.Enable);
                    EditorUtilities.DrawItem(action, field);
                    EditorGUI.EndDisabledGroup();
                }
            }
            else
            {
                EditorGUI.LabelField(labelRect, EditorUtilities.GetContent($"行为:{action.ActionName}  标识:{action.Name}"), EditorStyles.boldLabel);
            }
            EditorGUILayout.EndVertical();

            var lastRect = GUILayoutUtility.GetLastRect();

            var e = Event.current;

            if (e.type == EventType.MouseDown)
            {
                if (e.button == 1)
                {
                    if (lastRect.Contains(e.mousePosition))
                    {
                        var menu = new GenericMenu();
                        menu.AddItem(EditorUtilities.GetContent("移除行为"), false, () => RemoveAction(device, action));
                        menu.DropDown(new Rect(e.mousePosition, Vector2.zero));
                    }
                }
            }
        }

        /// <summary>
        /// 显示头上下文菜单
        /// </summary>
        /// <param name="position">菜单位置</param>
        /// <param name="deviceGroup">设备组</param>
        /// <param name="device">设备</param>
        private void ShowHeaderContextMenu(Vector2 position, InputGroupDataEditor deviceGroup, InputDeviceDataEditor device)
        {
            var menu = new GenericMenu();
            menu.AddItem(EditorUtilities.GetContent("重置"), false, () => device.ResetData());
            menu.AddItem(EditorUtilities.GetContent("移除"), false, () => deviceGroup.RemoveDevice(device));
            menu.AddSeparator(string.Empty);


            var count = AssemblyUtility.GetAllTypesDerivedFrom(device.BaseDataType);

            if (count == null || count.Count() == 0)
            {
                menu.AddDisabledItem(EditorUtilities.GetContent("行为缺失"));
            }
            else
            {
                foreach (var type in count)
                {
                    var attribute = (InputDataNameAttribute)type.GetCustomAttributes(typeof(InputDataNameAttribute), false)[0];
                    var title = "行为/" + attribute.Name;
                    menu.AddItem(EditorUtilities.GetContent(title), false, () => AddAction(type, device, attribute.Name));
                }
            }
            menu.DropDown(new Rect(position, Vector2.zero));
        }

        /// <summary>
        /// 添加行为
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="device"></param>
        /// <param name="actionName"></param>
        private void AddAction(Type dataType, InputDeviceDataEditor device,string actionName)
        {
            device.AddAction(dataType, "ActionName", actionName);
        }

        /// <summary>
        /// 移除行为
        /// </summary>
        /// <param name="device"></param>
        /// <param name="actionName"></param>
        private void RemoveAction(InputDeviceDataEditor device, InputActionDataEditor actionName)
        {
            device.RemoveAction(actionName);
        }

        /// <summary>
        /// 添加组
        /// </summary>
        private void AddGroup()
        {
            bool ok = EditorUtility.DisplayDialog("提示", "是否添加新组", "确定", "取消");

            if (!ok) return;

            InputSetting.AddGroup(m_GroupName);

            m_GroupName = "";
        }
    }
}

