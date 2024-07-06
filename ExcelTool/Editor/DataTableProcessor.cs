using GameFramework.Toolkit.Runtime;
using System;

namespace GameFramework.Toolkit.Editor
{
    public abstract class DataTableProcessor : IDataTableProcessor
    {
        private string[] m_NameRow;
        private string[] m_TypeRow;
        private string[] m_DefaultValueRow;
        private string[] m_CommentRow;
        private string[][] m_RawValues;
        private int m_ContentStartRow;
        private int m_IdColumn;
        private string m_DataTableName;
        private int m_RawColumnCount;
        private DataProcessor[] m_DataProcessor;

        public virtual int RawRowCount
        {
            get
            {
                return m_RawValues.Length;
            }
        }

        public virtual int RawColumnCount
        {
            get
            {
                return m_RawColumnCount;
            }
        }

        public virtual int ContentStartRow
        {
            get
            {
                return m_ContentStartRow;
            }
        }

        public virtual int IdColumn
        {
            get
            {
                return m_IdColumn;
            }
        }

        public virtual string DataTableName
        {
            get
            {
                return m_DataTableName;
            }
        }

        public virtual string[][] RawValues
        {
            get
            {
                return m_RawValues;
            }
        }

        public virtual string[] NameRow 
        {
            get
            {
                return m_NameRow;
            }
        }

        public virtual string[] TypeRow
        {
            get
            {
                return m_TypeRow;
            }
        }

        public virtual string[] DefaultValueRow
        {
            get
            {
                return m_DefaultValueRow;
            }
        }

        public virtual string[] CommentRow
        {
            get
            {
                return m_CommentRow;
            }
        }

        public virtual DataProcessor[] DataProcessors
        {
            get
            {
                return m_DataProcessor;
            }
        }

        protected DataTableProcessor(string dataTableName, string[][] rawData, int nameRow, int typeRow, int? defaultValueRow, int? commentRow, int contentStartRow, int idColumn)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                throw new Exception("Data table file name is invalid.");
            }
            if (nameRow < 0)
            {
                throw new Exception(string.Format("Name row '{0}' is invalid.", nameRow));
            }

            if (typeRow < 0)
            {
                throw new Exception(string.Format("Type row '{0}' is invalid.", typeRow));
            }

            if (contentStartRow < 0)
            {
                throw new Exception(string.Format("Content start row '{0}' is invalid.", contentStartRow));
            }

            if (idColumn < 0)
            {
                throw new Exception(string.Format("Id column '{0}' is invalid.", idColumn));
            }

            int rawColumnCount = rawData[nameRow].Length;
            m_RawValues = rawData;
            m_RawColumnCount = rawColumnCount;
            m_DataTableName = dataTableName;
            m_NameRow = m_RawValues[nameRow];
            m_TypeRow = m_RawValues[typeRow];
            m_DefaultValueRow = defaultValueRow.HasValue ? m_RawValues[defaultValueRow.Value] : null;
            m_CommentRow = commentRow.HasValue ? m_RawValues[commentRow.Value] : null;
            m_ContentStartRow = contentStartRow;
            m_IdColumn = idColumn;

            m_DataProcessor = new DataProcessor[rawColumnCount];
            for (int i = 0; i < rawColumnCount; i++)
            {
                if (i == IdColumn)
                {
                    m_DataProcessor[i] = DataProcessorUtility.GetDataProcessor("id");
                }
                else
                {
                    m_DataProcessor[i] = DataProcessorUtility.GetDataProcessor(m_TypeRow[i]);
                }
            }
        }

        public abstract bool IsCommentRow(int rawRow);

        public abstract bool IsCommentColumn(int rawColumn);

        public abstract bool IsIdColumn(int rawColumn);

        public abstract bool IsSystem(int rawColumn);

        public abstract string GetName(int rawColumn);

        public abstract Type GetType(int rawColumn);

        public abstract string GetLanguageKeyword(int rawColumn);

        public abstract string GetDefaultValue(int rawColumn);

        public abstract string GetComment(int rawColumn);

        public abstract string GetValue(int rawRow, int rawColumn);
    }
}
