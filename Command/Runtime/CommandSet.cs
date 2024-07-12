using System;
using System.Linq;
using System.Reflection;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// ���
    /// </summary>
    public class CommandSet
    {
        public Type mType;
        public string Name;
        public Command[] Commands;

        /// <summary>
        /// ִ������
        /// </summary>
        /// <param name="commandName"></param>
        /// <param name="args"></param>
        /// <exception cref="Exception"></exception>
        public void Execute(string commandName,params string[] args)
        {
            if (!IsCommand(commandName))
                throw new Exception($"����������[{Name}]δ�ҵ�ָ������[{commandName}]");

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
                throw new Exception("����ִ�д��󣺶�������������Ǿ�̬��������");
            }


        }

        /// <summary>
        /// �Ƿ�������
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        public bool IsCommand(string commandName)
        {
            return Commands.Any(x => x.Name == commandName);
        }

        /// <summary>
        /// ���ַ�������ת��Ϊ��Ӧ����������
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

