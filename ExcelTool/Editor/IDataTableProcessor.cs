using GameFramework.Toolkit.Runtime;

namespace GameFramework.Toolkit.Editor
{
    public interface IDataTableProcessor
    {
        /// <summary>
        /// 原始行数
        /// </summary>
        public int RawRowCount
        {
            get;
        }

        /// <summary>
        /// 原始列数
        /// </summary>
        public int RawColumnCount
        {
            get;
        }

        /// <summary>
        /// 内容开始行
        /// </summary>
        public int ContentStartRow
        {
            get;
        }

        /// <summary>
        /// Id开始行
        /// </summary>
        public int IdColumn
        {
            get;
        }

        /// <summary>
        /// 数据表名
        /// </summary>
        public string DataTableName
        {
            get;
        }

        /// <summary>
        /// 原数据
        /// </summary>
        public string[][] RawValues
        {
            get;
        }

        /// <summary>
        /// 名称行
        /// </summary>
        public string[] NameRow
        {
            get;
        }

        /// <summary>
        /// 类型行
        /// </summary>
        public string[] TypeRow
        {
            get;
        }

        /// <summary>
        /// 默认值行
        /// </summary>
        public string[] DefaultValueRow
        {
            get;
        }

        /// <summary>
        /// 注释行
        /// </summary>
        public string[] CommentRow
        {
            get;
        }

        /// <summary>
        /// 数据处理列表
        /// </summary>
        public DataProcessor[] DataProcessors
        {
            get;
        }

        /// <summary>
        /// 是否是注释行
        /// </summary>
        /// <param name="rawRow"></param>
        /// <returns></returns>
        public bool IsCommentRow(int rawRow);

        /// <summary>
        /// 是否是注释列
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public bool IsCommentColumn(int rawColumn);

        /// <summary>
        /// 是否是Id行
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public bool IsIdColumn(int rawColumn);

        /// <summary>
        /// 是否是系统变量
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public bool IsSystem(int rawColumn);

        /// <summary>
        /// 获取数据名
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public string GetName(int rawColumn);

        /// <summary>
        /// 获取数据类型
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public System.Type GetType(int rawColumn);

        /// <summary>
        /// 获取语言关键字
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public string GetLanguageKeyword(int rawColumn);

        /// <summary>
        /// 获取默认数据
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public string GetDefaultValue(int rawColumn);

        /// <summary>
        /// 获取注释
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public string GetComment(int rawColumn);

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="rawRow"></param>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public string GetValue(int rawRow, int rawColumn);
    }
}
