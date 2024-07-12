using System;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// ����
    /// </summary>
    public abstract class Variable : IReference
    {
        /// <summary>
        /// ��ʼ����������ʵ����
        /// </summary>
        public Variable()
        {
        }

        /// <summary>
        /// ��ȡ�������͡�
        /// </summary>
        public abstract Type Type
        {
            get;
        }

        /// <summary>
        /// ��ȡ����ֵ��
        /// </summary>
        /// <returns>����ֵ��</returns>
        public abstract object GetValue();

        /// <summary>
        /// ���ñ���ֵ��
        /// </summary>
        /// <param name="value">����ֵ��</param>
        public abstract void SetValue(object value);

        /// <summary>
        /// �������ֵ��
        /// </summary>
        public abstract void Clear();
    }

}
