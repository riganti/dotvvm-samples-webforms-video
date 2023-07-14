using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using Altairis.VtipBaze.Data;
using Altairis.VtipBaze.WebCore.Models;
using System;
using DotVVM.Framework.Controls;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class HomePageViewModel : SiteViewModel
    {
        private readonly VtipBazeContext dbContext;

        public override string PageTitle => "VtipBáze";

        [FromRoute("JokeId")]
        public int? JokeId { get; set; }

        [FromRoute("TagName")]
        public string TagName { get; set; }

        public bool WelcomePlaceHolderVisible { get; set; } = false;

        public bool PagingPanelVisible { get; set; } = true;

        public GridViewDataSet<JokeListModel> Jokes { get; set; }

        public List<string> TagList { get; set; }


        [FromRoute("PageIndex")]
        public int PageIndex { get; set; } = 1;

        public HomePageViewModel(VtipBazeContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override async Task Init()
        {
            if (this.Context.Route.RouteName.Equals("AdminHomePage"))
            {
                await Context.Authorize();
            } 
            await base.Init();
        }

        public override Task PreRender()
        {
            Jokes = new GridViewDataSet<JokeListModel>()
            {
                PagingOptions = { PageSize = 5, PageIndex = PageIndex - 1 },
                SortingOptions = { SortExpression = nameof(JokeListModel.DateCreated), SortDescending = true }
            };
            Jokes.LoadFromQueryable(SelectJokes());

            TagList = SelectTagNames().ToList();

            return base.PreRender();
        }

        public IQueryable<string> SelectTagNames()
        {
            return dbContext.Tags
                .OrderBy(x => x.TagName)
                .Select(x => x.TagName);
        }

        public IQueryable<JokeListModel> SelectJokes()
        {
            var q = this.dbContext.Jokes.AsQueryable();

            if (this.Context.Route.RouteName.Equals("HomePage"))
            {
                q = q.Where(x => x.Approved);
                WelcomePlaceHolderVisible = true;
            }
            else if (this.Context.Route.RouteName.Equals("SingleJoke"))
            {
                q = q.Where(x => x.JokeId == JokeId);
                if (!this.Context.HttpContext.User.Identity.IsAuthenticated) q = q.Where(x => x.Approved);
                PagingPanelVisible = false;
            }
            else if (this.Context.Route.RouteName.Equals("AdminHomePage"))
            {
                q = q.Where(x => !x.Approved);
            }
            else if (this.Context.Route.RouteName.Equals("TagSearch"))
            {
                if (!string.IsNullOrWhiteSpace(TagName))
                {
                    q = q.Where(x => x.Tags.Any(t => t.TagName.Equals(TagName)));
                }
            }

            return q
                .Select(x => new JokeListModel()
                {
                    Id = x.JokeId,
                    Text = x.Text,
                    Approved = x.Approved,
                    DateCreated = x.DateCreated,
                    Tags = x.Tags.Select(t => t.TagName)
                });
        }

        public void AddTag(int jokeId, string newTag)
        {
            var tagText = newTag.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(tagText)) return;

            var tag = this.dbContext.Tags.SingleOrDefault(x => x.TagName.Equals(tagText));
            if (tag == null)
            {
                tag = new Tag { TagName = tagText };
                this.dbContext.Tags.Add(tag);
            }

            var joke = this.dbContext.Jokes.Single(x => x.JokeId == jokeId);
            joke.Tags.Add(tag);

            this.dbContext.SaveChanges();
        }

        public void ClearTags(int jokeId)
        {
            var joke = this.dbContext.Jokes.Single(x => x.JokeId == jokeId);
            joke.Tags.Clear();
            this.dbContext.SaveChanges();
        }

        public void ApproveJoke(int jokeId)
        {
            var joke = this.dbContext.Jokes.Single(x => x.JokeId == jokeId);
            joke.Approved = true;
            this.dbContext.SaveChanges();
        }

        public void RemoveJoke(int jokeId)
        {
            var joke = this.dbContext.Jokes.Single(x => x.JokeId == jokeId);
            this.dbContext.Jokes.Remove(joke);
            this.dbContext.SaveChanges();
        }

    }
}

