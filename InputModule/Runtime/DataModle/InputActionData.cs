using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// 输入行为数据
    /// </summary>
    public class InputActionData
    {
        /// <summary>
        /// 输入行为名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 行为类型
        /// </summary>
        public string TypeName;

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enable;

        /// <summary>
        /// 方位X
        /// </summary>
        public float X;

        /// <summary>
        /// 方位Y
        /// </summary>
        public float Y;

        /// <summary>
        /// 方位Z
        /// </summary>
        public float Z;

        /// <summary>
        /// 键组
        /// </summary>
        public KeyCode[] KeyCodes;

        /// <summary>
        /// 鼠标键
        /// </summary>
        public MouseButton MouseButton;

        /// <summary>
        /// 时间
        /// </summary>
        public float Time1;
        public float Time2;

        /// <summary>
        /// 次数
        /// </summary>
        public int Count;
    }

    public enum MouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }
}

