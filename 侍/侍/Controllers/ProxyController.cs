namespace 侍.Controllers
{
    using System;
    using System.Web.Mvc;

    using 侍.Common;

    public class ProxyController : Controller
    {
        private int wordCount = 0;

        // GET: Proxy
        public string Index()
        {
                try
                {
                    var content = new HabrBlogParse(Request).Parse();
                    return content;
                }
                catch 
                {
                    return "An error has occurred";
                }
        }
    }
}