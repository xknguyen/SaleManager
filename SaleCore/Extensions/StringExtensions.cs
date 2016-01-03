using System.Text;
using HtmlAgilityPack;

namespace SaleCore.Extensions
{
    public static class StringExtensions
    {
        public static string GetHtmlContent(this string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            StringBuilder sb = new StringBuilder();
            foreach (var htmlNode in doc.DocumentNode.SelectNodes("//text()"))
            {
                var node = (HtmlTextNode) htmlNode;
                sb.AppendLine(node.Text);
            }
            return sb.ToString();
        }
    }
}
