using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public abstract class SiteViewModel : DotvvmViewModelBase
    {
        public abstract string PageTitle { get; }

        public void SignOut()
        {
            // TODO: change to ASP.NET Core equivalent
            FormsAuthentication.SignOut();
            Context.RedirectToLocalUrl("/");
        }
    }
}

