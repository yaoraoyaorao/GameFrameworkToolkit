using GameFramework.Toolkit.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public class ExcelToolMenu : EditorWindow
    {
        private ExcelToolData m_Data;
        private Vector2 m_ScrollView;
        private IExcelFormatBuilder m_Rule;
        private IExcelDataBuilder[] m_DataBuilders;
        private string[] m_RuleNames;
        private string[] m_TargetDataNames;


        [MenuItem("GameFramework/Excel����")]
        public static void ShowExample()
        {
            ExcelToolMenu wnd = GetWindow<ExcelToolMenu>();
            wnd.titleContent = new GUIContent("ExcelToolMenu");
            wnd.minSize = new Vector2(800, 420);
        }

        private void OnEnable()
        {
            m_Data = ExcelToolAsset.GetAsset;

            m_RuleNames = Utility.GetTypeNames(typeof(IExcelFormatBuilder), ExcelUtility.RuntimeOrEditorAssemblyNames);
            m_TargetDataNames = Utility.GetTypeNames(typeof(IExcelDataBuilder), ExcelUtility.RuntimeOrEditorAssemblyNames);
            
            RefreshRule();
            RefreshTargetData();
            RefreshDataBuilder();
        }

        private void OnDisable()
        {
            if (m_Data != null)
            {
                EditorUtility.SetDirty(m_Data);
            }
        }

        private void OnGUI()
        {
            if (m_Data == null)
            {
                return;
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
               
                EditorGUILayout.LabelField("Excel��·��*", GUILayout.Width(70));
                GUI.enabled = false;
                EditorGUILayout.TextField(m_Data.ExcelPath);
                GUI.enabled = true;

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.importExcel, "����Excel��Ŀ¼"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    SelectPath();
                }

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.refresh, "���µ���Excel"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    RefreshList();
                }
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                EditorGUILayout.LabelField("����·��*", GUILayout.Width(70));

                GUI.enabled = false;
                EditorGUILayout.TextField(m_Data.CSharpPath);
                GUI.enabled = true;

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.csharp, "����C#����Ŀ¼"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    m_Data.CSharpPath = OpenFolderPanel(m_Data.CSharpPath, "ѡ��C#�����ļ���");
                }
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                EditorGUILayout.LabelField("����·��*", GUILayout.Width(70));

                GUI.enabled = false;
                EditorGUILayout.TextField(m_Data.SaveDataPath);
                GUI.enabled = true;

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.data, "��������Ŀ¼"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    m_Data.SaveDataPath = OpenFolderPanel(m_Data.SaveDataPath, "ѡ��C#�����ļ���");
                }
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                EditorGUILayout.LabelField("�����ռ�*", GUILayout.Width(70));
                m_Data.NameSpace = EditorGUILayout.TextField(m_Data.NameSpace, GUILayout.Width(100));

                EditorGUILayout.LabelField("��ǰ׺", GUILayout.Width(70));
                m_Data.ClassPrefix = EditorGUILayout.TextField(m_Data.ClassPrefix, GUILayout.Width(100));
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                EditorGUILayout.LabelField("���ɹ���", GUILayout.Width(60));
                m_Data.RuleIndex = EditorGUILayout.Popup(m_Data.RuleIndex, m_RuleNames, EditorStyles.toolbarDropDown, GUILayout.Width(200));

                EditorGUILayout.LabelField("Ŀ�����ݣ�", GUILayout.Width(60));
                m_Data.DataBuilderIndex = EditorGUILayout.MaskField(m_Data.DataBuilderIndex, m_TargetDataNames, EditorStyles.toolbarDropDown, GUILayout.Width(200));
            }

            EditorGUILayout.Space(10);

            using (new EditorGUILayout.VerticalScope(GUILayout.MinHeight(250)))
            {
                m_ScrollView = EditorGUILayout.BeginScrollView(m_ScrollView,"framebox");
                DrawList();
                EditorGUILayout.EndScrollView();
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                if (GUILayout.Button(new GUIContent(ExcelToolStyles.selectAll, "ȫѡ"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    m_Data.SelectAll();
                }

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.disselectAll, "ȫ��ѡ"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    m_Data.DisSelectAll();
                }

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.invertSelect, "��ѡ"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    m_Data.InvertSelect();
                }

                using (new EditorGUI.DisabledGroupScope(m_Data.SelectCount == 0))
                {
                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.allConvert, "ת����ǰ��ѡExcel"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                    {
                        ConvertSelectAll();
                    }

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.remove, "�Ƴ���ǰ��ѡExcel"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                    {
                        RemoveSelectAll();
                    }

                    EditorGUILayout.LabelField($"��ѡ��:{m_Data.SelectCount}", GUILayout.Width(70));
                }

            }
        }

        /// <summary>
        /// �����б�
        /// </summary>
        private void DrawList()
        {
            if (m_Data.ExcelList == null)
                return;

            if (!Directory.Exists(m_Data.ExcelPath))
            {
                EditorGUILayout.HelpBox("Excel�ļ��в�����", MessageType.Error);
                return;
            }

            if (string.IsNullOrEmpty(m_Data.ExcelPath) || m_Data.ExcelPath == "/")
            {
                return;
            }

            if (m_Data.ExcelList.Count == 0)
            {
                EditorGUILayout.HelpBox("Excel�ļ�������", MessageType.Warning);
                return;
            }
            ExcelItem excelItem = null;
            for (int i = 0; i < m_Data.ExcelList.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope("GroupBox"))
                {
                    excelItem = m_Data.ExcelList[i];
                    string path = excelItem.FullName;
                    string name = excelItem.Name;

                    excelItem.IsSelect = EditorGUILayout.Toggle(excelItem.IsSelect, GUILayout.Width(30));
                    EditorGUILayout.LabelField(name, EditorStyles.boldLabel, GUILayout.Width(100));
                    EditorGUILayout.LabelField(path);

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.convert, "ת����ǰExcel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        ConvertSingle(path);
                    }

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.open, "�򿪵�ǰExcel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        OpenFile(path);
                    }

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.remove, "�Ƴ���ǰExcel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        m_Data.ExcelList.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// ѡ��Excel�ļ���
        /// </summary>
        private void SelectPath()
        {
            m_Data.ExcelPath = OpenFolderPanel(m_Data.ExcelPath,"ѡ��Excel�ļ���");

            RefreshList();
        }

        private string OpenFolderPanel(string rawPath,string title)
        {
            string path = EditorUtility.OpenFolderPanel(title, rawPath, "");
            if(string.IsNullOrEmpty(path))
            {
                return rawPath;
            }

            return path;
        }

        /// <summary>
        /// ˢ���б�
        /// </summary>
        private void RefreshList()
        {
            if (string.IsNullOrEmpty(m_Data.ExcelPath) || m_Data.ExcelPath == "/")
            {
                return;
            }

            m_Data.ExcelList.Clear();

            string[] excelFiles = Directory.GetFiles(m_Data.ExcelPath, "*.xlsx", SearchOption.AllDirectories);

            foreach (var item in excelFiles)
            {
                m_Data.ExcelList.Add(new ExcelItem()
                {
                    Name = Path.GetFileName(item),
                    FullName = item
                });
            }
        }

        /// <summary>
        /// ˢ�¹���
        /// </summary>
        private void RefreshRule()
        {
            if (m_RuleNames == null || m_RuleNames.Length == 1)
            {
                m_Rule = null;
                return;
            }

            string ruleType = m_RuleNames[m_Data.RuleIndex];

            if (ruleType != "<None>")
            {
                Type type = AssemblyUtility.GetType(ruleType);
                m_Rule = (IExcelFormatBuilder)Activator.CreateInstance(type);
            }
        }

        /// <summary>
        /// ˢ�����ݹ�����
        /// </summary>
        private void RefreshDataBuilder()
        {
            if(m_TargetDataNames == null || m_TargetDataNames.Length == 0)
            {
                m_DataBuilders = null;
                return;
            }
            List<IExcelDataBuilder> datas = new List<IExcelDataBuilder>();
            for (int i = 0; i < m_TargetDataNames.Length; i++)
            {
                if ((m_Data.DataBuilderIndex & (1 << i)) != 0)
                {
                    if (m_TargetDataNames[i] != "<None>")
                    {
                        Type type = AssemblyUtility.GetType(m_TargetDataNames[i]);
                        datas.Add((IExcelDataBuilder)Activator.CreateInstance(type));
                    }
                }
            }

            if (datas.Count > 0)
            {
                m_DataBuilders = datas.ToArray();

                return;
            }

            m_DataBuilders = null;
        }

        /// <summary>
        /// ˢ��Ŀ������
        /// </summary>
        private void RefreshTargetData()
        {

        }

        /// <summary>
        /// ��Excel�ļ�
        /// </summary>
        private void OpenFile(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            System.Diagnostics.Process.Start(path);
        }

        /// <summary>
        /// ת������Excel�ļ�
        /// </summary>
        /// <param name="path"></param>
        private void ConvertSingle(string path)
        {
            bool ok = EditorUtility.DisplayDialog("��ʾ", "ȷ��Ҫת����ǰ�ļ���", "ȷ��", "ȡ��");
            if (ok)
            {
                Convert(path);
            }
        }

        /// <summary>
        /// ת��Excel�ļ�
        /// </summary>
        private void ConvertSelectAll()
        {
            bool ok = EditorUtility.DisplayDialog("��ʾ", "ȷ��Ҫת����ǰ��ѡ�ļ���", "ȷ��", "ȡ��");
            if (ok)
            {
                foreach (var excelItem in m_Data.GetSelectItem())
                {
                    Convert(excelItem.FullName);
                }
            }
        }

        private void Convert(string path)
        {
            if (!CheckData())
            {
                return;
            }

            RefreshRule();
            RefreshDataBuilder();

            if (m_Rule == null || m_DataBuilders == null)
            {
                return;
            }

            ExcelUtility.Read(path, m_Rule, m_DataBuilders);
        }

        /// <summary>
        /// �Ƴ���ѡExcel�ļ�
        /// </summary>
        private void RemoveSelectAll()
        {
            bool ok = EditorUtility.DisplayDialog("����", "ȷ��Ҫ�Ƴ���ѡExcel�ļ���", "ȷ��", "ȡ��");
            if (ok)
            {
                m_Data.RemoveSelect();
            }
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            if (m_Data == null)
            {
                Debug.LogError("���ݲ���Ϊ��");

                return false;
            }

            if (string.IsNullOrWhiteSpace(m_Data.NameSpace))
            {
                Debug.LogError("�����ռ䲻��Ϊ��");
                return false;
            }

            if (!Directory.Exists(m_Data.SaveDataPath))
            {
                Debug.LogError("�洢�����ļ��в�����");
                return false;
            }

            if (!Directory.Exists(m_Data.CSharpPath))
            {
                Debug.LogError("�洢�����ļ��в�����");
                return false;
            }

            return true;
        }
    }
}
