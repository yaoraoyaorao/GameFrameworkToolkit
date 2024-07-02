using NPOI.SS.UserModel;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public class DefaultRule : IExcelReadRule
    {
        public bool CellRule(ICell cell)
        {
            Debug.Log(cell.ToString());
            return true;
        }

        public bool RowRule(IRow row)
        {
            ICell firstCell = row.GetCell(0);
            if (firstCell != null && firstCell.CellType == CellType.String && firstCell.StringCellValue.StartsWith("#"))
            {
                return false;
            }

            return true;
        }
    }
}

