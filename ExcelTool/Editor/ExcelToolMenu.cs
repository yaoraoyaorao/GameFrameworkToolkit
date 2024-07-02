using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public class ExcelToolMenu : EditorWindow
    {
        private ExcelToolData m_Data;
        private Vector2 m_ScrollView;
        private IExcelReadRule m_Rule;
        
        private const string key = "GameFramework.Toolkit.Editor.ExcelToolData";

        [MenuItem("GameFramework/ExcelToolMenu")]
        public static void ShowExample()
        {
            ExcelToolMenu wnd = GetWindow<ExcelToolMenu>();
            wnd.titleContent = new GUIContent("ExcelToolMenu");
        }

        private void OnEnable()
        {
            string url = ExcelUtility.GetPath() + "/Editor/ExcelToolData.asset";

            m_Data = AssetDatabase.LoadAssetAtPath<ExcelToolData>(url);

            if (m_Data == null)
            {

                m_Data = CreateInstance<ExcelToolData>();
                AssetDatabase.CreateAsset(m_Data, url);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            m_Rule = new DefaultRule();
        }

        private void OnDisable()
        {
            string jsonStr = JsonUtility.ToJson(m_Data);

            EditorPrefs.SetString(key, jsonStr);
        }

        private void OnGUI()
        {
            if (m_Data == null)
            {
                return;
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                if (GUILayout.Button(new GUIContent(ExcelToolStyles.importExcel, "����Excel��Ŀ¼"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    SelectPath();
                }

                EditorGUILayout.LabelField("Excel��·��", GUILayout.Width(70));

                GUI.enabled = false;
                EditorGUILayout.TextField(m_Data.m_ExcelPath);
                GUI.enabled = true;

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.allConvert, "ת����ǰ����Excel"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    ConvertAll();
                }

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.refresh, "���µ���Excel"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    RefreshList();
                }
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                if (GUILayout.Button(new GUIContent(ExcelToolStyles.csharp, "����C#����Ŀ¼"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {

                }

                EditorGUILayout.LabelField("����·��", GUILayout.Width(70));

                GUI.enabled = false;
                EditorGUILayout.TextField(m_Data.m_CSharpPath);
                GUI.enabled = true;

                
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                EditorGUILayout.LabelField("ɸѡ����", GUILayout.Width(60));
                EditorGUILayout.Popup(0, new string[] { "<None>" }, EditorStyles.toolbarDropDown, GUILayout.Width(200));

                EditorGUILayout.LabelField("Ŀ�����ݣ�", GUILayout.Width(60));
                EditorGUILayout.Popup(0, new string[] { "<None>" }, EditorStyles.toolbarDropDown, GUILayout.Width(200));
            }



            EditorGUILayout.Space(10);

            using (new EditorGUILayout.VerticalScope(GUILayout.Height(250)))
            {
                m_ScrollView = EditorGUILayout.BeginScrollView(m_ScrollView,"framebox");
                DrawList();
                EditorGUILayout.EndScrollView();
            }


        }

        /// <summary>
        /// ѡ��Excel�ļ���
        /// </summary>
        private void SelectPath()
        {
            m_Data.m_ExcelPath = EditorUtility.OpenFolderPanel("ѡ��Excel�ļ���", "", "");

            RefreshList();
        }

        /// <summary>
        /// ˢ���б�
        /// </summary>
        private void RefreshList()
        {
            if (string.IsNullOrEmpty(m_Data.m_ExcelPath) || m_Data.m_ExcelPath == "/")
            {
                return;
            }

            m_Data.m_ExcelList.Clear();

            string[] excelFiles = Directory.GetFiles(m_Data.m_ExcelPath, "*.xlsx", SearchOption.AllDirectories);

            foreach (var item in excelFiles)
            {
                m_Data.m_ExcelList.Add(item);
            }
        }

        /// <summary>
        /// ��Excel�ļ�
        /// </summary>
        private void OpenExcelFile(string path)
        {
            if (!File.Exists(path))
            {
                return;
            }

            System.Diagnostics.Process.Start(path);
        }

        /// <summary>
        /// �����б�
        /// </summary>
        private void DrawList()
        {
            if (m_Data.m_ExcelList == null)
                return;

            if (!Directory.Exists(m_Data.m_ExcelPath))
            {
                EditorGUILayout.HelpBox("Excel�ļ��в�����", MessageType.Error);
                return;
            }

            if (string.IsNullOrEmpty(m_Data.m_ExcelPath) || m_Data.m_ExcelPath == "/")
            {
                return;
            }

            if (m_Data.m_ExcelList.Count == 0)
            {
                EditorGUILayout.HelpBox("Excel�ļ�������", MessageType.Warning);
                return;
            }

            for (int i = 0; i < m_Data.m_ExcelList.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    string path = m_Data.m_ExcelList[i];

                    EditorGUILayout.LabelField(path);

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.convert,"ת����ǰExcel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        ConvertSingle(path);
                    }

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.open,"�򿪵�ǰExcel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        OpenExcelFile(path);
                    }

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.remove,"�Ƴ���ǰExcel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        m_Data.m_ExcelList.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// ת������Excel�ļ�
        /// </summary>
        private void ConvertAll()
        {

        }

        private void ConvertSingle(string path)
        {
            ExcelUtility.Read(path, m_Rule);
        }
    }
}
