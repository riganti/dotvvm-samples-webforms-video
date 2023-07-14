using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using Altairis.VtipBaze.Data;
using Altairis.VtipBaze.WebCore.Models;

namespace Altairis.VtipBaze.WebCore.ViewModels
{
    public class TagListViewModel : SiteViewModel
    {
        private readonly VtipBazeContext dbContext;

        public override string PageTitle => "Categories";

        public List<TagListModel> Tags { get; set; }

        public TagListViewModel(VtipBazeContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public override Task PreRender()
        {
            Tags = SelectTags().ToList();
            return base.PreRender();
        }

        public IQueryable<TagListModel> SelectTags()
        {
            return dbContext.Tags
                .OrderBy(x => x.TagName)
                .Select(x => new TagListModel()
                {
                    TagName = x.TagName,
                    JokesCount = x.Jokes.Count
                });
        }
    }
}

