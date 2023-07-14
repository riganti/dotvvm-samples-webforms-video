using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class LoginViewModel : SiteViewModel
    {
        public override string PageTitle => "Sign In";

        [Required(ErrorMessage = "No user name provided")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "No password provided")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public bool IsError { get; set; }

        public void SignIn()
        {
            if (!Membership.ValidateUser(UserName, Password))
            {
                IsError = true;
                return;
            }

            FormsAuthentication.SetAuthCookie(UserName, RememberMe);
            var url = FormsAuthentication.GetRedirectUrl(UserName, RememberMe);
            Context.RedirectToUrl(url);
        }
    }
}

