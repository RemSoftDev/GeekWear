using System.Web;
using System.Web.Optimization;

namespace GeekWear
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/utils").Include(
                    "~/Scripts/utils.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-animate.js",
                        "~/Scripts/angular-sanitize.js",
                        "~/Scripts/angular-touch.js",
                        "~/Scripts/angular-messages.js",
                        "~/Scripts/angular-message-format.js",
                        "~/Scripts/angular-cookies.js",
                        "~/Scripts/angular-route.js"));
            bundles.Add(new ScriptBundle("~/bundles/textEncoders").Include(
                        "~/Scripts/qrcode.js",
                        "~/Scripts/angular-qr.js",
                        "~/Scripts/morse.js",
                        "~/Scripts/md5.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular-bootstrap").Include(
                      "~/Scripts/angular-ui/ui-bootstrap-tpls.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/ui-bootstrap-csp.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/app/app.js")
                .IncludeDirectory("~/app/common/services/", "*.js", true)
                .IncludeDirectory("~/app/common/components/", "*.js", true)
                .IncludeDirectory("~/app/modules/home/", "*.js", true)
                .IncludeDirectory("~/app/modules/manage/", "*.js", true)
               );
        }
    }
}
