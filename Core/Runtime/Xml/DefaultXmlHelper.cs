using System;
using System.IO;
using System.Xml.Serialization;

namespace GameFramework.Toolkit.Runtime
{
    public class DefaultXmlHelper : IXmlHelper
    {
        public object ToObject(Type type, string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                XmlSerializer s = new XmlSerializer(type);

                return s.Deserialize(reader);
            }
        }

        public T ToObject<T>(string path)where T : class
        {
            using (StreamReader reader = new StreamReader(path))
            {
                XmlSerializer s = new XmlSerializer(typeof(T));

                return s.Deserialize(reader) as T;
            }
        }

        public void ToXml(string path, object obj)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                XmlSerializer s = new XmlSerializer(obj.GetType());
                s.Serialize(writer, obj);
            }
        }
    }
}

