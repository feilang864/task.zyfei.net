using System.Collections.Generic;
using FFLTask.SRV.ViewModel.Project;
using System.Text;

namespace System.Web.Mvc.Html
{
    public static class HtmlExtension
    {
        public static MvcHtmlString GetText(this _LiteralLinkedModel model)
        {
            string str = string.Empty;
            foreach (var project in model.LinkedList)
            {
                str += project.Name;
                if (project != model.LinkedList.Last.Value)
                {
                    str += ">>";
                }
            }

            return new MvcHtmlString(str);
        }

        public static IEnumerable<SelectListItem> SelectList(
            this IEnumerable<_LiteralLinkedModel> projects)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var project in projects)
            {
                SelectListItem item = new SelectListItem
                {
                    Text = project.GetText().ToHtmlString(),
                    Value = project.TailedId.ToString()
                };
                items.Add(item);
            }

            return items;
        }

        public static MvcHtmlString DocumentLink(
            this HtmlHelper helper,
            string relativePath,
            string linkText)
        {
            return helper.FullLink("doc.zyfei.net",
                relativePath,
                linkText,
                new Dictionary<string, object> { { "target", "_blank" } });
        }

        public static MvcHtmlString FullLink(
            this HtmlHelper helper,
            string domain,
            string relativePath,
            string linkText,
            IDictionary<string, object> htmlAttributes = null)
        {
            return helper.FullLink("http://", 
                domain, relativePath, linkText, htmlAttributes);
        }

        public static MvcHtmlString FullLink(
            this HtmlHelper helper,
            string header,
            string domain,
            string relativePath,  
            string linkText,
            IDictionary<string, object> htmlAttributes = null)
        {
            var output = new StringBuilder();

            TagBuilder tagBuilder = new TagBuilder("a");

            string path = header + domain + relativePath;

            tagBuilder.MergeAttribute("href", path);
            tagBuilder.MergeAttributes(htmlAttributes);

            tagBuilder.InnerHtml = linkText;

            output.Append(tagBuilder.ToString());

            return new MvcHtmlString(output.ToString());
        }
    }
}