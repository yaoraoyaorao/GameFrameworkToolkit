using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

namespace GameFramework.Toolkit.Editor
{
    public static class ExcelUtility
    {
        public const string PackageFullName = "com.rcy.gameframework-toolkit-exceltool";
        public const string PackageDisplayName = "ExcelTool";

        public static void Read(string path, IExcelReadRule readRule)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read)) 
            {
                IWorkbook workbook = new XSSFWorkbook(stream);

                ISheet sheet = workbook.GetSheetAt(0);

                for (int i = 0; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);

                    if (row == null)
                    {
                        continue;
                    }

                    if (!readRule.RowRule(row))
                    {
                        continue;
                    }

                    for (int j = 0; j < row.LastCellNum; j++)
                    {
                        ICell cell = row.GetCell(j);

                        if (cell == null)
                        {
                            continue;
                        }

                        if (!readRule.CellRule(cell))
                        {
                            continue;
                        }
                        
                    }
                }
            }
        }

        public static string GetPath()
        {
            return Utility.GetPackageRelativePath(PackageFullName, PackageDisplayName);
        }

        public static string GetIconPath(string iconName)
        {
            return Path.Combine(GetPath(), "Editor/Icons", iconName);
        }
    }

}
