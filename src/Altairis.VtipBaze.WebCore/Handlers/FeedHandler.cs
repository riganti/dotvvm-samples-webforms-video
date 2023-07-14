using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using Altairis.VtipBaze.Data;
using DotVVM.Framework.Hosting;

namespace Altairis.VtipBaze.WebCore.Handlers
{
    public class FeedHandler : IDotvvmPresenter 
    {

        public Task ProcessRequest(IDotvvmRequestContext context)
        {
            context.HttpContext.Response.ContentType = "application/atom+xml";
            context.HttpContext.Response.Headers["Cache-Control"] = "private";
            context.HttpContext.Response.Headers["Expires"] = DateTime.Now.AddMinutes(30).ToString("R");

            var baseUri = new Uri(context.GetApplicationBaseUri());
            var feed = new SyndicationFeed
            {
                Id = baseUri.ToString(),
                Title = new TextSyndicationContent("VTIPBÁZE.CZ"),
                Description = new TextSyndicationContent("Newest jokes"),
                Items = GetFeedItems(baseUri)
            };
            feed.Links.Add(new SyndicationLink(baseUri));
            using (var xw = new System.Xml.XmlTextWriter(context.HttpContext.Response.Body, Encoding.UTF8))
            {
                xw.Formatting = System.Xml.Formatting.Indented;
                var ff = new Atom10FeedFormatter(feed);
                ff.WriteTo(xw);
            }

            return Task.CompletedTask;
        }

        public IEnumerable<SyndicationItem> GetFeedItems(Uri baseUri)
        {
            using (var dc = new VtipBazeContext())
            {
                foreach (var j in dc.Jokes.Where(x => x.Approved).OrderByDescending(x => x.DateCreated).Take(10))
                {
                    var itemUri = new UriBuilder(baseUri);
                    itemUri.Path = "/joke/" + j.JokeId;

                    var si = new SyndicationItem
                    {
                        Title = new TextSyndicationContent(j.DateCreated.ToString()),
                        Content = new TextSyndicationContent(j.Text, TextSyndicationContentKind.Plaintext),
                        PublishDate = j.DateCreated,
                        LastUpdatedTime = j.DateCreated,
                        Id = itemUri.Uri.ToString(),
                    };
                    si.AddPermalink(itemUri.Uri);
                    foreach (var tag in j.Tags)
                    {
                        si.Categories.Add(new SyndicationCategory(tag.TagName));
                    }
                    yield return si;
                }
            }
        }
    }
}
