using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace BootstrapSupport
{
    public class BootstrapBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // core scripts
            bundles.Add(new ScriptBundle("~/Bundles/CoreJs").Include(
                "~/Scripts/jquery-1.9.1.min.js",
                "~/Scripts/jquery-ui-1.10.3/ui/jquery.ui.datepicker.js",
                "~/Scripts/jquery-migrate-1.1.1.minjs",
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-typeahead.js",
                "~/Scripts/jquery.validate.js",
                "~/scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.unobtrusive-custom-for-bootstrap.js",
                "~/Scripts/GroupScheduler/Base/GroupScheduler.js"
                
                ));

            // fullcalendar JS bundle
            bundles.Add(new ScriptBundle("~/Bundles/FulcalJs").Include(
                    "~/Scripts/fullcalendar-1.6.2/jquery/jquery-ui-1.10.2.custom.min.js",
                    "~/Scripts/fullcalendar-1.6.2/fullcalendar/fullcalendar.min.js",
                    "~/Scripts/fullcalendar-1.6.2/fullcalendar/gcal.js"
                ));

            // Home page JS bundle
            bundles.Add(new ScriptBundle("~/Bundles/HomeJs").Include(
                "~/Scripts/GroupScheduler/Home/Home.js"
                ));

            // View group JS bundle
            bundles.Add(new ScriptBundle("~/Bundles/ViewGroupJs").Include(
                "~/Scripts/GroupScheduler/Group/ViewGroupCalendar.js",
                "~/Scripts/GroupScheduler/Group/ManageGroupMembers.js",
                "~/Scripts/GroupScheduler/Group/AddGroupEvent.js"
                ));

            // Core CSS bundle
            bundles.Add(new StyleBundle("~/Bundles/CoreCss").Include(
                "~/Content/bootstrap.css",
                "~/Content/body.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/bootstrap-mvc-validation.css",
                "~/Scripts/jquery-ui-1.10.3/themes/base/jquery.ui.datepicker.css",
                "~/Content/Site/Site.css"
                ));

            // FUll calendar CSS bundle
            bundles.Add(new StyleBundle("~/Bundles/FullcalCss").Include(
                "~/Scripts/fullcalendar-1.6.2/fullcalendar/fullcalendar.css",
                "~/Scripts/fullcalendar-1.6.2/fullcalendar/fullcalendar.print.css"
                ));

            // Home CSS bundle
            bundles.Add(new StyleBundle("~/Bundles/HomeCss").Include(
                "~/Content/Home/Home.css"
                ));

            // View group CSS bundle
            bundles.Add(new StyleBundle("~/Bundles/ViewGroupCss").Include(
                "~/Content/Group/ViewGroup.css",
                "~/Content/Group/ManageGroupMembers.css"
                ));

            

            BundleTable.EnableOptimizations = true;
        }
    }
}