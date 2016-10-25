namespace 侍.Common
{
    using System.IO;
    using System.Net;

    public class HtmlDownloader
    {
        /// <summary>
        /// Возвращает по url, html документ как строку
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetAsString(string url)
        {
            var data = string.Empty;
            var client = new WebClient();
            var stream = client.OpenRead(url);
            var sr = new StreamReader(stream);
            string newLine;
            while ((newLine = sr.ReadLine()) != null)
                data += newLine;
            stream.Close();
            return data;
        }
    }
}