using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    [CommandSet("app")]
    public static class AppCommand
    {
        /// <summary>
        /// 获取应用程序的路径
        /// </summary>
        public static void path()
        {
            string path = 
                $"\n[dataPath:{Application.dataPath}]\n" +
                $"[persistentDataPath:{Application.persistentDataPath}]\n" +
                $"[streamingAssetsPath:{Application.streamingAssetsPath}]";

            Debug.Log(path);
        }
    }

    [CommandSet("time")]
    public static class TimeCommand
    {
        public static void setScale(float c)
        {
            Time.timeScale = c;

            getScale();
        }

        public static void getScale()
        {
            Debug.Log($"当前timeScale:{Time.timeScale}");
        }

        public static void stop()
        {
            Time.timeScale = 0;

            getScale();
        }

        public static void resume()
        {
            Time.timeScale = 1;

            getScale();
        } 
    }
}

