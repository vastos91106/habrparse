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

        private HttpRequestBase Request;

        public HabrBlogParse(HttpRequestBase request)
        {
            this.Request = request;
        }

        public string Parse()
        {
            var url = "https://habr.ru";

            if (Request.Url.Segments.Count() > 1)
            {
                url += Request.Url.AbsolutePath;
            }

            var html = new HtmlDownloader().GetAsString(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var document = htmlDocument.DocumentNode;

            SetTM(document);

            SetHref(document);

            return document.InnerHtml;
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

                foreach (var word in Regex.Split(node.InnerHtml, @"\s+",RegexOptions.Compiled))
                {
                    var match = Regex.Matches(word, "[a-zа-яA-ZА-Я]", RegexOptions.Compiled);
                    if (match.Count == 6)
                    {
                        var index = 0;
                        foreach (Match t in match)
                        {
                            index += 1;
                            if (index == 6)
                            {
                                var item = word;
                                item = $"{item.Insert(t.Index + 1, "™")} " ;
                                phrase.Append(item);
                            }
                        }
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