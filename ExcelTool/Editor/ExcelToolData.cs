using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public class ExcelToolData:ScriptableObject
    {
        public string ExcelPath = "/";
        public string CSharpPath = "/";
        public string SaveDataPath = "/";
        public string NameSpace = "";
        public string ClassPrefix = "";
        public int RuleIndex;
        public int DataBuilderIndex;

        public List<ExcelItem> ExcelList = new List<ExcelItem>();

        public int SelectCount
        {
            get
            {
                if(ExcelList.Count == 0)
                {
                    return 0;
                }

                int count = 0;
                foreach (var item in ExcelList)
                {
                    if (item.IsSelect)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public List<ExcelItem> GetSelectItem()
        {
            List<ExcelItem> list = new List<ExcelItem>();
            foreach (var item in ExcelList)
            {
                if (item.IsSelect)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public void SelectAll()
        {
            foreach (var item in ExcelList)
            {
                item.IsSelect = true;
            }
        }

        public void DisSelectAll()
        {
            foreach (var item in ExcelList)
            {
                item.IsSelect = false;
            }
        }

        public void InvertSelect()
        {
            foreach (var item in ExcelList)
            {
                item.IsSelect = !item.IsSelect;
            }
        }

        public void RemoveSelect()
        {
            ExcelList.RemoveAll(x => x.IsSelect);
        }


    }

}
