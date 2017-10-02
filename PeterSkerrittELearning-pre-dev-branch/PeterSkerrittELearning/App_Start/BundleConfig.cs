using System.Web;
using System.Web.Optimization;

namespace PeterSkerrittELearning
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/sidebar").Include(
                 "~/Scripts/sidebar.js"));

            bundles.Add(new ScriptBundle("~/bundles/core").Include(
                 "~/assets/js/plugins/loaders/pace.min.js",
                 "~/assets/js/plugins/loaders/blockui.min.js",
                 "~/assets/js/plugins/forms/selects/bootstrap_multiselect.js",
                 "~/assets/js/core/app.js",
                 "~/assets/js/pages/form_multiselect.js",
                 "~/assets/js/pages/login.js"));

            bundles.Add(new ScriptBundle("~/bundles/particles").Include(
                 "~/assets/js/particles.js"));


            bundles.Add(new ScriptBundle("~/bundles/wizzard_steppy").Include(
               "~/assets/js/pages/wizard_stepy.js"));


            bundles.Add(new ScriptBundle("~/bundles/advance_table").Include(
            "~/assets/js/plugins/tables/datatables/datatables.min.js",
            "~/assets/js/plugins/forms/selects/select2.min.js",
            "~/assets/js/pages/datatables_advanced.js"
            
            ));

            bundles.Add(new ScriptBundle("~/bundles/advanced_table").Include(
            "~/assets/js/pages/datatables_advanced.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
            "~/assets/dropzone/dropzone.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                 "~/assets/js/plugins/forms/wizards/stepy.min.js",
                 "~/assets/js/plugins/forms/selects/select2.min.js",
                 "~/assets/js/plugins/forms/styling/uniform.min.js",
                 "~/assets/js/core/libraries/jasny_bootstrap.min.js",
                 "~/assets/js/plugins/forms/validation/validate.min.js"
        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/sidebar.css"));

            bundles.Add(new StyleBundle("~/Content/icomoon").Include(
                   "~/assets/css/icons/icomoon/styles.css"));

            

            bundles.Add(new StyleBundle("~/Content/core").Include(
                      "~/assets/css/bootstrap.css",
                      "~/assets/css/core.css",
                      "~/assets/css/components.css",
                      "~/assets/css/colors.css",
                      "~/assets/css/extras/animate.min.css"));

            bundles.Add(new StyleBundle("~/Content/particles").Include(
                      "~/assets/css/particles.css"));

            bundles.Add(new StyleBundle("~/Content/dropzone").Include(
                   //"~/assets/dropzone/basic.min.css",
                   "~/assets/dropzone/dropzone.min.css"));
        }
    }
}
