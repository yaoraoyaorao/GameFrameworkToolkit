using GameFramework.Toolkit.Runtime;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework.Toolkit.Editor
{
    public static class TypeUtility
    {
        private static readonly string[] RuntimeAssemblyNames =
        {
            "GameFramework.Toolkit.Runtime",
            "Assembly-CSharp",
        };

        private static readonly string[] RuntimeOrEditorAssemblyNames =
        {
            "GameFramework.Toolkit.Runtime",
            "GameFramework.Toolkit.Editor",
            "Assembly-CSharp",
            "Assembly-CSharp-Editor",
        };

        /// <summary>
        /// 在运行时程序集中获取指定基类的所有子类的名称。
        /// </summary>
        /// <param name="typeBase">基类类型。</param>
        /// <returns>指定基类的所有子类的名称。</returns>
        public static string[] GetRuntimeTypeNames(System.Type typeBase)
        {
            return GetTypeNames(typeBase, RuntimeAssemblyNames);
        }

        /// <summary>
        /// 获取指定基类的所有子类的名称。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetConfigurationPath<T>() where T : Attribute
        {
            foreach (System.Type type in AssemblyUtility.GetTypes())
            {
                if (!type.IsAbstract || !type.IsSealed)
                {
                    continue;
                }

                foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    if (fieldInfo.FieldType == typeof(string) && fieldInfo.IsDefined(typeof(T), false))
                    {
                        return (string)fieldInfo.GetValue(null);
                    }
                }

                foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    if (propertyInfo.PropertyType == typeof(string) && propertyInfo.IsDefined(typeof(T), false))
                    {
                        return (string)propertyInfo.GetValue(null, null);
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// 在运行时或编辑器程序集中获取指定基类的所有子类的名称。
        /// </summary>
        /// <param name="typeBase">基类类型。</param>
        /// <returns>指定基类的所有子类的名称。</returns>
        public static string[] GetRuntimeOrEditorTypeNames(System.Type typeBase)
        {
            return GetTypeNames(typeBase, RuntimeOrEditorAssemblyNames);
        }

        /// <summary>
        /// 获取指定基类的所有子类的名称。
        /// </summary>
        /// <param name="typeBase"></param>
        /// <param name="assemblyNames"></param>
        /// <returns></returns>
        public static string[] GetTypeNames(System.Type typeBase, string[] assemblyNames)
        {
            List<string> typeNames = new List<string>();
            foreach (string assemblyName in assemblyNames)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.Load(assemblyName);
                }
                catch
                {
                    continue;
                }

                if (assembly == null)
                {
                    continue;
                }

                System.Type[] types = assembly.GetTypes();
                foreach (System.Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                    {
                        typeNames.Add(type.FullName);
                    }
                }
            }

            typeNames.Sort();
            return typeNames.ToArray();
        }
    }
}
