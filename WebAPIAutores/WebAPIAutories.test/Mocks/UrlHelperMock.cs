using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebAPIAutories.test.Mocks
{
    public class UrlHelperMock : IUrlHelper
    {
        public string? Action(UrlActionContext actionContext)
        {
            throw new System.NotImplementedException();
        }

        public string? Content(string? contentPath)
        {
            throw new System.NotImplementedException();
        }

        public bool IsLocalUrl(string? url)
        {
            throw new System.NotImplementedException();
        }

        public string? RouteUrl(UrlRouteContext routeContext)
        {
            throw new System.NotImplementedException();
        }

        public string? Link(string? routeName, object? values)
        {
            return "";
        }

        public ActionContext ActionContext { get; }
    }
}