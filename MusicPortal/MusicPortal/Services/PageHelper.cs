using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MusicPortal.Models;

namespace MusicPortal.Services {
    public class PageLinkTagHelper : TagHelper {
        public PageVM? PageModel { get; set; }
        public string PageAction { get; set; } = "";
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new();
        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;
        private IUrlHelperFactory urlHelperFactory;
        public PageLinkTagHelper(IUrlHelperFactory helperFactory) => urlHelperFactory = helperFactory;
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (PageModel == null) throw new Exception("PageModel не установлен!");
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "div";

            TagBuilder tag = new TagBuilder("ul"); // набор ссылок будет представлять список ul
            tag.AddCssClass("pagination");

            // формируем три ссылки - на текущую, предыдущую и следующую
            TagBuilder currentItem = CreateTag(PageModel.PageNumber, urlHelper);

            if (PageModel.HasPreviousPage) { // создаем ссылку на предыдущую страницу, если она есть
                TagBuilder prevItem = CreateTag(PageModel.PageNumber - 1, urlHelper);
                tag.InnerHtml.AppendHtml(prevItem);
            }
            tag.InnerHtml.AppendHtml(currentItem);

            if (PageModel.HasNextPage) { // создаем ссылку на следующую страницу, если она есть
                TagBuilder nextItem = CreateTag(PageModel.PageNumber + 1, urlHelper);
                tag.InnerHtml.AppendHtml(nextItem);
            }
            output.Content.AppendHtml(tag);
        }
        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper) {
            TagBuilder item = new TagBuilder("li"), link = new TagBuilder("a");
            if (pageNumber == PageModel?.PageNumber) item.AddCssClass("active");
            else {
                PageUrlValues["page"] = pageNumber;
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            }
            item.AddCssClass("page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
