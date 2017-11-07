using System;
using UnityEngine;

namespace Scripting.SimpleJson
{
    public class JsonHelper
    {
        public static T[] GetJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        public static string ToJsonArray<T>(T[] objs)
        {
            Wrapper<T> wrapper = new Wrapper<T>() { array = objs };
            return JsonUtility.ToJson(wrapper);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] array = null;
        }
    }
}
