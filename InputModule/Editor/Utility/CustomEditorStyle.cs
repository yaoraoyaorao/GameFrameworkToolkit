using UnityEditor;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    /// <summary>
    /// 编辑器风格
    /// </summary>
    public static class CustomEditorStyle
    {
        public static readonly GUIStyle smallTickbox;
        public static readonly GUIStyle headerLabel;

        static readonly Color splitterDark;
        static readonly Color splitterLight;

        static readonly Texture2D paneOptionsIconDark;
        static readonly Texture2D paneOptionsIconLight;

        static readonly Color headerBackgroundDark;
        static readonly Color headerBackgroundLight;

        public static readonly Color listHeaderBackgroundDark;
        public static readonly Color listHeaderBackgroundLight;

        /// <summary>
        /// 分割线颜色
        /// </summary>
        public static Color splitter
        { get { return EditorGUIUtility.isProSkin ? splitterDark : splitterLight; } }

        /// <summary>
        /// 面板选项图标
        /// </summary>
        public static Texture2D paneOptionsIcon
        { get { return EditorGUIUtility.isProSkin ? paneOptionsIconDark : paneOptionsIconLight; } }

        public static Color headerBackground
        { get { return EditorGUIUtility.isProSkin ? headerBackgroundDark : headerBackgroundLight; } }


        static CustomEditorStyle()
        {
            smallTickbox = new GUIStyle("ShurikenToggle");
            headerLabel = new GUIStyle(EditorStyles.miniLabel);

            splitterDark = new Color(0.12f, 0.12f, 0.12f, 1.333f);
            splitterLight = new Color(0.6f, 0.6f, 0.6f, 1.333f);

            headerBackgroundDark = new Color(0.1f, 0.1f, 0.1f, 0.2f);
            headerBackgroundLight = new Color(1f, 1f, 1f, 0.2f);

            listHeaderBackgroundDark = new Color(0.2f, 0.2f, 0.2f, 1);
            listHeaderBackgroundLight = new Color(0.2f, 0.2f, 0.2f, 0.2f);

            paneOptionsIconDark = (Texture2D)EditorGUIUtility.Load("Builtin Skins/DarkSkin/Images/pane options.png");
            paneOptionsIconLight = (Texture2D)EditorGUIUtility.Load("Builtin Skins/LightSkin/Images/pane options.png");
        }
    }
}
