using System;
using UnityEngine;

namespace GameFramework.Toolkit.Runtime
{
    public class DefaultJsonHelper : IJsonHelper
    {
        public string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public T ToObject<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public object ToObject(Type type, string json)
        {
            return JsonUtility.FromJson(json, type);
        }
    }
}

