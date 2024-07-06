using System.Collections.Generic;

namespace GameFramework.Toolkit.Runtime
{
    public class PropertyCollection
    {
        private readonly string m_Name;
        private readonly string m_LanguageKeyword;
        private readonly List<KeyValuePair<int, string>> m_Items;

        public PropertyCollection(string name, string languageKeyword)
        {
            m_Name = name;
            m_LanguageKeyword = languageKeyword;
            m_Items = new List<KeyValuePair<int, string>>();
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        public string LanguageKeyword
        {
            get
            {
                return m_LanguageKeyword;
            }
        }

        public int ItemCount
        {
            get
            {
                return m_Items.Count;
            }
        }

        public KeyValuePair<int, string> GetItem(int index)
        {
            if (index < 0 || index >= m_Items.Count)
            {
                throw new System.Exception(string.Format("GetItem with invalid index '{0}'.", index));
            }

            return m_Items[index];
        }

        public void AddItem(int id, string propertyName)
        {
            m_Items.Add(new KeyValuePair<int, string>(id, propertyName));
        }
    }
}
