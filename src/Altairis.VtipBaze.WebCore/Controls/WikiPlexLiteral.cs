using System.Web.UI;
using WikiPlex;

namespace Altairis.VtipBaze.WebCore.Controls {
    public class WikiPlexLiteral : Control, ITextControl {

        public string Text {
            get { return this.ViewState["Text"] as string; }
            set { this.ViewState["Text"] = value; }
        }

        protected override void Render(HtmlTextWriter writer) {
            var engine = new WikiEngine();
            writer.Write(engine.Render(this.Text));
        }

    }
}