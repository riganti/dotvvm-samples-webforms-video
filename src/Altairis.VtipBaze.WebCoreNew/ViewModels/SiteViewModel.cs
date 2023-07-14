using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public abstract class SiteViewModel : DotvvmViewModelBase
    {
        public abstract string PageTitle { get; }

        public async Task SignOut()
        {
            var signInManager = Context.Services.GetRequiredService<SignInManager<IdentityUser>>();
            await signInManager.SignOutAsync();
            
            Context.RedirectToRoute("HomePage");
        }
    }
}

