namespace GameFramework.Toolkit.Editor
{
    public interface IExcelFormatBuilder
    {
        public void FormateBuilder(string sheetName, string[][] data, IExcelDataBuilder[] dataBuilders);
    }
}