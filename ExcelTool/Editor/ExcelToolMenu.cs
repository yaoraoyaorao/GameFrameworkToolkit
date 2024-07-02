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
                if (GUILayout.Button(new GUIContent(ExcelToolStyles.importExcel, "导入Excel总目录"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    SelectPath();
                }

                EditorGUILayout.LabelField("Excel总路径", GUILayout.Width(70));

                GUI.enabled = false;
                EditorGUILayout.TextField(m_Data.m_ExcelPath);
                GUI.enabled = true;

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.allConvert, "转换当前所有Excel"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    ConvertAll();
                }

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.refresh, "重新导入Excel"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    RefreshList();
                }
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                if (GUILayout.Button(new GUIContent(ExcelToolStyles.csharp, "设置C#代码目录"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {

                }

                EditorGUILayout.LabelField("代码路径", GUILayout.Width(70));

                GUI.enabled = false;
                EditorGUILayout.TextField(m_Data.m_CSharpPath);
                GUI.enabled = true;

                
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                EditorGUILayout.LabelField("筛选规则：", GUILayout.Width(60));
                EditorGUILayout.Popup(0, new string[] { "<None>" }, EditorStyles.toolbarDropDown, GUILayout.Width(200));

                EditorGUILayout.LabelField("目标数据：", GUILayout.Width(60));
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
        /// 选择Excel文件夹
        /// </summary>
        private void SelectPath()
        {
            m_Data.m_ExcelPath = EditorUtility.OpenFolderPanel("选择Excel文件夹", "", "");

            RefreshList();
        }

        /// <summary>
        /// 刷新列表
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
        /// 打开Excel文件
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
        /// 绘制列表
        /// </summary>
        private void DrawList()
        {
            if (m_Data.m_ExcelList == null)
                return;

            if (!Directory.Exists(m_Data.m_ExcelPath))
            {
                EditorGUILayout.HelpBox("Excel文件夹不存在", MessageType.Error);
                return;
            }

            if (string.IsNullOrEmpty(m_Data.m_ExcelPath) || m_Data.m_ExcelPath == "/")
            {
                return;
            }

            if (m_Data.m_ExcelList.Count == 0)
            {
                EditorGUILayout.HelpBox("Excel文件不存在", MessageType.Warning);
                return;
            }

            for (int i = 0; i < m_Data.m_ExcelList.Count; i++)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    string path = m_Data.m_ExcelList[i];

                    EditorGUILayout.LabelField(path);

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.convert,"转换当前Excel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        ConvertSingle(path);
                    }

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.open,"打开当前Excel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        OpenExcelFile(path);
                    }

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.remove,"移除当前Excel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        m_Data.m_ExcelList.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 转换所有Excel文件
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
