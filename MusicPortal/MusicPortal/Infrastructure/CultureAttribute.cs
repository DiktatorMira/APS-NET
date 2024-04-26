using Microsoft.AspNetCore.Mvc.Filters;
using MusicPortal.BLL.Services;
using System.Globalization;

namespace MusicPortal.Infrastructure {
    public class CultureAttribute : Attribute, IActionFilter {
        public void OnActionExecuted(ActionExecutedContext filterContext) {}
        public void OnActionExecuting(ActionExecutingContext filterContext) {
            string? cultureName = null;
            var cultureCookie = filterContext.HttpContext.Request.Cookies["lang"];
            if (cultureCookie != null) cultureName = cultureCookie;
            else cultureName = "ru";

            List<string> cultures = filterContext.HttpContext.RequestServices.GetRequiredService<ILangService>().languageList().Select(t => t.ShortName).ToList()!;
            if (!cultures.Contains(cultureName)) cultureName = "ru";

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
        }
    }
}
