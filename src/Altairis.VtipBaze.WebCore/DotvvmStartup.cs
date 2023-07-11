using Altairis.VtipBaze.Data;
using DotVVM.Adapters.WebForms;
using DotVVM.Framework.Configuration;
using DotVVM.Framework.ResourceManagement;
using DotVVM.Framework.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Altairis.VtipBaze.WebCore
{
    public class DotvvmStartup : IDotvvmStartup, IDotvvmServiceConfigurator
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            ConfigureRoutes(config, applicationPath);
            ConfigureControls(config, applicationPath);
            ConfigureResources(config, applicationPath);

            config.AddWebFormsAdapters();
        }

        private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
        {
            // register routes   
            config.RouteTable.Add("TagList", "tags", "Views/TagList.dothtml");
            config.RouteTable.AutoDiscoverRoutes(new DefaultRouteStrategy(config));    
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            // register code-only controls and markup controls
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            // register custom resources and adjust paths to the built-in resources
            config.Resources.RegisterStylesheetFile("main-css", "Styles/main.css");
            config.Resources.RegisterScriptFile("jquery", "Scripts/jquery-2.0.3.min.js");
            config.Resources.RegisterStylesheetUrl("jquery-ui-css", "//ajax.aspnetcdn.com/ajax/jquery.ui/1.10.3/themes/south-street/jquery-ui.css", null);
            config.Resources.RegisterScriptFile("jquery-ui", "Scripts/jquery-ui-1.10.3.min.js", dependencies: new [] { "jquery", "jquery-ui-css" });
            config.Resources.RegisterScriptFile("site", "Scripts/Site/ui.js", dependencies: new[] { "jquery-ui", "main-css" });
        }
		
		public void ConfigureServices(IDotvvmServiceCollection options)
        {
            options.AddDefaultTempStorages("temp");

            options.Services.AddScoped<VtipBazeContext>();
        }
    }
}
