using Altairis.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Altairis.VtipBaze.WebCore
{
    public class Helpers
    {

        public static Uri GetApplicationBaseUri()
        {
            return HttpContext.Current.Request.ApplicationBaseUri();
        }

    }
}