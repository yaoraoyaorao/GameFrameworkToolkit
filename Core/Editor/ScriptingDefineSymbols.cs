using System.Collections.Generic;
using UnityEditor;

namespace GameFramework.Toolkit.Editor
{
    public static class ScriptingDefineSymbols
    {

        private static readonly BuildTargetGroup[] BuildTargetGroups = new BuildTargetGroup[]
        {
            BuildTargetGroup.Standalone,
            BuildTargetGroup.iOS,
            BuildTargetGroup.Android,
            BuildTargetGroup.WSA,
            BuildTargetGroup.WebGL
        };

        /// <summary>
        /// ���ָ��ƽ̨�Ƿ����ָ���Ľű��궨�塣
        /// </summary>
        /// <param name="buildTargetGroup">Ҫ���ű��궨���ƽ̨��</param>
        /// <param name="scriptingDefineSymbol">Ҫ���Ľű��궨�塣</param>
        /// <returns>ָ��ƽ̨�Ƿ����ָ���Ľű��궨�塣</returns>
        public static bool HasScriptingDefineSymbol(BuildTargetGroup buildTargetGroup, string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return false;
            }

            string[] scriptingDefineSymbols = GetScriptingDefineSymbols(buildTargetGroup);
            foreach (string i in scriptingDefineSymbols)
            {
                if (i == scriptingDefineSymbol)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasScriptingDefineSymbol(string scriptingDefineSymbol)
        {
            bool flag = false;

            foreach (BuildTargetGroup buildTargetGroup in BuildTargetGroups)
            {
                flag = HasScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol);

                if (!flag)
                {
                    return false;
                }
            }

            return flag;
        }

        /// <summary>
        /// Ϊָ��ƽ̨����ָ���Ľű��궨�塣
        /// </summary>
        /// <param name="buildTargetGroup">Ҫ���ӽű��궨���ƽ̨��</param>
        /// <param name="scriptingDefineSymbol">Ҫ���ӵĽű��궨�塣</param>
        public static void AddScriptingDefineSymbol(BuildTargetGroup buildTargetGroup, string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            if (HasScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol))
            {
                return;
            }

            List<string> scriptingDefineSymbols = new List<string>(GetScriptingDefineSymbols(buildTargetGroup))
            {
                scriptingDefineSymbol
            };

            SetScriptingDefineSymbols(buildTargetGroup, scriptingDefineSymbols.ToArray());
        }

        /// <summary>
        /// Ϊָ��ƽ̨�Ƴ�ָ���Ľű��궨�塣
        /// </summary>
        /// <param name="buildTargetGroup">Ҫ�Ƴ��ű��궨���ƽ̨��</param>
        /// <param name="scriptingDefineSymbol">Ҫ�Ƴ��Ľű��궨�塣</param>
        public static void RemoveScriptingDefineSymbol(BuildTargetGroup buildTargetGroup, string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            if (!HasScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol))
            {
                return;
            }

            List<string> scriptingDefineSymbols = new List<string>(GetScriptingDefineSymbols(buildTargetGroup));
            while (scriptingDefineSymbols.Contains(scriptingDefineSymbol))
            {
                scriptingDefineSymbols.Remove(scriptingDefineSymbol);
            }

            SetScriptingDefineSymbols(buildTargetGroup, scriptingDefineSymbols.ToArray());
        }

        /// <summary>
        /// Ϊ����ƽ̨����ָ���Ľű��궨�塣
        /// </summary>
        /// <param name="scriptingDefineSymbol">Ҫ���ӵĽű��궨�塣</param>
        public static void AddScriptingDefineSymbol(string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            foreach (BuildTargetGroup buildTargetGroup in BuildTargetGroups)
            {
                AddScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol);
            }
        }

        /// <summary>
        /// Ϊ����ƽ̨�Ƴ�ָ���Ľű��궨�塣
        /// </summary>
        /// <param name="scriptingDefineSymbol">Ҫ�Ƴ��Ľű��궨�塣</param>
        public static void RemoveScriptingDefineSymbol(string scriptingDefineSymbol)
        {
            if (string.IsNullOrEmpty(scriptingDefineSymbol))
            {
                return;
            }

            foreach (BuildTargetGroup buildTargetGroup in BuildTargetGroups)
            {
                RemoveScriptingDefineSymbol(buildTargetGroup, scriptingDefineSymbol);
            }
        }

        /// <summary>
        /// ��ȡָ��ƽ̨�Ľű��궨�塣
        /// </summary>
        /// <param name="buildTargetGroup">Ҫ��ȡ�ű��궨���ƽ̨��</param>
        /// <returns>ƽ̨�Ľű��궨�塣</returns>
        public static string[] GetScriptingDefineSymbols(BuildTargetGroup buildTargetGroup)
        {
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';');
        }

        /// <summary>
        /// ����ָ��ƽ̨�Ľű��궨�塣
        /// </summary>
        /// <param name="buildTargetGroup">Ҫ���ýű��궨���ƽ̨��</param>
        /// <param name="scriptingDefineSymbols">Ҫ���õĽű��궨�塣</param>
        public static void SetScriptingDefineSymbols(BuildTargetGroup buildTargetGroup, string[] scriptingDefineSymbols)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, string.Join(";", scriptingDefineSymbols));
        }
    }
}
