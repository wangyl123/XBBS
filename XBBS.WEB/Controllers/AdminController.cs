using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XBBS.WEB.Controllers
{
    public class AdminController : RootController
    {
        //
        // GET: /Admin/

        public ActionResult Dashboard()
        {
            return View();
        }

        #region 基本设置
        public ActionResult Settings_Web()
        {
            var dic = DataProvider.AdminDataProvider.GetSettingsDict();
            ViewData["dic"] = dic;
            return View("settings_web");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Settings_Web(FormCollection collection)
        {
            UpdateSettings(collection);
            var dic = DataProvider.AdminDataProvider.GetSettingsDict();
            ViewData["dic"] = dic;
            return View("settings_web");
        }

        private void UpdateSettings(FormCollection collection)
        {
            Dictionary<string, string> formDic = new Dictionary<string, string>();
            for (int i = 0; i < collection.Count; i++)
            {
                formDic.Add(collection.Keys[i].ToString(), collection[i]);
            }
            bool uResult = DataProvider.AdminDataProvider.UpdateSettings(formDic);
            if (uResult)
            {
                ViewBag.res = "修改成功";
            }
            else
            {
                ViewBag.res = "修改失败";
            }
        }

        public ActionResult Settings_Topic()
        {
            var dic = DataProvider.AdminDataProvider.GetSettingsDict();
            ViewData["dic"] = dic;
            return View("settings_topic");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Settings_Topic(FormCollection collection)
        {
            UpdateSettings(collection);
            var dic = DataProvider.AdminDataProvider.GetSettingsDict();
            ViewData["dic"] = dic;
            return View("settings_topic");
        }

        public ActionResult Settings_LoginApi()
        {
            var dic = DataProvider.AdminDataProvider.GetSettingsDict();
            ViewData["dic"] = dic;
            return View("settings_loginapi");
        }

        [HttpPost]
        public ActionResult Settings_LoginApi(FormCollection collection)
        {
            UpdateSettings(collection);
            var dic = DataProvider.AdminDataProvider.GetSettingsDict();
            ViewData["dic"] = dic;
            return View("settings_loginapi");
        }

        public ActionResult Settings_Mail()
        {
            var dic = DataProvider.AdminDataProvider.GetSettingsDict();
            ViewData["dic"] = dic;
            return View("settings_mail");
        }

        [HttpPost]
        public ActionResult Settings_Mail(FormCollection collection)
        {
            UpdateSettings(collection);
            var dic = DataProvider.AdminDataProvider.GetSettingsDict();
            ViewData["dic"] = dic;
            return View("settings_mail");
        }
        #endregion

        public ActionResult Users(int page = 1)
        {
            var list = DataProvider.AdminDataProvider.GetUserList(page);
            ViewData["list"] = list;
            ViewBag.pageNow = page;
            ViewBag.url = "/Admin/Users";
            ViewBag.rowTotalNun = DataProvider.AdminDataProvider.GetUserTotalCount();
            return View("users");
        }


        public ActionResult UserGroup()
        {
            var list = DataProvider.AdminDataProvider.GetUserGroupsList();
            ViewData["list"] = list;
            return View();
        }

        public ActionResult PageNavigate(int rowTotalNun, string url, int pageNow = 1, int pageCount = 10)
        {
            List<string> navigateList = new List<string>(); ;
            int pageTotalNum = rowTotalNun % pageCount == 0 ? (rowTotalNun / pageCount) : rowTotalNun / pageCount + 1;
            if (pageNow < 1 || pageNow > pageTotalNum)
            {
                ViewData["pageNavigate"] = navigateList;
                return View("pagenavigate");
            }
            if (pageNow > 1)
            {
                navigateList.Add("<li class='prev'><a href='" + url + "/" + (pageNow - 1) + "'><-</a></li>");
            }

            for (int i = 1; i <= pageTotalNum; i++)
            {
                if (pageNow == i)
                {
                    navigateList.Add("<li class='prev'><a href='" + url + "/" + i + "'>" + i + "</a></li>");
                }
                else if (pageNow < i)
                {
                    navigateList.Add("<li class='next'><a href='" + url + "/" + i + "'>" + i + "</a></li>");
                }
                else
                {
                    navigateList.Add("<li class='prev'><a href='" + url + "/" + i + "'>" + i + "</a></li>");
                }
            }
            if (pageTotalNum > pageNow)
            {
                navigateList.Add("<li class='prev'><a href='" + url + "/" + (pageNow + 1) + "'>-></a></li>");
            }
            ViewData["pageNavigate"] = navigateList;
            ViewBag.html = "<li class='prev'><a href='/Admin/Users/1'>1</a></li><li class='prev'><a href='/Admin/Users/2'>-></a></li> ";
            return View("pagenavigate");
        }
        
    }
}
