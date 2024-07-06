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


        [MenuItem("GameFramework/Excel工具")]
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
               
                EditorGUILayout.LabelField("Excel总路径*", GUILayout.Width(70));
                GUI.enabled = false;
                EditorGUILayout.TextField(m_Data.ExcelPath);
                GUI.enabled = true;

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.importExcel, "导入Excel总目录"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    SelectPath();
                }

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.refresh, "重新导入Excel"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    RefreshList();
                }
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                EditorGUILayout.LabelField("代码路径*", GUILayout.Width(70));

                GUI.enabled = false;
                EditorGUILayout.TextField(m_Data.CSharpPath);
                GUI.enabled = true;

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.csharp, "设置C#代码目录"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    m_Data.CSharpPath = OpenFolderPanel(m_Data.CSharpPath, "选择C#代码文件夹");
                }
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                EditorGUILayout.LabelField("数据路径*", GUILayout.Width(70));

                GUI.enabled = false;
                EditorGUILayout.TextField(m_Data.SaveDataPath);
                GUI.enabled = true;

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.data, "设置数据目录"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    m_Data.SaveDataPath = OpenFolderPanel(m_Data.SaveDataPath, "选择C#代码文件夹");
                }
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                EditorGUILayout.LabelField("命名空间*", GUILayout.Width(70));
                m_Data.NameSpace = EditorGUILayout.TextField(m_Data.NameSpace, GUILayout.Width(100));

                EditorGUILayout.LabelField("类前缀", GUILayout.Width(70));
                m_Data.ClassPrefix = EditorGUILayout.TextField(m_Data.ClassPrefix, GUILayout.Width(100));
            }

            using (new EditorGUILayout.HorizontalScope("toolbar"))
            {
                EditorGUILayout.LabelField("生成规则：", GUILayout.Width(60));
                m_Data.RuleIndex = EditorGUILayout.Popup(m_Data.RuleIndex, m_RuleNames, EditorStyles.toolbarDropDown, GUILayout.Width(200));

                EditorGUILayout.LabelField("目标数据：", GUILayout.Width(60));
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
                if (GUILayout.Button(new GUIContent(ExcelToolStyles.selectAll, "全选"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    m_Data.SelectAll();
                }

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.disselectAll, "全不选"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    m_Data.DisSelectAll();
                }

                if (GUILayout.Button(new GUIContent(ExcelToolStyles.invertSelect, "反选"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    m_Data.InvertSelect();
                }

                using (new EditorGUI.DisabledGroupScope(m_Data.SelectCount == 0))
                {
                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.allConvert, "转换当前所选Excel"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                    {
                        ConvertSelectAll();
                    }

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.remove, "移除当前所选Excel"), EditorStyles.toolbarButton, GUILayout.Width(50)))
                    {
                        RemoveSelectAll();
                    }

                    EditorGUILayout.LabelField($"所选数:{m_Data.SelectCount}", GUILayout.Width(70));
                }

            }
        }

        /// <summary>
        /// 绘制列表
        /// </summary>
        private void DrawList()
        {
            if (m_Data.ExcelList == null)
                return;

            if (!Directory.Exists(m_Data.ExcelPath))
            {
                EditorGUILayout.HelpBox("Excel文件夹不存在", MessageType.Error);
                return;
            }

            if (string.IsNullOrEmpty(m_Data.ExcelPath) || m_Data.ExcelPath == "/")
            {
                return;
            }

            if (m_Data.ExcelList.Count == 0)
            {
                EditorGUILayout.HelpBox("Excel文件不存在", MessageType.Warning);
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

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.convert, "转换当前Excel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        ConvertSingle(path);
                    }

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.open, "打开当前Excel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        OpenFile(path);
                    }

                    if (GUILayout.Button(new GUIContent(ExcelToolStyles.remove, "移除当前Excel"), EditorStyles.iconButton, GUILayout.Width(35)))
                    {
                        m_Data.ExcelList.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 选择Excel文件夹
        /// </summary>
        private void SelectPath()
        {
            m_Data.ExcelPath = OpenFolderPanel(m_Data.ExcelPath,"选择Excel文件夹");

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
        /// 刷新列表
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
        /// 刷新规则
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
        /// 刷新数据构建器
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
        /// 刷新目标数据
        /// </summary>
        private void RefreshTargetData()
        {

        }

        /// <summary>
        /// 打开Excel文件
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
        /// 转换单个Excel文件
        /// </summary>
        /// <param name="path"></param>
        private void ConvertSingle(string path)
        {
            bool ok = EditorUtility.DisplayDialog("提示", "确定要转换当前文件吗？", "确定", "取消");
            if (ok)
            {
                Convert(path);
            }
        }

        /// <summary>
        /// 转换Excel文件
        /// </summary>
        private void ConvertSelectAll()
        {
            bool ok = EditorUtility.DisplayDialog("提示", "确定要转换当前所选文件吗？", "确定", "取消");
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
        /// 移除所选Excel文件
        /// </summary>
        private void RemoveSelectAll()
        {
            bool ok = EditorUtility.DisplayDialog("警告", "确定要移除所选Excel文件吗？", "确定", "取消");
            if (ok)
            {
                m_Data.RemoveSelect();
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns></returns>
        private bool CheckData()
        {
            if (m_Data == null)
            {
                Debug.LogError("数据不能为空");

                return false;
            }

            if (string.IsNullOrWhiteSpace(m_Data.NameSpace))
            {
                Debug.LogError("命名空间不能为空");
                return false;
            }

            if (!Directory.Exists(m_Data.SaveDataPath))
            {
                Debug.LogError("存储数据文件夹不存在");
                return false;
            }

            if (!Directory.Exists(m_Data.CSharpPath))
            {
                Debug.LogError("存储代码文件夹不存在");
                return false;
            }

            return true;
        }
    }
}
