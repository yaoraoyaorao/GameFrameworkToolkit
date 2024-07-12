using System;

namespace GameFramework.Toolkit.Runtime
{
    public interface IXmlHelper
    {
        public void ToXml(string path, object obj);

        public object ToObject(Type type, string path);

        public T ToObject<T>(string path) where T : class;
    }

}
