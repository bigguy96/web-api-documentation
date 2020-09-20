using System.Threading.Tasks;

namespace WebApiDocumentationWebApplication.Utilities
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
//https://ppolyzos.com/2016/09/09/asp-net-core-render-view-to-string/
//https://www.learnrazorpages.com/advanced/render-partial-to-string