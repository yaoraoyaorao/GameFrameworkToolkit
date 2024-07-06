using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Toolkit.Editor
{
    public static class Utility
    {
        /// <summary>
        /// 获取包相对路径
        /// </summary>
        /// <param name="packageFullName"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static string GetPackageRelativePath(string packageFullName, string packageName)
        {
            string packagePath = Path.GetFullPath($"Packages/{packageFullName}");

            if (Directory.Exists(packagePath))
            {
                return $"Packages/{packageFullName}";
            }

            packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                if (Directory.Exists(packagePath + $"/Assets/Packages/{packageFullName}"))
                {
                    return $"Assets/Packages/{packageName}";
                }

                if (Directory.Exists(packagePath + $"/Assets/{packageName}"))
                {
                    return $"Assets/{packageName}";
                }

                string[] matchingPaths = Directory.GetDirectories(packagePath, packageName, SearchOption.AllDirectories);
                string path = ValidateLocation(matchingPaths, packagePath);
                if (path != null) return path;
            }

            return null;
        }

        /// <summary>
        /// 获取包路径
        /// </summary>
        /// <param name="packageFullName"></param>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public static string GetPackagePath(string packageFullName,string packageName)
        {
            string packagePath = Path.GetFullPath($"Packages/{packageFullName}");

            if (Directory.Exists(packagePath))
            {
                return packagePath;
            }

            packagePath = Path.GetFullPath("Assets/..");
            if (Directory.Exists(packagePath))
            {
                if (Directory.Exists(packagePath + $"/Assets/Packages/{packageFullName}"))
                {
                    return packagePath + $"/Assets/Packages/{packageName}";
                }

                if (Directory.Exists(packagePath + $"/Assets/{packageName}"))
                {
                    return packagePath + $"/Assets/{packageName}";
                }

                string[] matchingPaths = Directory.GetDirectories(packagePath, packageName, SearchOption.AllDirectories);
                string path = ValidateLocation(matchingPaths, packagePath);
                if (path != null) return Path.Combine(packagePath,path);
            }

            return null;
        }

        /// <summary>
        /// 获取类型名称组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string[] GetTypeNames(Type type,string[] assemblyNames)
        {
            List<string> typeNames = new List<string>
            {
                "<None>"
            };

            typeNames.AddRange(TypeUtility.GetTypeNames(type, assemblyNames));

            return typeNames.ToArray();
        }

        private static string ValidateLocation(string[] paths, string projectPath)
        {
            string folderPath = "";
            for (int i = 0; i < paths.Length; i++)
            {
                if (Directory.Exists(paths[i]))
                {
                    folderPath = paths[i].Replace(projectPath, "");
                    folderPath = folderPath.TrimStart('\\', '/');
                    return folderPath;
                }
            }

            return null;
        }
    }
}
