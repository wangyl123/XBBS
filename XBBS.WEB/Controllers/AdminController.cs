using System;
using System.Collections.Generic;
using System.Web.Mvc;
using XBBS.Models;

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

        #region 用户
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

        public ActionResult UserEdit(int uid)
        {
            var model = DataProvider.AdminDataProvider.GetUserInfo(uid);
            var list = DataProvider.AdminDataProvider.GetUserGroupList();
            ViewData["list"] = list;
            return View("usersedit", model);
        }

        [HttpPost]
        public ActionResult UserEdit(User model, string PasswordConfirmation)
        {
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (model.Password != PasswordConfirmation)
                {
                    TempData["result"] = "两次密码不一样，更新失败";
                    return RedirectToAction("UserEdit", new { uid = model.Uid });
                }
                model.Password = XBBS.Core.Utils.MD5(model.Password);
            }

            var result = DataProvider.AdminDataProvider.UpdateUserInfo(model);
            if (result)
            {
                TempData["result"] = "更新成功";
            }
            else
            {
                TempData["result"] = "更新失败";
            }
            return RedirectToAction("UserEdit", new { uid = model.Uid });
        }


        public ActionResult UserGroupEdit(string groupName, int? gid)
        {
            if (gid != null)
            {
                ViewBag.Gid = gid;
            }
            ViewBag.GroupName = groupName;
            return View("usergroupedit");
        }

        [HttpPost]
        public ActionResult UserGroupEdit(FormCollection collection)
        {
            var groupName = collection["groupName"];
            var gid = collection["gid"];
            bool result = false;
            if (string.IsNullOrWhiteSpace(gid))
            {
                result = DataProvider.AdminDataProvider.AddUserGroup(groupName);
            }
            else
            {
                result=DataProvider.AdminDataProvider.UpdateUserGroup(groupName,gid);
            }
            if (result)
            {
                TempData["result"] = "操作成功";
            }
            else
            {
                TempData["result"] = "操作失败";
            }
            return RedirectToAction("UserGroup");
        }


        public ActionResult DeleteGroup(int gid)
        {
            if (DataProvider.AdminDataProvider.DeleteUserGroup(gid))
            {
                TempData["result"] = "删除成功";
            }
            else
            {
                TempData["result"] = "删除失败";
            }
            return RedirectToAction("UserGroup");
        }

        public ActionResult DeleteUser(int uid)
        {
            if(DataProvider.AdminDataProvider.DeleteUser(uid))
            {
                TempData["result"] = "删除成功";
            }
            else
            {
                TempData["result"] = "删除失败";
            }
            return RedirectToAction("Users");
        }

        #endregion

        #region 节点

        public ActionResult Nodes()
        {
            var list = DataProvider.AdminDataProvider.GetAllCategoryList();
            var tree = GetCategoryTree(list, 0);
            ViewData["tree"] = tree;
            return View("node");
        }

        private List<Category> GetCategoryTree(List<Category> list, int pid)
        {
            List<Category> tree = new List<Category>();
            System.Collections.Stack task = new System.Collections.Stack();
            task.Push(pid);
            int level = 0;
            while (task.Count > 0)
            {
                bool flag = false;
                for (int i = 0; i < list.Count; i++)
                {
                    var l = list[i];
                    if (l.PId == pid)
                    {
                        task.Push(l.CId);
                        pid = l.CId;
                        l.CLevel = level.ToString();
                        tree.Add(l);
                        list.Remove(l);
                        i--;
                        level++;
                        flag = true;
                    }
                }
                if (!flag)
                {
                    task.Pop();
                    level--;
                    if (task.Count > 0)
                    {
                        pid = Convert.ToInt32(task.Peek());
                    }
                }
            }
            return tree;
        }

        public ActionResult AddNodes()
        {
            var categoryList = DataProvider.AdminDataProvider.GetFirstLevelCategoryList();
            ViewData["categoryList"] = categoryList;
            var usergroupList = DataProvider.AdminDataProvider.GetUserGroupList();
            ViewData["usergroupList"] = usergroupList;
            return View("nodeadd");
        }

        [HttpPost]
        public ActionResult AddNodes(Category model)
        {
            model.Permit = Request["premit"];
            if (DataProvider.AdminDataProvider.AddCategory(model))
            {
                TempData["result"] = "添加成功";
            }
            else
            {
                TempData["result"] = "添加失败";
            }
            return RedirectToAction("Nodes");
        }

        #endregion

    }
}
