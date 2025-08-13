using System.Web;
using System.Web.Optimization;

namespace ICMR_Project
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {


            // IcfaiTech  Start
            bundles.Add(new StyleBundle("~/assets/css").Include(
          "~/assets/vendor/bootstrap/css/bootstrap.min.css",
          "~/assets/vendor/aos/aos.css",
              "~/assets/vendor/owlcarousel/css/owl.carousel.min.css",
         "~/assets/vendor/bootstrap-icons/bootstrap-icons.css",
            "~/assets/css/style.css"
         ));
            //JS files

            bundles.Add(new ScriptBundle("~/assets/js").Include(
                     "~/assets/vendor/jquery/js/jquery.min.js",
                     "~/assets/vendor/purecounter/purecounter_vanilla.js",
                        "~/assets/vendor/owlcarousel/js/owl.carousel.js",
                     "~/assets/js/jscript.js",
                      "~/assets/js/valid-email.js",
                     "~/assets/js/main.js"
                    ));

            BundleTable.EnableOptimizations = false;
        }
    }
}
