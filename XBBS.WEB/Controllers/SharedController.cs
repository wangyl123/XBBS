using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XBBS.WEB.Controllers
{
    public class SharedController :  RootController
    {
        //
        // GET: /Shared/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RightLogin()
        {
            var user = Session["User"] as Models.User;
            if (user == null)
            {
                return View("_right-login");
            }
            if (Core.UploadTools.CheckAvatarExists(user.Uid.ToString(), Server.MapPath("/")))
            {
                var model = Core.UploadTools.GetAvatarPath(user.Uid.ToString());
                ViewBag.Avatar = model.Large;
            }
            else
            {
                ViewBag.Avatar = "~/uploads/avatar/avatar_large.jpg";
            }
            return View("_right-login");
        }

    }
}
