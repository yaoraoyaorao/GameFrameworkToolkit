using GameFramework.Toolkit.Runtime;

namespace GameFramework.Toolkit.Editor
{
    public interface IDataTableProcessor
    {
        /// <summary>
        /// ԭʼ����
        /// </summary>
        public int RawRowCount
        {
            get;
        }

        /// <summary>
        /// ԭʼ����
        /// </summary>
        public int RawColumnCount
        {
            get;
        }

        /// <summary>
        /// ���ݿ�ʼ��
        /// </summary>
        public int ContentStartRow
        {
            get;
        }

        /// <summary>
        /// Id��ʼ��
        /// </summary>
        public int IdColumn
        {
            get;
        }

        /// <summary>
        /// ���ݱ���
        /// </summary>
        public string DataTableName
        {
            get;
        }

        /// <summary>
        /// ԭ����
        /// </summary>
        public string[][] RawValues
        {
            get;
        }

        /// <summary>
        /// ������
        /// </summary>
        public string[] NameRow
        {
            get;
        }

        /// <summary>
        /// ������
        /// </summary>
        public string[] TypeRow
        {
            get;
        }

        /// <summary>
        /// Ĭ��ֵ��
        /// </summary>
        public string[] DefaultValueRow
        {
            get;
        }

        /// <summary>
        /// ע����
        /// </summary>
        public string[] CommentRow
        {
            get;
        }

        /// <summary>
        /// ���ݴ����б�
        /// </summary>
        public DataProcessor[] DataProcessors
        {
            get;
        }

        /// <summary>
        /// �Ƿ���ע����
        /// </summary>
        /// <param name="rawRow"></param>
        /// <returns></returns>
        public bool IsCommentRow(int rawRow);

        /// <summary>
        /// �Ƿ���ע����
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public bool IsCommentColumn(int rawColumn);

        /// <summary>
        /// �Ƿ���Id��
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public bool IsIdColumn(int rawColumn);

        /// <summary>
        /// �Ƿ���ϵͳ����
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public bool IsSystem(int rawColumn);

        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public string GetName(int rawColumn);

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public System.Type GetType(int rawColumn);

        /// <summary>
        /// ��ȡ���Թؼ���
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public string GetLanguageKeyword(int rawColumn);

        /// <summary>
        /// ��ȡĬ������
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public string GetDefaultValue(int rawColumn);

        /// <summary>
        /// ��ȡע��
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public string GetComment(int rawColumn);

        /// <summary>
        /// ��ȡֵ
        /// </summary>
        /// <param name="rawRow"></param>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        public string GetValue(int rawRow, int rawColumn);
    }
}
