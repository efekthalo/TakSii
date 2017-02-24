using System.Web.Optimization;

namespace SiiTaxi
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-3.1.1.min.js",
                "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                "~/Scripts/moment-with-locales.js",
                "~/Scripts/bootstrap-datetimepicker.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/validator.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/bootstrap.min.css",
                 "~/Content/bootstrap-theme.min.css",
                 "~/Content/bootstrap-datetimepicker.min.css",
                 "~/Content/jquery-ui.css",
                 "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/table").Include(
                 "~/Content/bootstrap-table.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/table").Include(
                "~/Scripts/bootstrap-table.min.js",
                "~/Scripts/bootstrap-table-locale-all.min.js",
                "~/Scripts/bootstrap-table-filter.min.js",
                "~/Scripts/bootstrap-table-export.min.js"));
        }
    }
}
