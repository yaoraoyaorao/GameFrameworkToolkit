using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class CommandComponent : MonoBehaviour
    {
        public bool EnableGUI;
        public List<string> RuntimeAssemblyNames;

        private static readonly string[] DefaultAssemblyNames =
        {
            "GameFramework.Toolkit.Runtime",

            "Assembly-CSharp",
        };

        private readonly Dictionary<string, CommandSet> commandDic = new Dictionary<string, CommandSet>();

        private const string CommandName = "Command";
        
        private List<Command> commandList = new List<Command>();

        private void Start()
        {
            InitilizeAssemblyNames();

            RegisterDefaultCommand();

            BuildCommandList();

            OnEnableGUI();
        }

        /// <summary>
        /// ִ������
        /// </summary>
        /// <param name="command"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Execute(string command)
        {
            if (string.IsNullOrEmpty(command)) return;

            if (!command.StartsWith("/"))
            {
                throw new Exception("�����ʽ����:���������'/'��ͷ");
            }

            command = command.TrimStart('/');

            var parts = command.Split(' ');

            if (parts.Length < 2)
            {
                throw new ArgumentException("�����ʽ����:[���] [������] [����]");
            }

            var commandSetName = parts[0];
            var methodNmae = parts[1];
            var methodArgs = parts.Skip(2).ToArray();

            if (!commandDic.ContainsKey(commandSetName))
            {
                throw new ArgumentException($"δ�ҵ����:{commandSetName}");
            }

            var commandSet = commandDic[commandSetName];

            commandSet.Execute(methodNmae, methodArgs);
        }

        /// <summary>
        /// ע���������ת����
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        public void RegisterCommandArgs<T1, T2>() where T2 : ICommandArgs
        {
            CommandHelper.RegisterCommandArgs<T1, T2>();
        }

        /// <summary>
        /// ע���������ת����
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        public void RegisterCommandArgs<T>(ICommandArgs arg)
        {
            CommandHelper.RegisterCommandArgs<T>(arg);
        }

        /// <summary>
        /// ע���������ת����
        /// </summary>
        /// <param name="type"></param>
        /// <param name="arg"></param>
        public void RegisterCommandArgs(Type type, ICommandArgs arg)
        {
            CommandHelper.RegisterCommandArgs(type, arg);
        }

        /// <summary>
        /// ��ʼ����������
        /// </summary>
        private void InitilizeAssemblyNames()
        {
            List<string> temp = RuntimeAssemblyNames;

            if (temp == null)
            {
                temp = new List<string>();

                temp.AddRange(DefaultAssemblyNames);

                return;
            }

            if (temp.Count == 0)
            {
                temp.AddRange(DefaultAssemblyNames);

                return;
            }

            if (!temp.Contains(DefaultAssemblyNames[0]))
            {
                temp.Add(DefaultAssemblyNames[0]);
            }

            if (!temp.Contains(DefaultAssemblyNames[1]))
            {
                temp.Add(DefaultAssemblyNames[1]);
            }

            RuntimeAssemblyNames = temp;
        }

        /// <summary>
        /// �������
        /// </summary>
        private void BuildCommandList()
        {
            commandDic.Add(CommandName, new CommandSet()
            {
                mType = this.GetType(),
                Name = CommandName,
            });

            Type[] types = AssemblyUtility.GetTypes();
            foreach (Type type in types)
            {
                if (BuildCommandSetAttribute(type)) continue;

                BuildCommandAttribute(type);
            }

            commandDic[CommandName].Commands = commandList.ToArray();
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool BuildCommandSetAttribute(Type type)
        {
            bool isSet = Attribute.IsDefined(type, typeof(CommandSetAttribute))
                         && type.IsAbstract && type.IsSealed;

            if (!isSet) return false;

            var attribute = (CommandSetAttribute)type.GetCustomAttributes(typeof(CommandSetAttribute), false)[0];
            
            string commandSetName = attribute.Name;
            
            if (string.IsNullOrEmpty(commandSetName))
            {
                commandSetName = type.Name;
            }

            if (commandDic.ContainsKey(commandSetName)) return false;

            var methods = type.GetMethods().Where(m => m.DeclaringType == type).ToArray();

            Command[] commands = new Command[methods.Length];

            for (int i = 0; i < methods.Length; i++)
            {
                commands[i] = new Command()
                {
                    Name = methods[i].Name,
                    Method = methods[i]
                };
            }

            commandDic.Add(commandSetName, new CommandSet
            {
                mType = type,
                Name = commandSetName,
                Commands = commands
            });

            return true;
        }

        /// <summary>
        /// ������ɢ����
        /// </summary>
        /// <param name="type"></param>
        private void BuildCommandAttribute(Type type)
        {
            var methods = type.GetMethods();

            foreach (var method in methods)
            {
                bool isCommand = Attribute.IsDefined(method, typeof(CommandAttribute));

                if (!isCommand) continue;

                var attribute = (CommandAttribute)method.GetCustomAttributes(typeof(CommandAttribute), false)[0];

                string commandName = attribute.Name;

                if (string.IsNullOrEmpty(commandName))
                {
                    commandName = method.Name;
                }

                commandList.Add(new Command()
                {
                    Name = commandName,
                    Method = method
                });
            }
        }

        /// <summary>
        /// ����Ĭ���������
        /// </summary>
        private void RegisterDefaultCommand()
        {
            CommandHelper.RegisterCommandArgs<Vector3>(new Vector3Args());
            CommandHelper.RegisterCommandArgs<Vector2>(new Vector2Args());
            CommandHelper.RegisterCommandArgs<string[]>(new StringArrayArgs());
        }

        /// <summary>
        /// ����GUI
        /// </summary>
        private void OnEnableGUI()
        {
            if(EnableGUI)
            {

                var go = new GameObject("CommandGUI");

                go.AddComponent<CommandGUI>();
            }
        }

    }
}

