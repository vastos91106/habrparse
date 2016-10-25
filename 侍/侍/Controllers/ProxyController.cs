namespace 侍.Controllers
{
    using System;
    using System.Web.Mvc;

    using 侍.Common;

    public class ProxyController : Controller
    {
        private int wordCount = 0;

        // GET: Proxy
        public string Index(string companyName, int postID)
        {
            if (string.IsNullOrEmpty(companyName) || postID <= 0)
            {
                return "company or postID is null or invalid";
            }
            else
            {
                try
                {
                    var content = new HabrBlogParse(postID, companyName, Request).Parse();
                    return content;
                }
                catch (Exception)
                {
                    return "An error has occurred";
                }
            }
        }
    }
}