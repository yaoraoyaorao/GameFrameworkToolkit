using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public static class ExcelUtility
    {
        public static readonly string[] RuntimeOrEditorAssemblyNames =
        {
            "GameFramework.Toolkit.Editor",
            "GameFramework.Toolkit.Runtime",
            "GameFramework.Toolkit.ExcelTool.Editor",
            "GameFramework.Toolkit.ExcelTool.Runtime",
            "Assembly-CSharp",
            "Assembly-CSharp-Editor",
        };

        public const string PackageFullName = "com.rcy.gameframework-toolkit-exceltool";
        public const string PackageDisplayName = "ExcelTool";

        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="readRule"></param>
        public static void Read(string path, IExcelFormatBuilder readRule, IExcelDataBuilder[] dataBuilders)
        {
            if (!File.Exists(path))
            {
                Debug.LogError("Excel Error:Excel文件不存在");
                return;
            }

            if (readRule == null)
            {
                Debug.LogError("Excel Error:生成规则为空");
                return;
            }

            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = new XSSFWorkbook(stream);

                    for (int i = 0; i < workbook.NumberOfSheets; i++)
                    {
                        ISheet sheet = workbook.GetSheetAt(i);

                        if (sheet == null) continue;

                        int rowCount = sheet.LastRowNum + 1;
                        string sheetName = sheet.SheetName;
                        string[][] data = new string[rowCount][];

                        for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                        {
                            IRow row = sheet.GetRow(rowIndex);
                            if (row == null) continue;

                            int cellCount = row.LastCellNum;
                            data[rowIndex] = new string[cellCount];

                            for (int cellIndex = 0; cellIndex < cellCount; cellIndex++)
                            {
                                ICell cell = row.GetCell(cellIndex);
                                data[rowIndex][cellIndex] = cell != null ? cell.ToString() : "";
                            }
                        }

                        readRule.FormateBuilder(sheetName, data, dataBuilders);
                    }
                }
            }
            catch (System.Exception e)
            {

                Debug.LogError("Excel Error:" + e.Message);
            }
           
        }

        /// <summary>
        /// 获取插件的相对路径
        /// </summary>
        /// <returns></returns>
        public static string GetPath()
        {
            return Utility.GetPackageRelativePath(PackageFullName, PackageDisplayName);
        }

        /// <summary>
        /// 获取插件的图标路径
        /// </summary>
        /// <param name="iconName"></param>
        /// <returns></returns>
        public static string GetIconPath(string iconName)
        {
            return Path.Combine(GetPath(), "Editor/Icons", iconName);
        }
    }

}
