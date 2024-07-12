using System;
using System.Collections.Generic;

namespace GameFramework.Toolkit.Runtime
{
    internal static class CommandHelper
    {
        private static Dictionary<Type, ICommandArgs> commandArgs = new Dictionary<Type, ICommandArgs>();
    
        public static void RegisterCommandArgs<T1,T2>() where T2 : ICommandArgs
        {
            var instance = Activator.CreateInstance<T2>();
            RegisterCommandArgs<T1>(instance);
        }

        public static void RegisterCommandArgs<T1>(ICommandArgs arg)
        {
            RegisterCommandArgs(typeof(T1), arg);
        }

        public static void RegisterCommandArgs(Type type1, ICommandArgs arg)
        {
            if (commandArgs.ContainsKey(type1))
            {
                return;
            }

            if (arg == null)
                return;

            commandArgs.Add(type1, arg);
        }

        public static object ToCommand<T>(string args)
        {
            return ToCommand(typeof(T), args);
        }

        public static object ToCommand(Type type,string args)
        {
            if (!commandArgs.ContainsKey(type))
            {
                throw new Exception($"未找到命令参数转换器:{type}");
            }

            object obj = commandArgs[type].ToCommand(args);

            if (obj == null)
            {
                throw new Exception("参数格式错误");
            }

            return obj;
            
        }
    }
}

