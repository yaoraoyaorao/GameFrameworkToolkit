using System;

namespace GameFramework.Toolkit.Runtime
{
    /// <summary>
    /// ������
    /// </summary>
    /// <typeparam name="T">�������͡�</typeparam>
    public abstract class Variable<T> : Variable
    {
        private T m_Value;

        /// <summary>
        /// ��ʼ����������ʵ����
        /// </summary>
        public Variable()
        {
            m_Value = default(T);
        }

        /// <summary>
        /// ��ȡ�������͡�
        /// </summary>
        public override Type Type
        {
            get
            {
                return typeof(T);
            }
        }

        /// <summary>
        /// ��ȡ�����ñ���ֵ��
        /// </summary>
        public T Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

        /// <summary>
        /// ��ȡ����ֵ��
        /// </summary>
        /// <returns>����ֵ��</returns>
        public override object GetValue()
        {
            return m_Value;
        }

        /// <summary>
        /// ���ñ���ֵ��
        /// </summary>
        /// <param name="value">����ֵ��</param>
        public override void SetValue(object value)
        {
            m_Value = (T)value;
        }

        /// <summary>
        /// �������ֵ��
        /// </summary>
        public override void Clear()
        {
            m_Value = default(T);
        }

        /// <summary>
        /// ��ȡ�����ַ�����
        /// </summary>
        /// <returns>�����ַ�����</returns>
        public override string ToString()
        {
            return (m_Value != null) ? m_Value.ToString() : "<Null>";
        }
    }

}
