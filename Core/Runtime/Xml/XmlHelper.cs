using System;

namespace GameFramework.Toolkit.Runtime
{
    public static class XmlHelper
    {
        private static IXmlHelper m_XmlHelper;
        
        public static void SetXmlHelper(IXmlHelper xmlHelper)
        {
            m_XmlHelper = xmlHelper;
        }

        public static void ToXml(string path, object obj)
        {
            if (m_XmlHelper == null)
            {
                throw new Exception("Xml Helper is invaild");
            }

            try
            {
                m_XmlHelper.ToXml(path, obj);
            }
            catch (Exception e)
            {
                throw new Exception("Xml error " + e.Message);
            }

        }

        public static object ToObject(Type type, string path)
        {
            if (m_XmlHelper == null)
            {
                throw new Exception("Xml Helper is invaild");
            }

            try
            {
                return m_XmlHelper.ToObject(type, path);
            }
            catch (Exception e)
            {
                throw new Exception("Xml error " + e.Message);
            }
        }

        public static T ToObject<T>(string path) where T : class
        {
            if (m_XmlHelper == null)
            {
                throw new Exception("Xml Helper is invaild");
            }

            try
            {
                return m_XmlHelper.ToObject<T>(path);
            }
            catch (Exception e)
            {
                throw new Exception("Xml error " + e.Message);
            }
        }
    }

}
