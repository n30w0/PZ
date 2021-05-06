using System.IO;
using System.Text;
using Web_Service.Helpers;

namespace Client_WPF.Helpers
{
    public class ReadWriter<T>
    {
        static private string path = "Favorites.json";
        static public string Read()
        {
            string data;

            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path);
                }

                using (StreamReader sr = new StreamReader(path))
                {
                    data = sr.ReadToEnd();
                }

                return data;
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        static public void Write(T data)
        {
            string current_data;
            try
            {
                current_data = File.ReadAllText(path, Encoding.Default);                    
                                   
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    if (data != null)
                        sw.WriteLine(JWriter<T>.Write(data, current_data));
                    else                   
                        sw.WriteLine();
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }

        }

    }
}