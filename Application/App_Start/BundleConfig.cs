using System.Web;
using System.Web.Optimization;

namespace Pharma
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.12.4.js",
                        "~/Scripts/jquery-ui-1.12.1.min.js",
                        "~/Scripts/moment.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      //"~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/select2.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/JQueryUI/jquery-ui.min.css",
                      "~/Content/bootstrap.css",
                      //"~/Content/bootstrap-datepicker.css",
                      "~/Content/site.css",
                      "~/Content/select2.min.css"
                      ));
        }
    }
}
