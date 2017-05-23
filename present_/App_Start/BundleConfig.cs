using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Optimization;

namespace Presentation_.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = false;

            //all in one
            /*
            bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Scripts/*.js"));
            bundles.Add(new StyleBundle("~/bundles/css").Include("~/Content/*.css"));
            */

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"
                , "~/Scripts/bootstrap-select.js"));
            bundles.Add(new StyleBundle("~/content/bootstrap").Include("~/Content/bootstrap.css"
                ,"~/Content/bootstrap-theme.css"
                , "~/Content/bootstrap-select.css"));

            bundles.Add(new StyleBundle("~/content/style").Include("~/Content/Site.css"                
                , "~/Content/jquery-ui.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/JavaScript").Include("~/Scripts/JavaScript.js", "~/Scripts/d3.js"));

            /*

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.css", "~/Content/bootstrap-theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include("~/Scripts/moment.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetime").Include("~/Scripts/bootstrap-datetimepicker*"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap-datetimepicker.min.css", "~/Content/Site.css"));

            bundles.Add(new ScriptBundle("~/bundles/JavaScript").Include("~/Scripts/JavaScript.js"));
            */


            //bundles.IgnoreList.Clear();
        }
    }
}