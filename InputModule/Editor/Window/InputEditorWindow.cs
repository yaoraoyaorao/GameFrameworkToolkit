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

        [MenuItem("GameFramework/����ģ��")]
        public static void Open()
        {
            if (m_Instance == null)
            {
                m_Instance = GetWindow<InputEditorWindow>("����ģ��");
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
                EditorGUILayout.HelpBox("�����ļ�ȱʧ���봴�������ļ�", MessageType.Error);
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                EditorGUILayout.LabelField("�˳�ʱ����", GUILayout.Width(65));
                InputSetting.EditorConfig.SaveOnExit = EditorGUILayout.Toggle(InputSetting.EditorConfig.SaveOnExit, GUILayout.Width(20));

                if (GUILayout.Button("����", EditorStyles.miniButtonMid, GUILayout.Width(80)))
                {
                    string filePath = EditorUtility.SaveFilePanel("���������ļ�", Application.dataPath, "InputConfig.json", "json");

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        InputSetting.ExportInputConfig(filePath);
                    }
                    
                } 
                
                if (GUILayout.Button("����", EditorStyles.miniButtonMid, GUILayout.Width(80)))
                {
                    InputSetting.Save();
                }
            }

            using (new EditorGUILayout.HorizontalScope("framebox"))
            {
                EditorGUILayout.LabelField("����:", GUILayout.Width(40));
                m_GroupName = EditorGUILayout.TextField(m_GroupName);
                if (GUILayout.Button("���",EditorStyles.miniButtonRight, GUILayout.Width(80)))
                {
                    AddGroup();
                }
            }

            if (groupData == null || groupData.Count == 0)
            {
                EditorGUILayout.HelpBox("�豸��Ϊ��", MessageType.Info);
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
        /// ������
        /// </summary>
        /// <param name="deviceGroup"></param>
        /// <param name="index"></param>
        private void DrawGroup(InputGroupDataEditor deviceGroup)
        {
            using (new EditorGUILayout.VerticalScope("box"))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField("����:", GUILayout.Width(40));
                    EditorGUILayout.LabelField(deviceGroup.GroupName, GUILayout.Width(100));

                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField("�޸�����:", GUILayout.Width(55));
                    deviceGroup.CacheName = EditorGUILayout.TextField(deviceGroup.CacheName, GUILayout.Width(100));

                    if (GUILayout.Button("�޸�", EditorStyles.miniButtonMid, GUILayout.Width(60)))
                    {
                        bool ok = EditorUtility.DisplayDialog("��ʾ", "�Ƿ��޸�����", "ȷ��", "ȡ��");
                        if (ok)
                        {
                            InputSetting.ModifyGroupName(deviceGroup.GroupName, deviceGroup.CacheName);
                        }
                    }

                    if (GUILayout.Button("�Ƴ�", EditorStyles.miniButtonMid, GUILayout.Width(60)))
                    {
                        bool ok = EditorUtility.DisplayDialog("��ʾ", "�Ƿ�ǰ��", "ȷ��", "ȡ��");
                        if (ok)
                        {
                            InputSetting.RemoveGroup(deviceGroup);
                        }
                    }

                    if (GUILayout.Button(deviceGroup.Enable ? "����" : "����", EditorStyles.miniButtonMid, GUILayout.Width(60)))
                    {
                        deviceGroup.Enable = !deviceGroup.Enable;
                    }

                    using (new EditorGUI.DisabledGroupScope(!deviceGroup.Enable))
                    {
                        if (GUILayout.Button("����豸", EditorStyles.miniButtonMid, GUILayout.Width(60)))
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
                        EditorGUILayout.HelpBox("�豸Ϊ��", MessageType.Info);
                        return;
                    }
                    EditorGUILayout.Space(10);
                    for (int i = 0; i < deviceGroup.InputDevices.Count; i++)
                    {
                        //�����豸
                        DrawDevice(deviceGroup, deviceGroup.InputDevices[i]);
                    }
                }

            }
        }

        /// <summary>
        /// �����豸
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
                    EditorGUILayout.HelpBox("��ΪΪ��", MessageType.Info);
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
        /// �����豸ͷ
        /// </summary>
        /// <param name="deviceGroup">�豸��</param>
        /// <param name="device">�豸</param>
        /// <returns></returns>
        private bool DrawHeader(InputGroupDataEditor deviceGroup,InputDeviceDataEditor device)
        {
            var backgroundRect = GUILayoutUtility.GetRect(0f, 17f);

            //��������
            var labelRect = backgroundRect;
            labelRect.xMin += 32f;
            labelRect.xMax -= 20f;

            //�۵���ť����
            var foldoutRect = backgroundRect;
            foldoutRect.y += 1f;
            foldoutRect.width = 13f;
            foldoutRect.height = 13f;

            //toggle����
            var toggleRect = backgroundRect;
            toggleRect.x += 16f;
            toggleRect.y += 2f;
            toggleRect.width = 13f;
            toggleRect.height = 13f;

            //�˵�ͼ��
            var menuIcon = CustomEditorStyle.paneOptionsIcon;

            //�˵�����
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
        /// ������Ϊ
        /// </summary>
        /// <param name="action">��Ϊ</param>
        /// <param name="device">�豸</param>
        private void DrawAction(InputActionDataEditor action, InputDeviceDataEditor device)
        {
            if (action == null)
            {
                return;
            }

            

            EditorGUILayout.BeginVertical("GroupBox");

            Rect backgroundRect = GUILayoutUtility.GetRect(0f, 20f);

            //��������
            var labelRect = backgroundRect;
            labelRect.xMin += 32f;
            labelRect.xMax -= 20f;

            //�۵���ť����
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
                    EditorGUILayout.LabelField("��Ϊ", GUILayout.Width(80));
                    EditorGUILayout.LabelField(action.ActionName);
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("��ʶ", GUILayout.Width(80));
                    EditorGUILayout.LabelField(action.Name);

                    action.CacheName = EditorGUILayout.TextField(action.CacheName, GUILayout.Width(150));
                    if (GUILayout.Button("�޸�", EditorStyles.miniButtonMid, GUILayout.Width(50)))
                    {
                        try
                        {
                            bool change = device.RenameAction(action.Name, action.CacheName);

                            if (change)
                                Debug.Log("�޸ĳɹ�");
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
                    EditorGUILayout.LabelField("����", GUILayout.Width(80));
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
                EditorGUI.LabelField(labelRect, EditorUtilities.GetContent($"��Ϊ:{action.ActionName}  ��ʶ:{action.Name}"), EditorStyles.boldLabel);
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
                        menu.AddItem(EditorUtilities.GetContent("�Ƴ���Ϊ"), false, () => RemoveAction(device, action));
                        menu.DropDown(new Rect(e.mousePosition, Vector2.zero));
                    }
                }
            }
        }

        /// <summary>
        /// ��ʾͷ�����Ĳ˵�
        /// </summary>
        /// <param name="position">�˵�λ��</param>
        /// <param name="deviceGroup">�豸��</param>
        /// <param name="device">�豸</param>
        private void ShowHeaderContextMenu(Vector2 position, InputGroupDataEditor deviceGroup, InputDeviceDataEditor device)
        {
            var menu = new GenericMenu();
            menu.AddItem(EditorUtilities.GetContent("����"), false, () => device.ResetData());
            menu.AddItem(EditorUtilities.GetContent("�Ƴ�"), false, () => deviceGroup.RemoveDevice(device));
            menu.AddSeparator(string.Empty);


            var count = AssemblyUtility.GetAllTypesDerivedFrom(device.BaseDataType);

            if (count == null || count.Count() == 0)
            {
                menu.AddDisabledItem(EditorUtilities.GetContent("��Ϊȱʧ"));
            }
            else
            {
                foreach (var type in count)
                {
                    var attribute = (InputDataNameAttribute)type.GetCustomAttributes(typeof(InputDataNameAttribute), false)[0];
                    var title = "��Ϊ/" + attribute.Name;
                    menu.AddItem(EditorUtilities.GetContent(title), false, () => AddAction(type, device, attribute.Name));
                }
            }
            menu.DropDown(new Rect(position, Vector2.zero));
        }

        /// <summary>
        /// �����Ϊ
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="device"></param>
        /// <param name="actionName"></param>
        private void AddAction(Type dataType, InputDeviceDataEditor device,string actionName)
        {
            device.AddAction(dataType, "ActionName", actionName);
        }

        /// <summary>
        /// �Ƴ���Ϊ
        /// </summary>
        /// <param name="device"></param>
        /// <param name="actionName"></param>
        private void RemoveAction(InputDeviceDataEditor device, InputActionDataEditor actionName)
        {
            device.RemoveAction(actionName);
        }

        /// <summary>
        /// �����
        /// </summary>
        private void AddGroup()
        {
            bool ok = EditorUtility.DisplayDialog("��ʾ", "�Ƿ��������", "ȷ��", "ȡ��");

            if (!ok) return;

            InputSetting.AddGroup(m_GroupName);

            m_GroupName = "";
        }
    }
}

