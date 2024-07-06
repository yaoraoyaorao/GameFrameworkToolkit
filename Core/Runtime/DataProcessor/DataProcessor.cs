using System.IO;

namespace GameFramework.Toolkit.Runtime
{
    public abstract class DataProcessor
    {
        /// <summary>
        /// 类型
        /// </summary>
        public abstract System.Type Type
        {
            get;
        }

        /// <summary>
        /// 是否为Id
        /// </summary>
        public abstract bool IsId
        {
            get;
        }

        /// <summary>
        /// 是否为评论
        /// </summary>
        public abstract bool IsComment
        {
            get;
        }

        /// <summary>
        /// 是否为系统类型
        /// </summary>
        public abstract bool IsSystem
        {
            get;
        }

        /// <summary>
        /// 关键词
        /// </summary>
        public abstract string LanguageKeyword
        {
            get;
        }

        public abstract string[] GetTypeStrings();

        public abstract void WriteToStream(BinaryWriter binaryWriter, string value);
    }
}
