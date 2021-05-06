using Newtonsoft.Json;

namespace Web_Service.Helpers
{
    static public class JDeserializer<T>
    {
        static public T Deser(in string data)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }           
        }
    }
}