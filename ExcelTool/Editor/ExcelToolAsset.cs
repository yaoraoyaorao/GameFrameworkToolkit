using UnityEditor;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public static class ExcelToolAsset
    {
        private static ExcelToolData m_Data;

        private static string AssetPath
        {
            get
            {
                return ExcelUtility.GetPath() + "/Editor/ExcelToolData.asset";
            }
        }

        public static ExcelToolData GetAsset
        {
            get
            {
                m_Data = AssetDatabase.LoadAssetAtPath<ExcelToolData>(AssetPath);

                if (m_Data == null)
                {
                    m_Data = ScriptableObject.CreateInstance<ExcelToolData>();
                    AssetDatabase.CreateAsset(m_Data, AssetPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                return m_Data;
            }
        }

        public static void SaveAsset()
        {
            EditorUtility.SetDirty(m_Data);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
