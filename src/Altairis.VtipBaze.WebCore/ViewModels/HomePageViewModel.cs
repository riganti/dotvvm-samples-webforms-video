using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Controls;
using Altairis.VtipBaze.WebCore.Model;
using Altairis.VtipBaze.Data;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class HomePageViewModel : SiteViewModel
    {
        private readonly VtipBazeContext dbContext;

        public override string PageTitle => "VtipBáze";

        public bool ShowHeader { get; set; } = false;
        public bool ShowPager { get; set; } = true;

        public GridViewDataSet<JokeListModel> Jokes { get; set; }

        [FromRoute("PageIndex")]
        public int CurrentPageIndex { get; set; } = 1;

        [FromRoute("JokeId")]
        public int? JokeId { get; set; }

        [FromRoute("TagName")]
        public string TagName { get; set; }

        public HomePageViewModel(VtipBazeContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override Task PreRender()
        {
            Jokes = new GridViewDataSet<JokeListModel>()
            {
                PagingOptions = 
                { 
                    PageSize = 6,
                    PageIndex = CurrentPageIndex - 1
                }
            };
            Jokes.LoadFromQueryable(SelectJokes());

            return base.PreRender();
        }

        public IQueryable<JokeListModel> SelectJokes()
        {
            var q = dbContext.Jokes.AsQueryable();

            if (Context.Route.RouteName.Equals("HomePage"))
            {
                q = q.Where(x => x.Approved);
                ShowHeader = true;
            }
            else if (Context.Route.RouteName.Equals("SingleJoke"))
            {
                q = q.Where(x => x.JokeId == JokeId.Value);
                if (!Context.HttpContext.User.Identity.IsAuthenticated) q = q.Where(x => x.Approved);
                ShowPager = false;
            }
            else if (Context.Route.RouteName.Equals("AdminHomePage"))
            {
                q = q.Where(x => !x.Approved);
            }
            else if (Context.Route.RouteName.Equals("TagSearch"))
            {
                if (!string.IsNullOrWhiteSpace(TagName)) q = q.Where(x => x.Tags.Any(t => t.TagName.Equals(TagName)));
            }

            return q.OrderByDescending(x => x.DateCreated)
                .Select(x => new JokeListModel()
                {
                    Id = x.JokeId,
                    Text = x.Text,
                    DateCreated = x.DateCreated,
                    Tags = x.Tags.Select(t => new TagListModel()
                    {
                        TagName = t.TagName
                    })
                });
        }
    }
}

