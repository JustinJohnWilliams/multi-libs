using System;
using System.Linq;
using System.IO;
using System.Json;
using Newtonsoft.Json.Linq;

namespace RestfulAdapter
{
    public class RestConverter
    {
        public object ConstructObject<T>(Stream stream) where T : class, new()
        {
				return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(new StreamReader(stream).ReadToEnd());
            
        }

//        public object ToDictionary(JToken token)
//        {
//            if (token is JObject)
//            {
//                Hash dictionary = new Hash();
//
//                (from childToken in ((JToken)token) where childToken is JProperty select childToken as JProperty).ToList().ForEach(property =>
//                {
//                    dictionary.Add(property.Name, ToDictionary(property.Value));
//                });
//
//                return dictionary;
//            }
//
//            if (token is JValue)
//            {
//                return ((JValue)token).Value;
//            }
//
//            if (token is JArray)
//            {
//                object[] array = new object[((JArray)token).Count];
//                int index = 0;
//                foreach (JToken arrayItem in ((JArray)token))
//                {
//                    array[index] = ToDictionary(arrayItem);
//                    index++;
//                }
//
//                return array;
//            }
//
//            throw new ArgumentException(string.Format("Unknown token type '{0}'", token.GetType()), "token");
//        }
    }
}
