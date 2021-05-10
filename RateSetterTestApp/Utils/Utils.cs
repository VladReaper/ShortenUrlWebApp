using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RateSetterTestApp.Utils
{
    public static class Utils
    {
        public static string GetNewShortUrl(out string guid)
        {
            guid = Guid.NewGuid().ToString().Substring(0, 8);

            var uriBuilder = new UriBuilder();
            uriBuilder.Scheme = HttpContext.Current.Request.Url.Scheme;
            uriBuilder.Host = HttpContext.Current.Request.Url.Host;
            uriBuilder.Port = HttpContext.Current.Request.Url.Port;
            uriBuilder.Path = guid;

            return uriBuilder.ToString();
        }
    }

    enum RequestType
    {
        Create = 0,
        Retrieve = 1
    }
}