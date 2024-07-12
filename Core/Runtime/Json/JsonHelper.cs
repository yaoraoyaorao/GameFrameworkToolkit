using System;

namespace GameFramework.Toolkit.Runtime
{
    public class JsonHelper
    {
        private static IJsonHelper m_JsonHelper = null;

        public static void SetJsonHelper(IJsonHelper jsonHelper)
        {
            m_JsonHelper = jsonHelper;
        }

        public static string ToJson(object obj)
        {
            if (m_JsonHelper == null)
            {
                throw new Exception("Json Helper is invaild");
            }

            try
            {
                return m_JsonHelper.ToJson(obj);
            }
            catch (Exception e)
            {

                throw new Exception("Json Error:" + e.Message);
            }
        }

        public static T ToObject<T>(string json)
        {
            if (m_JsonHelper == null)
            {
                throw new Exception("Json Helper is invaild");
            }

            try
            {
                return m_JsonHelper.ToObject<T>(json);
            }
            catch (Exception e)
            {

                throw new Exception("Json Error:" + e.Message);
            }
        }

        public static object ToObject(Type type, string json)
        {
            if (m_JsonHelper == null)
            {
                throw new Exception("Json Helper is invaild");
            }

            try
            {
                return m_JsonHelper.ToObject(type, json);
            }
            catch (Exception e)
            {

                throw new Exception("Json Error:" + e.Message);
            }
        }
    }
}

