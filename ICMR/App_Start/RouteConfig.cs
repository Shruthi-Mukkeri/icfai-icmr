using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ICMR
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Admin Pages

                // routes.MapRoute(
                //name: "adminlogin",
                //url: "admin/adminlogin",
                //defaults: new { controller = "Admin", action = "AdminLogin" }
                //  );

            routes.MapRoute(
               name: "AdminLogin",
               url: "Admin/AdminLogin",
               defaults: new { controller = "Admin", action = "AdminLogin", id = UrlParameter.Optional }
           );
         
            routes.MapRoute(
              name: "AddEvents",
              url: "Admin/AddEvents",
              defaults: new { controller = "Admin", action = "AddEvents", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "Events",
              url: "Admin/Events",
              defaults: new { controller = "Admin", action = "Events", id = UrlParameter.Optional }
          );

            routes.MapRoute(
            name: "AddProfiles",
            url: "Admin/AddProfiles",
            defaults: new { controller = "Admin", action = "AddProfiles", id = UrlParameter.Optional }
        );

            routes.MapRoute(
              name: "Profiles",
              url: "Admin/Profiles",
              defaults: new { controller = "Admin", action = "Profiles", id = UrlParameter.Optional }
          );

       //     routes.MapRoute(
       //    name: "EditProfile",
       //    url: "User/EditProfile",
       //    defaults: new { controller = "User", action = "EditProfile", id = UrlParameter.Optional }
       //);

            routes.MapRoute(
                name: "CategoryRoute",
                url: "case-studies/{category}",
                defaults: new { controller = "Home", action = "CaseStudies", category = UrlParameter.Optional }
              );


            routes.MapRoute(
             name: "InterviewDetail",
             url: "interview-detail",
             defaults: new { controller = "Home", action = "InterviewDetail" }
           );

            routes.MapRoute(
            name: "EditProfile",
            url: "EditProfile",
            defaults: new { controller = "Home", action = "EditProfile" }
          );

            routes.MapRoute(
              name: "Interview",
              url: "case-resources/interview",
              defaults: new { controller = "Home", action = "Interview" }
            );

            routes.MapRoute(
              name: "checkout",
              url: "checkout",
              defaults: new { controller = "Home", action = "CheckOut" }
            );

            routes.MapRoute(
              name: "Cart",
              url: "add-to-cart/cart-details",
              defaults: new { controller = "Home", action = "Cart" }
            );

            routes.MapRoute(
              name: "achievements",
              url: "achievements",
              defaults: new { controller = "Home", action = "Achievements" }
            );

            routes.MapRoute(
              name: "registration",
              url: "registration",
              defaults: new { controller = "Home", action = "Registration" }
            );

            routes.MapRoute(
              name: "login",
              url: "login",
              defaults: new { controller = "Home", action = "Login" }
            );

            routes.MapRoute(
              name: "just-in-case",
              url: "just-in-case",
              defaults: new { controller = "Home", action = "JustInCase" }
            );

            routes.MapRoute(
              name: "icmr-services",
              url: "icmr-services",
              defaults: new { controller = "Home", action = "ICMRServices" }
            );

            routes.MapRoute(
              name: "MediaDetailPage",
              url: "mediadetailpage",
              defaults: new { controller = "Home", action = "MediaDetailPage" }
            );
            routes.MapRoute(
              name: "media",
              url: "media",
              defaults: new { controller = "Home", action = "Media" }
            );
            routes.MapRoute(
             name: "event-details-page",
             url: "event-details-page",
             defaults: new { controller = "Home", action = "EventDetailPage" }
           );
            routes.MapRoute(
            name: "search",
            url: "search",
            defaults: new { controller = "Home", action = "Search" }
          );
            routes.MapRoute(
              name: "EventPage",
              url: "event",
              defaults: new { controller = "Home", action = "EventPage" }
            );

            routes.MapRoute(
              name: "AlumniSpeak",
              url: "case-resources/video/alumni-speak",
              defaults: new { controller = "Home", action = "AlumniSpeak" }
            );
            routes.MapRoute(
              name: "Subscribe",
              url: "e-newsletter/subscribe",
              defaults: new { controller = "Home", action = "Subscribe" }
            );

            routes.MapRoute(
              name: "mdp-fdp",
              url: "services/mdp-fdp",
              defaults: new { controller = "Home", action = "MdpFdp" }
             );

            routes.MapRoute(
              name: "case-mapping-to-curriculum",
              url: "services/case-mapping-to-curriculum",
              defaults: new { controller = "Home", action = "CaseMappingToCurriculum" }
             );

            routes.MapRoute(
              name: "case-subscription",
              url: "services/licensing/case-subscription",
              defaults: new { controller = "Home", action = "CaseSubscription" }
             );
            routes.MapRoute(
              name: "telecom",
              url: "other-products/case-studies-pack/telecom",
              defaults: new { controller = "Home", action = "Telecom" }
            );

            routes.MapRoute(
              name: "retailing",
              url: "other-products/case-studies-pack/retailing",
              defaults: new { controller = "Home", action = "Retailing" }
            );

            routes.MapRoute(
              name: "media-entertainment",
              url: "other-products/case-studies-pack/media-entertainment",
              defaults: new { controller = "Home", action = "MediaEntertainment" }
            );

            routes.MapRoute(
              name: "electricals-and-electronics",
              url: "other-products/case-studies-pack/electricals-and-electronics",
              defaults: new { controller = "Home", action = "ElectricalsElectronics" }
            );



            routes.MapRoute(
              name: "banking-and-financial-services",
              url: "other-products/case-studies-pack/banking-and-financial-services",
              defaults: new { controller = "Home", action = "BankingFinancialServices" }
            );

            routes.MapRoute(
              name: "aviation",
              url: "other-products/case-studies-pack/aviation",
              defaults: new { controller = "Home", action = "Aviation" }
            );

            routes.MapRoute(
              name: "icmr-case-studies",
              url: "icmr-case-studies",
              defaults: new { controller = "Home", action = "IcmrCaseStudies" }
          );

            routes.MapRoute(
              name: "awards-and-recognitions",
              url: "awards-and-recognitions",
              defaults: new { controller = "Home", action = "AwardsRecognitions" }
          );

            routes.MapRoute(
              name: "publications",
              url: "publications",
              defaults: new { controller = "Home", action = "Publications" }
          );
            routes.MapRoute(
              name: "profile-sanjib-dutta",
              url: "profile-sanjib-dutta",
              defaults: new { controller = "Home", action = "ProfileSanjibDutta" }
          );

            routes.MapRoute(
              name: "about-ibs-case-research-center",
              url: "about-ibs-case-research-center",
              defaults: new { controller = "Home", action = "AboutIbsCaseResearchCenter" }
          );

            routes.MapRoute(
              name: "case-study-details",
              url: "case-study-details",
              defaults: new { controller = "Home", action = "CaseStudyDetails" }
            );

            routes.MapRoute(
               name: "business-environment",
               url: "business-environment",
               defaults: new { controller = "Home", action = "BusinessEnvironment" }
           );

            routes.MapRoute(
               name: "contact-us",
               url: "contact-us",
               defaults: new { controller = "Home", action = "ContactUs" }
           );

            routes.MapRoute(
               name: "payment-methods",
               url: "payment-methods",
               defaults: new { controller = "Home", action = "PaymentMethods" }
           );

            routes.MapRoute(
               name: "careers",
               url: "careers",
               defaults: new { controller = "Home", action = "Careers" }
           );

            routes.MapRoute(
               name: "faqs",
               url: "faqs",
               defaults: new { controller = "Home", action = "Faqs" }
           );

            routes.MapRoute(
               name: "terms-of-use",
               url: "terms-of-use",
               defaults: new { controller = "Home", action = "TermsOfUse" }
           );

            routes.MapRoute(
               name: "disclosure",
               url: "disclosure",
               defaults: new { controller = "Home", action = "Disclosure" }
           );

            routes.MapRoute(
               name: "privacy-policy",
               url: "privacy-policy",
               defaults: new { controller = "Home", action = "PrivacyPolicy" }
           );

            routes.MapRoute(
               name: "sitemap",
               url: "sitemap",
               defaults: new { controller = "Home", action = "Sitemap" }
           );

            routes.MapRoute(
               name: "customized-case-studies",
               url: "customized-case-studies",
               defaults: new { controller = "Home", action = "CustomizedCaseStudies" }
           );

            routes.MapRoute(
               name: "case-writing-workshops",
               url: "case-writing-workshops",
               defaults: new { controller = "Home", action = "CaseWritingWorkshops" }
           );

            routes.MapRoute(
                name: "reprint-permission",
                url: "reprint-permission",
                defaults: new { controller = "Home", action = "ReprintPermission" }
            );

            routes.MapRoute(
                name: "icmr-case-pricing",
                url: "icmr-case-pricing",
                defaults: new { controller = "Home", action = "IcmrCasePricing" }
            );



            routes.MapRoute(
                name: "Default1",
                url: "index1",
                defaults: new { controller = "Home", action = "Index1" }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
