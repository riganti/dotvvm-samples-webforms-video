using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using DotVVM.Framework.Hosting;

namespace Altairis.VtipBaze.WebCore
{
    public static class Extensions
    {
        public static string GetApplicationBaseUri(this IDotvvmRequestContext context)
        {
            var url = context.HttpContext.Request.Url;
            var uriBuilder = new UriBuilder(url.Scheme, url.Host, url.Port);
            if (uriBuilder.Uri.IsDefaultPort)
            {
                uriBuilder.Port = -1;
            }
            uriBuilder.Path = context.HttpContext.Request.PathBase.Value ?? "";
            return uriBuilder.ToString();
        }
    }
}