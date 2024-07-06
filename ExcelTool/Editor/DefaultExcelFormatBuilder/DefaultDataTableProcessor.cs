using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public delegate void DefaultDataTableCodeGenerator(DefaultDataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData);

    public class DefaultDataTableProcessor : DataTableProcessor
    {
        private const string CommentLineSeparator = "#";
        private string m_CodeTemplate;
        private DefaultDataTableCodeGenerator m_CodeGenerator;

        public DefaultDataTableProcessor(string dataTableName, string[][] rawData, int nameRow, int typeRow, int? defaultValueRow, int? commentRow, int contentStartRow, int idColumn) :
            base(dataTableName, rawData, nameRow, typeRow, defaultValueRow, commentRow, contentStartRow, idColumn)
        {
            m_CodeTemplate = null;
            m_CodeGenerator = null;
        }

        /// <summary>
        /// 是否是注释行
        /// </summary>
        /// <param name="rawRow"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public override bool IsCommentRow(int rawRow)
        {
            if (rawRow < 0 || rawRow >= RawRowCount)
            {
                throw new Exception(string.Format("Raw row '{0}' is out of range.", rawRow));
            }

            return GetValue(rawRow, 0).StartsWith(CommentLineSeparator, StringComparison.Ordinal);
        }

        public override bool IsCommentColumn(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception(string.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return string.IsNullOrEmpty(GetName(rawColumn)) || DataProcessors[rawColumn].IsComment;
        }

        /// <summary>
        /// 是否是Id
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public override bool IsIdColumn(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception(string.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return DataProcessors[rawColumn].IsId;
        }

        /// <summary>
        /// 获取字段名
        /// </summary>
        /// <param name="rawColumn"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public override string GetName(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception(string.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            if (IsIdColumn(rawColumn))
            {
                return "Id";
            }

            return NameRow[rawColumn];
        }

        public override bool IsSystem(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception(string.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return DataProcessors[rawColumn].IsSystem;
        }

        public override System.Type GetType(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception(string.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return DataProcessors[rawColumn].Type;
        }

        public override string GetLanguageKeyword(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception(string.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return DataProcessors[rawColumn].LanguageKeyword;
        }

        public override string GetDefaultValue(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception(string.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return DefaultValueRow != null ? DefaultValueRow[rawColumn] : null;
        }

        public override string GetComment(int rawColumn)
        {
            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception(string.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return CommentRow != null ? CommentRow[rawColumn] : null;
        }

        public override string GetValue(int rawRow, int rawColumn)
        {
            if (rawRow < 0 || rawRow >= RawRowCount)
            {
                throw new Exception(string.Format("Raw row '{0}' is out of range.", rawRow));
            }

            if (rawColumn < 0 || rawColumn >= RawColumnCount)
            {
                throw new Exception(string.Format("Raw column '{0}' is out of range.", rawColumn));
            }

            return RawValues[rawRow][rawColumn];
        }

        /// <summary>
        /// 设置代码模板
        /// </summary>
        /// <param name="codeTemplateFileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public bool SetCodeTemplate(string codeTemplateFileName, Encoding encoding)
        {
            try
            {
                m_CodeTemplate = File.ReadAllText(codeTemplateFileName, encoding);
                Debug.Log(string.Format("Set code template '{0}' success.", codeTemplateFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(string.Format("Set code template '{0}' failure, exception is '{1}'.", codeTemplateFileName, exception));
                return false;
            }
        }

        public void SetCodeGenerator(DefaultDataTableCodeGenerator codeGenerator)
        {
            m_CodeGenerator = codeGenerator;
        }

        public bool GenerateCodeFile(string outputFileName, Encoding encoding, object userData = null)
        {
            if (string.IsNullOrEmpty(m_CodeTemplate))
            {
                throw new Exception("You must set code template first.");
            }

            if (string.IsNullOrEmpty(outputFileName))
            {
                throw new Exception("Output file name is invalid.");
            }

            try
            {
                StringBuilder stringBuilder = new StringBuilder(m_CodeTemplate);
                if (m_CodeGenerator != null)
                {
                    m_CodeGenerator(this, stringBuilder, userData);
                }

                using (FileStream fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter stream = new StreamWriter(fileStream, encoding))
                    {
                        stream.Write(stringBuilder.ToString());
                    }
                }

                Debug.Log(string.Format("Generate code file '{0}' success.", outputFileName));
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(string.Format("Generate code file '{0}' failure, exception is '{1}'.", outputFileName, exception));
                return false;
            }
        }
    }
}
