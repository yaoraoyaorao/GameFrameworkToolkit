using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public class DefaultExcelFormatBuilder : IExcelFormatBuilder
    {
        private static readonly Regex NameRegex = new Regex(@"^[A-Z][A-Za-z0-9_]*$");

        private string CSharpCodeTemplateFileName
        {
            get
            {
                string path = ExcelUtility.GetPath() + "/Editor/DefaultExcelFormatBuilder/DataTableCodeTemplate.txt";

                return path;
            }
        }

        private ExcelToolData m_Data;

        public void FormateBuilder(string sheetName, string[][] data, IExcelDataBuilder[] dataBuilders)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                Debug.LogError("DefaultExcelFormatBuilder Error: 工作表名称为空");
                return;
            }

            if (!NameRegex.IsMatch(sheetName))
            {
                Debug.LogError("DefaultExcelFormatBuilder Error: 工作表名不能有中文或其它特殊符号");
                return;
            }

            if (data == null || data.Length == 0)
            {
                Debug.LogError("DefaultExcelFormatBuilder Error: 数据为空");
                return;
            }

            m_Data = ExcelToolAsset.GetAsset;

            DefaultDataTableProcessor defaultDataTableProcessor = new DefaultDataTableProcessor(sheetName, data, 1, 2, null, 3, 4, 1);

            if (!CheckRawData(defaultDataTableProcessor))
            {
                Debug.LogError(string.Format("DefaultExcelFormatBuilder Error: Check raw data failure. DataTableName='{0}'", sheetName));
                return;
            }

            GenerateCodeFile(defaultDataTableProcessor);

            AssetDatabase.Refresh();

            if (dataBuilders != null && dataBuilders.Length > 0)
            {
                foreach (var dataBuilder in dataBuilders)
                {
                    dataBuilder.DataBuilder(defaultDataTableProcessor);
                }
            }
        }

        private bool CheckRawData(DefaultDataTableProcessor dataTableProcessor)
        {
            for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                string name = dataTableProcessor.GetName(i);
                if (string.IsNullOrEmpty(name) || name == "#")
                {
                    continue;
                }

                if (!NameRegex.IsMatch(name))
                {
                    Debug.LogWarning(string.Format("Check raw data failure. DataTableName='{0}' Name='{1}'", dataTableProcessor.DataTableName, name));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 生成代码文件
        /// </summary>
        /// <param name="dataTableProcessor"></param>
        private void GenerateCodeFile(DefaultDataTableProcessor dataTableProcessor)
        {
            dataTableProcessor.SetCodeTemplate(CSharpCodeTemplateFileName, Encoding.UTF8);
            dataTableProcessor.SetCodeGenerator(DataTableCodeGenerator);

            string csharpCodeFileName = m_Data.CSharpPath + "/" + m_Data.ClassPrefix + dataTableProcessor.DataTableName + ".cs";
            if (!dataTableProcessor.GenerateCodeFile(csharpCodeFileName, Encoding.UTF8) && File.Exists(csharpCodeFileName))
            {
                File.Delete(csharpCodeFileName);
            }
        }

        private void DataTableCodeGenerator(DefaultDataTableProcessor dataTableProcessor, StringBuilder codeContent, object userData)
        {

            codeContent.Replace("__DATA_TABLE_CREATE_TIME__", DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            codeContent.Replace("__DATA_TABLE_NAME_SPACE__", m_Data.NameSpace);
            codeContent.Replace("__DATA_TABLE_CLASS_NAME__", m_Data.ClassPrefix + dataTableProcessor.DataTableName);
            codeContent.Replace("__DATA_TABLE_COMMENT__", dataTableProcessor.GetValue(0, 1) + "。");
            codeContent.Replace("__DATA_TABLE_ID_COMMENT__", "获取" + dataTableProcessor.GetComment(dataTableProcessor.IdColumn) + "。");
            codeContent.Replace("__DATA_TABLE_PROPERTIES__", GenerateDataTableProperties(dataTableProcessor));
        }

        private string GenerateDataTableProperties(DefaultDataTableProcessor dataTableProcessor)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool firstProperty = true;
            for (int i = 0; i < dataTableProcessor.RawColumnCount; i++)
            {
                if (dataTableProcessor.IsCommentColumn(i))
                {
                    // 注释列
                    continue;
                }

                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    stringBuilder.AppendLine().AppendLine();
                }

                stringBuilder
                   .AppendLine("        /// <summary>")
                   .AppendFormat("        /// 获取{0}。", dataTableProcessor.GetComment(i)).AppendLine()
                   .AppendLine("        /// </summary>")
                   .AppendFormat("        public {0} {1}", dataTableProcessor.GetLanguageKeyword(i), dataTableProcessor.GetName(i)).AppendLine()
                   .AppendLine("        {")
                   .AppendLine("            get;")
                   .AppendLine("            set;")
                   .Append("        }");
            }

            return stringBuilder.ToString();
        }
    }
}

