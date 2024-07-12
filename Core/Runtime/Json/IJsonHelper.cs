using System;

namespace GameFramework.Toolkit.Runtime
{
    public interface IJsonHelper
    {
        public string ToJson(object obj);

        public T ToObject<T>(string json);

        public object ToObject(Type type, string json);
    }
}

