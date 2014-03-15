using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XBBS.WEB.Controllers
{
    public class SettingsController : RootController
    {
        //
        // GET: /Settings/

        [Authorize]
        public ActionResult Avatar()
        {
            InitAvatar();
            return View();
        }

        private void InitAvatar()
        {
            Models.User user = Session["User"] as Models.User;
            if (Core.UploadTools.CheckAvatarExists(user.Uid.ToString(), Server.MapPath("/")))
            {
                var model = Core.UploadTools.GetAvatarPath(user.Uid.ToString());
                ViewBag.User.Avatar = "yes";
                ViewBag.L = model.Large;
                ViewBag.M = model.Medium;
                ViewBag.S = model.Small;
            }
        }

        [HttpPost, Authorize, ValidateInput(false)]
        public ActionResult Avatar(FormCollection collection)
        {
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                return View();
            }
            Models.User user = Session["User"] as Models.User;
            var avatarSavePath = Core.UploadTools.GetAvatorSavePath(user.Uid.ToString(), System.IO.Path.GetExtension(file.FileName), Server.MapPath("/"));
            file.SaveAs(avatarSavePath.Source);
            Core.UploadTools.MakeAvatarThumbnal(avatarSavePath);
            InitAvatar();
            return View();
        }

        [Authorize]
        public ActionResult Profile()
        {

            return View(System.Web.HttpContext.Current.Session["User"] as Models.User);
        }

        [HttpPost, Authorize]
        public ActionResult Profile(FormCollection collenction)
        {
            var user = Session["User"] as Models.User;

            //user.UserName = collenction["username"];
            user.Email = collenction["email"];
            user.Homepage = collenction["homepage"];
            user.Location = collenction["location"];
            user.QQ = collenction["qq"];
            user.Signature = collenction["signature"];
            user.Introduction = collenction["introduction"];
            DataProvider.SettingsDataProvider.UpdateUserInfo(user);
            Session["User"] = user;
            return View(user);
        }

        [Authorize]
        public ActionResult Password()
        {
            return View();
        }

        [Authorize, HttpPost]
        public ActionResult Password(FormCollection collection)
        {
            var pwd = collection["password"];
            var npwd = collection["newpassword"];
            var npwd2 = collection["newpassword2"];
            bool opera = false;
       
            if (npwd != npwd2)
            {
                ViewBag.oResult = "操作失败";
                return View();
            }
            var user = Session["User"] as Models.User;
            if (user.Password != Core.Utils.MD5(pwd))
            {
                ViewBag.oResult = "操作失败";
                return View();
            }
            user.Password = Core.Utils.MD5(npwd);
            DataProvider.SettingsDataProvider.UpdateUserInfo(user);
            Session["User"] = user;
            ViewBag.oResult = "操作成功";
            return View();
        }
    }
}
