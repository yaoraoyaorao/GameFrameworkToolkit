using UnityEngine;

namespace GameFramework.Toolkit.Core.Runtime
{
    public static class LogUtility
    {
        public static LogLevel ChangeLogLevel(LogType logType)
        {
            switch (logType)
            {
                case LogType.Error:
                    return LogLevel.Error;
                case LogType.Warning:
                    return LogLevel.Warning;
                case LogType.Log:
                    return LogLevel.Info;
                default:
                    return LogLevel.Info;
            }
        }
    }
}
