using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// ������Ϊ����
    /// </summary>
    public class InputActionData
    {
        /// <summary>
        /// ������Ϊ����
        /// </summary>
        public string Name;

        /// <summary>
        /// ��Ϊ����
        /// </summary>
        public string TypeName;

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool Enable;

        /// <summary>
        /// ��λX
        /// </summary>
        public float X;

        /// <summary>
        /// ��λY
        /// </summary>
        public float Y;

        /// <summary>
        /// ��λZ
        /// </summary>
        public float Z;

        /// <summary>
        /// ����
        /// </summary>
        public KeyCode[] KeyCodes;

        /// <summary>
        /// ����
        /// </summary>
        public MouseButton MouseButton;

        /// <summary>
        /// ʱ��
        /// </summary>
        public float Time1;
        public float Time2;

        /// <summary>
        /// ����
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

