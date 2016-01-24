using System.Web;
using System.Web.Optimization;

namespace SaleManager
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/shopstyle").Include(
               //"~/Content/css/bootstrap.css",
               "~/Content/css/style.css",
               "~/Content/fonts/font-awesome/css/font-awesome.min.css",
               "~/Content/css/animate.css"
               ));
            bundles.Add(new ScriptBundle("~/bundles/shopjs").Include(
               "~/Scripts/jquery-{version}.js",
               "~/Content/js/bootstrap/bootstrap.min.js",
               "~/Content/js/scrolltopcontrol.js",
               "~/Content/js/custom.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/admmainjs").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/plugins/metisMenu/jquery.metisMenu.js",
                "~/Scripts/plugins/slimscroll/jquery.slimscroll.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/admjqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.validate.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/admcustomjs").Include(
                "~/Scripts/cheapdeal.js",
                                           "~/Scripts/plugins/pace/pace.min.js"));

            bundles.Add(new StyleBundle("~/Content/admbootstrap").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/fonts/font-awesome/css/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/Content/admstyle").Include(
                      "~/Content/animate.css",
                      "~/Content/AdmSite.css"));

            bundles.Add(new ScriptBundle("~/bundles/dropzonescripts").Include(
                     "~/Scripts/dropzone/dropzone.js"));
            bundles.Add(new StyleBundle("~/Content/dropzonescss").Include(
                     "~/Scripts/dropzone/css/basic.css",
                     "~/Scripts/dropzone/css/dropzone.css"));


            //Index
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.validate.bootstrap.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/inspinia/cbpAnimatedHeader.js",
                      "~/Scripts/inspinia/classie.js",
                      "~/Scripts/inspinia/pace.min.js",
                      //"~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/inspinia/wow.min.js",
                      "~/Scripts/inspinia/inspinia.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/plugins/datepicker/datepicker3.css",
                      "~/Content/style.css"));
        }
    }
}
