using GameFramework.Toolkit.Runtime;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public class BinaryDataBuilder : IExcelDataBuilder
    {
        private string outputFileName;
        private ExcelToolData m_Data;
        private IDataTableProcessor m_Processor;
        private DataProcessor[] m_DataProcessor;
        public void DataBuilder(IDataTableProcessor dataTableProcessor)
        {
            m_Data = ExcelToolAsset.GetAsset;
            m_Processor = dataTableProcessor;
            m_DataProcessor = m_Processor.DataProcessors;
            outputFileName = m_Data.SaveDataPath + "/" + m_Data.ClassPrefix + dataTableProcessor.DataTableName + ".bytes";
            if (File.Exists(outputFileName))
            {
                File.Delete(outputFileName);
            }
            try
            {
                using (FileStream fileStream = new FileStream(outputFileName, FileMode.Create, FileAccess.Write))
                {
                    using (BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8))
                    {
                        for (int rawRow = m_Processor.ContentStartRow; rawRow < m_Processor.RawRowCount; rawRow++)
                        {
                            if (m_Processor.IsCommentRow(rawRow))
                            {
                                continue;
                            }

                            byte[] bytes = GetRowBytes(outputFileName, rawRow);
                            binaryWriter.Write(bytes.Length);
                            binaryWriter.Write(bytes);
                        }
                    }
                }

                Debug.Log(string.Format("成功解析数据表'{0}'", outputFileName));

                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {

                Debug.LogError(string.Format("解析数据表'{0}'失败，异常为'{1}'", outputFileName, e));
            }
        }

        private byte[] GetRowBytes(string outputFileName, int rawRow)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8))
                {
                    for (int rawColumn = 0; rawColumn < m_Processor.RawColumnCount; rawColumn++)
                    {
                        if (m_Processor.IsCommentColumn(rawColumn))
                        {
                            continue;
                        }

                        try
                        {
                            m_DataProcessor[rawColumn].WriteToStream(binaryWriter, m_Processor.GetValue(rawRow, rawColumn));
                        }
                        catch
                        {
                            if (m_DataProcessor[rawColumn].IsId || string.IsNullOrEmpty(m_Processor.GetDefaultValue(rawColumn)))
                            {
                                Debug.LogError(string.Format("Parse raw value failure. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'", outputFileName, rawRow, rawColumn, m_Processor.GetName(rawColumn), m_Processor.GetLanguageKeyword(rawColumn), m_Processor.GetValue(rawRow, rawColumn)));
                                return null;
                            }
                            else
                            {
                                Debug.LogWarning(string.Format("Parse raw value failure, will try default value. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'", outputFileName, rawRow, rawColumn, m_Processor.GetName(rawColumn), m_Processor.GetLanguageKeyword(rawColumn), m_Processor.GetValue(rawRow, rawColumn)));
                                try
                                {
                                    m_DataProcessor[rawColumn].WriteToStream(binaryWriter, m_Processor.GetDefaultValue(rawColumn));
                                }
                                catch
                                {
                                    Debug.LogError(string.Format("Parse default value failure. OutputFileName='{0}' RawRow='{1}' RowColumn='{2}' Name='{3}' Type='{4}' RawValue='{5}'", outputFileName, rawRow, rawColumn, m_Processor.GetName(rawColumn), m_Processor.GetLanguageKeyword(rawColumn), m_Processor.GetComment(rawColumn)));
                                    return null;
                                }
                            }
                        }
                    }

                    return memoryStream.ToArray();
                }
            }
        }
    }
}
