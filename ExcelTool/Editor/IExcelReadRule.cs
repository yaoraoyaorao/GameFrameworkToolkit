using NPOI.SS.UserModel;

namespace GameFramework.Toolkit.Editor
{
    public interface IExcelReadRule
    {
        public bool RowRule(IRow row);

        public bool CellRule(ICell cell);
    }
}