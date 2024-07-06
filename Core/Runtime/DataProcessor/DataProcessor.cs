using System.IO;

namespace GameFramework.Toolkit.Runtime
{
    public abstract class DataProcessor
    {
        /// <summary>
        /// ����
        /// </summary>
        public abstract System.Type Type
        {
            get;
        }

        /// <summary>
        /// �Ƿ�ΪId
        /// </summary>
        public abstract bool IsId
        {
            get;
        }

        /// <summary>
        /// �Ƿ�Ϊ����
        /// </summary>
        public abstract bool IsComment
        {
            get;
        }

        /// <summary>
        /// �Ƿ�Ϊϵͳ����
        /// </summary>
        public abstract bool IsSystem
        {
            get;
        }

        /// <summary>
        /// �ؼ���
        /// </summary>
        public abstract string LanguageKeyword
        {
            get;
        }

        public abstract string[] GetTypeStrings();

        public abstract void WriteToStream(BinaryWriter binaryWriter, string value);
    }
}
