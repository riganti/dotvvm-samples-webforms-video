using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altairis.VtipBaze.Data;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Routing;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class NewJokeViewModel : SiteViewModel
    {
        private readonly VtipBazeContext dbContext;

        public override string PageTitle => "Add your joke";
        
        public int Step { get; set; }

        [Required(ErrorMessage = "Empty text is not very funny")]
        public string JokeText { get; set; }

        public NewJokeViewModel(VtipBazeContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Submit()
        {
            var isAuthenticated = Context.HttpContext.User.Identity.IsAuthenticated;

            var joke = new Joke
            {
                Text = JokeText,
                Approved = isAuthenticated
            };
            dbContext.Jokes.Add(joke);
            dbContext.SaveChanges();

            if (isAuthenticated)
            {
                // Published directly
                Context.RedirectToRouteHybrid("SingleJoke", new { JokeId = joke.JokeId });
            }
            else
            {
                // Waiting for approval
                Step = 1;

                // Send message to users
                var recipients = from u in dbContext.Users 
                    select u.Email;
                var client = new System.Net.Mail.SmtpClient();
                var message = new System.Net.Mail.MailMessage()
                {
                    Subject = "New joke to approve",
                    Body = joke.Text + "\r\n\r\nApprove or reject at " + Context.GetApplicationBaseUri() + "admin",
                    IsBodyHtml = false
                };
                message.To.Add(string.Join(",", recipients));
                client.Send(message);
            }
        }
    }
}

