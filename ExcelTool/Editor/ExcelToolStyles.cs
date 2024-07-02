using UnityEditor;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    [InitializeOnLoad]
    public static class ExcelToolStyles
    {
        public static Texture2D open;
        public static Texture2D convert;
        public static Texture2D remove;
        public static Texture2D refresh;
        public static Texture2D allConvert;
        public static Texture2D importExcel;
        public static Texture2D rule;
        public static Texture2D csharp;
        public static Texture2D prohibit;

        static ExcelToolStyles()
        {
            Load();
        }

        [InitializeOnLoadMethod]
        public static void Load()
        {
            open = EditorGUIUtility.Load(ExcelUtility.GetIconPath("open.png")) as Texture2D;
            convert = EditorGUIUtility.Load(ExcelUtility.GetIconPath("convert.png")) as Texture2D;
            remove = EditorGUIUtility.Load(ExcelUtility.GetIconPath("remove.png")) as Texture2D;
            refresh = EditorGUIUtility.Load(ExcelUtility.GetIconPath("refresh.png")) as Texture2D;
            allConvert = EditorGUIUtility.Load(ExcelUtility.GetIconPath("allConvert.png")) as Texture2D;
            importExcel = EditorGUIUtility.Load(ExcelUtility.GetIconPath("importExcel.png")) as Texture2D;
            rule = EditorGUIUtility.Load(ExcelUtility.GetIconPath("rule.png")) as Texture2D;
            csharp = EditorGUIUtility.Load(ExcelUtility.GetIconPath("csharp.png")) as Texture2D;
            prohibit = EditorGUIUtility.Load(ExcelUtility.GetIconPath("prohibit.png")) as Texture2D;

        }
    }

}
