using System;
using System.Linq;
using System.Reflection;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 命令集
    /// </summary>
    public class CommandSet
    {
        public Type mType;
        public string Name;
        public Command[] Commands;

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="args"></param>
        /// <exception cref="Exception"></exception>
        public void Execute(string commandName,params string[] args)
        {
            if (!IsCommand(commandName))
                throw new Exception($"命令错误：命令集[{Name}]未找到指定命令[{commandName}]");

            var matchingMethods = Commands.Where(x => x.Name == commandName).ToArray();

            try
            {
                foreach (var command in matchingMethods)
                {
                    var parameters = command.Method.GetParameters();

                    if (parameters.Length == args.Length)
                    {
                        object[] typedArgs = ConvertArgs(args, parameters);

                        if (typedArgs != null)
                        {
                            command.Method.Invoke(null, typedArgs);
                        }
                    }
                }
            }
            catch (TargetException)
            {
                throw new Exception("命令执行错误：独立的命令必须是静态公共方法");
            }


        }

        /// <summary>
        /// 是否是命令
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public bool IsCommand(string commandName)
        {
            return Commands.Any(x => x.Name == commandName);
        }

        /// <summary>
        /// 将字符串参数转换为对应的数据类型
        /// </summary>
        /// <param name="args"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private object[] ConvertArgs(string[] args, ParameterInfo[] parameters)
        {
            object[] typedArgs = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                Type parameterType = parameters[i].ParameterType;

                try
                {
                    typedArgs[i] = Convert.ChangeType(args[i], parameterType);
                }
                catch (Exception)
                {
                    typedArgs[i] = CommandHelper.ToCommand(parameterType, args[i]);
                }
            }

            return typedArgs;
        }
    }
}

