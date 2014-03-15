using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XBBS.Core
{
    public class ThemeViewEngine : BuildManagerViewEngine
    {

        private static Dictionary<string, string> dicViewPath = new Dictionary<string, string>();
        public static string ThemeName
        {
            get
            {
                object re = HttpContext.Current.Items["ThemeName"];
                if (re == null)
                {
                    re = "default";
                }
                HttpContext.Current.Items["ThemeName"] = re;
                return re.ToString();
            }
            set { HttpContext.Current.Items["ThemeName"] = value; }
        }

        public ThemeViewEngine()
            : this(null)
        {
        }

        public ThemeViewEngine(IViewPageActivator viewPageActivator)
            : base(viewPageActivator)
        {
            //throw new Exception(SkinName);
            base.AreaViewLocationFormats = new string[] { "~/Areas/{2}/View/{1}/{0}.cshtml", "~/Areas/{2}/View/{1}/{0}.vbhtml", "~/Areas/{2}/View/Shared/{0}.cshtml", "~/Areas/{2}/View/Shared/{0}.vbhtml" };
            base.AreaMasterLocationFormats = new string[] { "~/Areas/{2}/View/{1}/{0}.cshtml", "~/Areas/{2}/View/{1}/{0}.vbhtml", "~/Areas/{2}/View/Shared/{0}.cshtml", "~/Areas/{2}/View/Shared/{0}.vbhtml" };
            base.AreaPartialViewLocationFormats = new string[] { "~/Areas/{2}/View/{1}/{0}.cshtml", "~/Areas/{2}/View/{1}/{0}.vbhtml", "~/Areas/{2}/View/Shared/{0}.cshtml", "~/Areas/{2}/View/Shared/{0}.vbhtml" };
            base.ViewLocationFormats = new string[] { "~/theme/" + ThemeName + "/{1}/{0}.cshtml", "~/theme/" + ThemeName + "/{1}/{0}.vbhtml", "~/theme/" + ThemeName + "/Shared/{0}.cshtml", "~/theme/" + ThemeName + "/Shared/{0}.vbhtml" };
            base.MasterLocationFormats = new string[] { "~/theme/" + ThemeName + "/{1}/{0}.cshtml", "~/theme/" + ThemeName + "/{1}/{0}.vbhtml", "~/theme/" + ThemeName + "/Shared/{0}.cshtml", "~/theme/" + ThemeName + "/Shared/{0}.vbhtml" };
            base.PartialViewLocationFormats = new string[] { "~/theme/" + ThemeName + "/{1}/{0}.cshtml", "~/theme/" + ThemeName + "/{1}/{0}.vbhtml", "~/theme/" + ThemeName + "/Shared/{0}.cshtml", "~/theme/" + ThemeName + "/Shared/{0}.vbhtml" };
            base.FileExtensions = new string[] { "cshtml", "vbhtml" };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            string layoutPath = null;
            bool runViewtartPages = false;

            IEnumerable<string> fileExtensions = base.FileExtensions;
            return new RazorView(controllerContext, partialPath, layoutPath, runViewtartPages, fileExtensions, base.ViewPageActivator);
        }


        private string PascalName(string name)
        {
            name = name.ToLower();
            char[] chs = name.ToArray();
            chs[0] = Char.ToUpper(chs[0]);
            return new string(chs);

        }
        //private string RedressViewPath(string viewPath)
        //{
        //    if (dicViewPath.ContainsKey(viewPath))
        //        return dicViewPath[viewPath];
        //    System.Text.RegularExpressions.Regex regx = new System.Text.RegularExpressions.Regex(@"~/theme/" + ThemeName + @"/(\w+)/(\w+)\.cshtml");
        //    var match = regx.Match(viewPath);
        //    if (match.Success)
        //    {
        //        string truestPath = "~/theme/" + ThemeName + "/" + PascalName(match.Groups[1].Value) + "/" + match.Groups[2].Value + ".cshtml";
        //        dicViewPath.Add(viewPath, truestPath);
        //        viewPath = truestPath;
        //    }
        //    else
        //    {
        //        dicViewPath.Add(viewPath, viewPath);
        //    }
        //    return viewPath;


        //}

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            string layoutPath = masterPath;
            bool runViewtartPages = true;
            IEnumerable<string> fileExtensions = base.FileExtensions;
            //viewPath = RedressViewPath(viewPath);
            return new RazorView(controllerContext, viewPath, layoutPath, runViewtartPages, fileExtensions, base.ViewPageActivator);
        }

        //public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        //{
        //    partialViewName = partialViewName.ToLower();
        //    return base.FindPartialView(controllerContext, partialViewName, useCache);
        //}

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            //viewName = viewName.ToLower();
            //return base.FindView(controllerContext, viewName, masterName, useCache);


            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewName))
            {
             throw new ArgumentException( "viewNameu不能为空");
            }
            string requiredString = controllerContext.RouteData.GetRequiredString("controller");
            // string[] first;
            // string path = this.GetPath(controllerContext, this.ViewLocationFormats, this.AreaViewLocationFormats, "ViewLocationFormats", viewName, requiredString, "View", useCache, out first);
            // string[] second;
            // string path2 = this.GetPath(controllerContext, this.MasterLocationFormats, this.AreaMasterLocationFormats, "MasterLocationFormats", masterName, requiredString, "Master", useCache, out second);
            //if (string.IsNullOrEmpty(path) || (string.IsNullOrEmpty(path2) && !string.IsNullOrEmpty(masterName)))
            //{
            //    return new ViewEngineResult(first.Union(second));
            //}
            string path = "~/theme/" + ThemeName + "/" + PascalName(requiredString) + "/" + viewName.ToLower() + ".cshtml";

            if (!System.IO.File.Exists(controllerContext.HttpContext.Server.MapPath(path)))
            {
                throw new System.IO.FileNotFoundException(path);
            }

            return new ViewEngineResult(this.CreateView(controllerContext, path, ""), this);

        }
    }
}