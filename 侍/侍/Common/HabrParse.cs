namespace 侍.Common
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    using Fizzler.Systems.HtmlAgilityPack;

    using HtmlAgilityPack;

    public class HabrBlogParse
    {
        private int wordCount = 0;

        private string CompanyName;

        private int PostID;

        private HttpRequestBase Request;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="companyName"></param>
        /// <param name="request"></param>
        public HabrBlogParse(int postId, string companyName, HttpRequestBase request)
        {
            this.PostID = postId;
            this.CompanyName = companyName;
            this.Request = request;
        }

        public string Parse()
        {
            var html = new HtmlDownloader().GetAsString($"https://habrahabr.ru/company/{CompanyName}/blog/{PostID}/");

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var document = htmlDocument.DocumentNode;

            var nodes = document.QuerySelectorAll(".post__body").First();

            var wordCount = 0;

            SetTM(nodes);

            SetHref(nodes);

            return nodes.InnerHtml;
        }

        /// <summary>
        /// замена у всех ссылок (href) корненового домена на текущий http://habrahabr.ru/company/yandex/blog/258673/ => http://localhost/company/yandex/blog/258673/
        /// </summary>
        private void SetHref(HtmlNode node)
        {
            foreach (var href in node.SelectNodes("//a[@href]"))
            {
                var url = Request.Url;
                var pattern = @"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)*";

                var rgx = new Regex(pattern);
                href.Attributes.FirstOrDefault().Value = rgx.Replace(href.Attributes.FirstOrDefault().Value, $"{url.Scheme}://{url.Authority}");
            }
        }

        /// <summary>
        /// устанавливает после каждого слова из шести букв  значок «™»
        /// </summary>
        /// <param name="node"></param>
        private void SetTM(HtmlNode node)
        {
            if (node.NodeType == HtmlNodeType.Text)
            {
                var phrase = new StringBuilder();

                //todo учет символов ;,: итд
                foreach (var word in node.InnerHtml.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (word.Length == 6)
                    {
                        var item = $"{word}™ ";
                        phrase.Append(item);
                        continue;
                    }
                    else
                    {
                        phrase.Append($"{word} ");
                    }
                }

                node.InnerHtml = phrase.ToString();
            }

            if (node.ChildNodes != null)
                foreach (var item in node.ChildNodes)
                {
                    SetTM(item);
                }
        }
    }
}