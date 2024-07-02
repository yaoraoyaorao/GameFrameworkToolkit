using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Toolkit.Editor
{
    public class ExcelToolData:ScriptableObject
    {
        public string m_ExcelPath = "/";
        public string m_CSharpPath = "/";
        public List<string> m_ExcelList = new List<string>();
    }

}
