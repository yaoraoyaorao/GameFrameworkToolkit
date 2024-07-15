using Newtonsoft.Json;
using System;

namespace GameFramework.Toolkit.Runtime
{
    public class NewtonsoftJsonHelper : IJsonHelper
    {
        //JsonSerializerSettings settings = new JsonSerializerSettings
        //{
        //    TypeNameHandling = TypeNameHandling.All
        //};

        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj); ;
        }

        public T ToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public object ToObject(Type type, string json)
        {
            return JsonConvert.DeserializeObject(json, type);
        }
    }

}
