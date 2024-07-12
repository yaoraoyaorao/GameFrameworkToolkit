using GameFramework.Toolkit.Core.Runtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class CommandGUI : MonoBehaviour
    {
        private static CommandGUI instance;

        private CommandComponent commandModule;
        private readonly Queue<LogData> logs = new Queue<LogData>();

        private Rect m_WindowRect = DefaultWindowRect;
        private Rect m_DragRect = new Rect(0f, 0f, float.MaxValue, 25f);
        private static readonly Rect DefaultWindowRect = new Rect(0, 0, 640f, 480f);
        private static readonly Vector2 ReferenceResolution = new Vector2(1280, 720);


        private string command;
        private bool isCollapse;
        private bool isSelected;
        private Vector2 logScrollPosition;
        private Vector2 traceScrollPosition;
        private LogData selectedLog;

        private float collapseHeight = 60;
        private Vector2 m_WindowScale = Vector2.one;
        private Action actions;

        private float m_Height;
        private float m_Width;

        private float maxX;
        private float maxY;

        private float m_ScaleFactor = 1f;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            commandModule = GameObject.FindObjectOfType<CommandComponent>();

            actions += DrawCommandArae;
            actions += DrawLogArea;

            CalculatedScalingFactor();

            Application.logMessageReceivedThreaded += LogCallback;
        }

        private void Update()
        {
            if (m_Height != Screen.height || m_Width != Screen.width)
            {
                CalculatedScalingFactor();
            }
        }

        private void OnDestroy()
        {
            Application.logMessageReceivedThreaded -= LogCallback;

        }

        private void OnGUI()
        {
            maxX = m_Width - DefaultWindowRect.width + OffsetX();
            maxY = m_Height - (isCollapse ? collapseHeight : DefaultWindowRect.height) + OffsetY();
            m_WindowRect.x = Mathf.Clamp(m_WindowRect.x, 0, maxX);
            m_WindowRect.y = Mathf.Clamp(m_WindowRect.y, 0, maxY);

            Matrix4x4 cachedMatrix = GUI.matrix;
            //GUI.matrix = Matrix4x4.Scale(new Vector3(m_WindowScale.x, m_WindowScale.y, 1f));
            GUI.matrix = Matrix4x4.Scale(new Vector3(m_ScaleFactor, m_ScaleFactor, 1f));

            m_WindowRect = GUILayout.Window(0, m_WindowRect, DrawWindow, "命令行工具");

            GUI.matrix = cachedMatrix;
        }

        private void DrawWindow(int id)
        {
            GUI.DragWindow(m_DragRect);

            actions?.Invoke();
        }

        private void DrawCommandArae()
        {
            GUILayout.BeginHorizontal("box", GUILayout.ExpandWidth(true));
            GUILayout.Label("输入命令：", GUILayout.Width(70));
            command = GUILayout.TextField(command, GUILayout.ExpandWidth(true));
            if (GUILayout.Button("确认", GUILayout.Width(50)))
            {
                ExecuteCommand();
            }
            if (GUILayout.Button("清除", GUILayout.Width(50)))
            {
                logs.Clear();
            }
            if (GUILayout.Button(isCollapse ? "展开" : "折叠", GUILayout.Width(50)))
            {
                isCollapse = !isCollapse;

                //如果窗口折叠
                if (isCollapse)
                {
                    m_WindowRect.height = collapseHeight;

                    if (m_WindowRect.y >= maxY)
                    {
                        m_WindowRect.y = m_Height - collapseHeight + OffsetY();
                    }
                }
                else
                {
                    float ofsset = m_WindowRect.y + DefaultWindowRect.height;
                    //窗口超出下方，日志区域先渲染
                    if (ofsset > maxY)
                    {
                        actions = null;
                        actions += DrawLogArea;
                        actions += DrawCommandArae;

                        m_WindowRect.y = m_Height - DefaultWindowRect.height + OffsetY();
                    }
                    else
                    {
                        actions = null;
                        actions += DrawCommandArae;
                        actions += DrawLogArea;
                    }

                    m_WindowRect.height = DefaultWindowRect.height;
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawLogArea()
        {

            if (isCollapse) return;

            GUILayout.BeginHorizontal("box", GUILayout.MaxHeight(250));
            logScrollPosition = GUILayout.BeginScrollView(logScrollPosition);
            foreach (var log in logs)
            {
                if (log != null)
                {
                    if (GUILayout.Toggle(selectedLog == log, log.log))
                    {
                        isSelected = true;

                        if (selectedLog != log)
                        {
                            selectedLog = log;
                            traceScrollPosition = Vector2.zero;
                        }
                    }
                }

            }
            if (!isSelected)
            {
                selectedLog = null;
            }
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box", GUILayout.Height(150));
            traceScrollPosition = GUILayout.BeginScrollView(traceScrollPosition);
            if (selectedLog != null)
            {
                GUILayout.Label(selectedLog.trace);
            }
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        private void ExecuteCommand()
        {
            if (commandModule == null)
                return;
            commandModule.Execute(command);
        }

        /// <summary>
        /// 计算缩放系数
        /// </summary>
        private void CalculatedScalingFactor()
        {
            m_Width = Screen.width;
            m_Height = Screen.height;
            m_WindowScale = new Vector2(m_Width / ReferenceResolution.x, m_Height / ReferenceResolution.y);

            m_ScaleFactor = (m_WindowScale.x + m_WindowScale.y) / 2;
        }

        /// <summary>
        /// X偏移
        /// </summary>
        /// <returns></returns>
        private float OffsetX()
        {
            //return (1 - m_WindowScale.x) * ReferenceResolution.x;
            return (1 - m_ScaleFactor) * ReferenceResolution.x;
        }

        /// <summary>
        /// Y偏移
        /// </summary>
        /// <returns></returns>
        private float OffsetY()
        {
            //return (1 - m_WindowScale.y) * ReferenceResolution.y;
            return (1 - m_ScaleFactor) * ReferenceResolution.y;
        }

        /// <summary>
        /// 日志回调
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="stackTrace"></param>
        /// <param name="type"></param>
        private void LogCallback(string condition, string stackTrace, LogType type)
        {
            logs.Enqueue(new LogData()
            {
                log = condition,
                trace = stackTrace,
                logLevel = LogUtility.ChangeLogLevel(type)
            });
        }
    }
}

