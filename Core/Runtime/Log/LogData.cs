namespace GameFramework.Toolkit.Core.Runtime
{
    public class LogData
    {
        public string log;
        public string trace;
        public LogLevel logLevel;
    }

    public enum LogLevel : byte
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
    }
}
