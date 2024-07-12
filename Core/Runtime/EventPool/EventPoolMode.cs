using System;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// �¼���ģʽ��
    /// </summary>
    [Flags]
    public enum EventPoolMode : byte
    {
        /// <summary>
        /// Ĭ���¼���ģʽ���������������ֻ��һ���¼���������
        /// </summary>
        Default = 0,

        /// <summary>
        /// ���������¼���������
        /// </summary>
        AllowNoHandler = 1,

        /// <summary>
        /// ������ڶ���¼���������
        /// </summary>
        AllowMultiHandler = 2,

        /// <summary>
        /// ��������ظ����¼���������
        /// </summary>
        AllowDuplicateHandler = 4
    }
}

