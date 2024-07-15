using System;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Toolkit.Runtime
{
    public static class AssemblyUtility
    {
        private static readonly System.Reflection.Assembly[] s_Assemblies = null;
        private static readonly Dictionary<string, Type> s_CachedTypes = new Dictionary<string, Type>(StringComparer.Ordinal);

        static AssemblyUtility()
        {
            s_Assemblies = AppDomain.CurrentDomain.GetAssemblies();
        }

        /// <summary>
        /// ��ȡ�Ѽ��صĳ��򼯡�
        /// </summary>
        /// <returns>�Ѽ��صĳ��򼯡�</returns>
        public static System.Reflection.Assembly[] GetAssemblies()
        {
            return s_Assemblies;
        }

        /// <summary>
        /// ��ȡ�Ѽ��صĳ����е��������͡�
        /// </summary>
        /// <returns>�Ѽ��صĳ����е��������͡�</returns>
        public static Type[] GetTypes()
        {
            List<Type> results = new List<Type>();
            foreach (System.Reflection.Assembly assembly in s_Assemblies)
            {
                results.AddRange(assembly.GetTypes());
            }

            return results.ToArray();
        }

        /// <summary>
        /// ��ȡ�Ѽ��صĳ����е��������͡�
        /// </summary>
        /// <param name="results">�Ѽ��صĳ����е��������͡�</param>
        public static void GetTypes(List<Type> results)
        {
            if (results == null)
            {
                throw new Exception("Results is invalid.");
            }

            results.Clear();
            foreach (System.Reflection.Assembly assembly in s_Assemblies)
            {
                results.AddRange(assembly.GetTypes());
            }
        }

        /// <summary>
        /// ��ȡ���г�������
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllAssemblyTypes()
        {
            var result = s_Assemblies.SelectMany(t =>
            {
                var innerTypes = new Type[0];
                try
                {
                    innerTypes = t.GetTypes();
                }
                catch { }
                return innerTypes;
            });

            return result;
        }

        /// <summary>
        /// ��ȡ��������������
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllTypesDerivedFrom<T>()
        {
#if UNITY_EDITOR && UNITY_2019_2_OR_NEWER
            return UnityEditor.TypeCache.GetTypesDerivedFrom<T>();
#else
            return GetAllAssemblyTypes().Where(t => t.IsSubclassOf(typeof(T)));
#endif
        }

        public static IEnumerable<Type> GetAllTypesDerivedFrom(Type type)
        {
            return UnityEditor.TypeCache.GetTypesDerivedFrom(type);
        }

        /// <summary>
        /// ��ȡ�Ѽ��صĳ����е�ָ�����͡�
        /// </summary>
        /// <param name="typeName">Ҫ��ȡ����������</param>
        /// <returns>�Ѽ��صĳ����е�ָ�����͡�</returns>
        public static Type GetType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                throw new Exception("Type name is invalid.");
            }

            Type type = null;
            if (s_CachedTypes.TryGetValue(typeName, out type))
            {
                return type;
            }

            type = Type.GetType(typeName);
            if (type != null)
            {
                s_CachedTypes.Add(typeName, type);
                return type;
            }

            foreach (System.Reflection.Assembly assembly in s_Assemblies)
            {
                type = Type.GetType(string.Format("{0}, {1}", typeName, assembly.FullName));
                if (type != null)
                {
                    s_CachedTypes.Add(typeName, type);
                    return type;
                }
            }

            return null;
        }
    }
}
