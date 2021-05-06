using System.IO;
using System.Net;

namespace Web_Service.Helpers
{
    static public class Req_h
    {
        static public string Resp(string url_data)
        {
            string data;

            try
            {
                HttpWebRequest Request_data = (HttpWebRequest)WebRequest.Create(url_data);
                HttpWebResponse Response_data = (HttpWebResponse)Request_data.GetResponse();

                using (StreamReader streamReader = new StreamReader(Response_data.GetResponseStream()))
                {
                    data = streamReader.ReadToEnd();
                }

            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }

            return data;

        }
    }
}