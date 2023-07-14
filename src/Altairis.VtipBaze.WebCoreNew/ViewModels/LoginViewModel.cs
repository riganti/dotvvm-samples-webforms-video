﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class LoginViewModel : SiteViewModel
    {
        private readonly SignInManager<IdentityUser> signInManager;

        public override string PageTitle => "Sign In";

        [Required(ErrorMessage = "No user name provided")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "No password provided")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public bool IsError { get; set; }

        [FromQuery("returnUrl")]
        public string ReturnUrl { get; set; }

        public LoginViewModel(SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public async Task SignIn()
        {
            var result = await signInManager.PasswordSignInAsync(UserName, Password, RememberMe, true);
            if (!result.Succeeded)
            {
                IsError = true;
                return;
            }

            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                Context.RedirectToLocalUrl(ReturnUrl);
            }
            else
            {
                Context.RedirectToRoute("AdminHomePage");
            }
        }
    }
}
