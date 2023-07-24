using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using System.Web.Security;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public abstract class SiteViewModel : DotvvmViewModelBase
    {
        public abstract string PageTitle { get; }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
            Context.RedirectToLocalUrl("/");
        }
    }
}

