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
        #region 运行状态
        public ActionResult Dashboard()
        {
            ViewBag.UserCount = DataProvider.AdminDataProvider.GetUserCount();
            ViewBag.ForumsCount = DataProvider.AdminDataProvider.GetForumsCount();
            ViewBag.CommentsCount = DataProvider.AdminDataProvider.GetCommentsCount();

            return View();
        }

        #endregion

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

        public ActionResult UserSearch(string username)
        {
            var list = DataProvider.AdminDataProvider.UserSearch(username);
            ViewData["list"] = list;
            ViewBag.pageNow = 1;
            ViewBag.url = "/Admin/Users";
            ViewBag.rowTotalNun = list.Count;
            return View("users");
        }


        public ActionResult UserGroup()
        {
            var list = DataProvider.AdminDataProvider.GetUserGroupsList();
            ViewData["list"] = list;
            return View();
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
                result = DataProvider.AdminDataProvider.UpdateUserGroup(groupName, gid);
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
            if (DataProvider.AdminDataProvider.DeleteUser(uid))
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

        #region 分页栏
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

        public ActionResult AddNodes(int? cid)
        {
            var categoryList = DataProvider.AdminDataProvider.GetFirstLevelCategoryList();
            ViewData["categoryList"] = categoryList;
            var usergroupList = DataProvider.AdminDataProvider.GetUserGroupList();
            ViewData["usergroupList"] = usergroupList;
            Category model = new Category();
            if (cid != null)
            {
                model = DataProvider.AdminDataProvider.GetCategoryInfo((int)cid);
            }
            return View("nodeadd", model);
        }

        [HttpPost]
        public ActionResult AddNodes(Category model)
        {
            model.Permit = Request["premit"];
            bool result = false;
            if (model.CId > 0)
            {
                result = DataProvider.AdminDataProvider.UpdateCategory(model);
            }
            else
            {
                result = DataProvider.AdminDataProvider.AddCategory(model);
            }
            if (result)
            {
                TempData["result"] = "操作成功";
            }
            else
            {
                TempData["result"] = "操作成功";
            }
            return RedirectToAction("Nodes");
        }

        public ActionResult DeleteNode(int cid)
        {
            if (DataProvider.AdminDataProvider.DeleteCategory(cid))
            {
                TempData["result"] = "操作成功";
            }
            else
            {
                TempData["result"] = "操作成功";
            }
            return RedirectToAction("Nodes");
        }

        #endregion

        #region 讨论话题

        public ActionResult Topics(int page=1)
        {
            ViewData["list"] = DataProvider.AdminDataProvider.GetTopicsList(page);
            ViewBag.pageNow = page;
            ViewBag.url = "/Admin/Topics";
            ViewBag.rowTotalNun = DataProvider.AdminDataProvider.GetForumsCount();
            return View();
        }

        public ActionResult SetTop(Int16 istop, int fid)
        {
            if (DataProvider.AdminDataProvider.SetTopicTop(fid, istop))
            {
                TempData["result"] = "操作成功";
            }
            else
            {
                TempData["result"] = "操作失败";
            }
            return RedirectToAction("Topics");
        }

        public ActionResult DeleteTopic(int fid)
        {
            if (DataProvider.AdminDataProvider.DeleteTopic(fid))
            {
                TempData["result"] = "操作成功";
            }
            else
            {
                TempData["result"] = "操作失败";
            }
            return RedirectToAction("Topics");
        }

        [HttpPost]
        public ActionResult BatchDeleteTopic(FormCollection collection)
        {
            var fids = collection["fid"];
            if (string.IsNullOrWhiteSpace(fids))
            {
                TempData["result"] = "请先选择";
                return RedirectToAction("Topics");
            }
            if (DataProvider.AdminDataProvider.BatchDeleteTopic(fids))
            {
                TempData["result"] = "操作成功";
            }
            else
            {
                TempData["result"] = "操作失败";
            }
            return RedirectToAction("Topics");
        }

        #endregion

        #region 友情链接
        public ActionResult Links()
        {
            ViewData["list"] = DataProvider.AdminDataProvider.GetLinksList();
            return View();
        }

        public ActionResult EditLink(int? id)
        {
            if (id == null)
            {
                return View(new XBBS.Models.Links());
            }
            var entity = DataProvider.AdminDataProvider.GetLinkInfo((int)id);
            return View(entity);
        }

        [HttpPost]
        public ActionResult UpdateLink(Models.Links model)
        {
            bool result=false;
            if (model.Id == 0)
            {
                result = DataProvider.AdminDataProvider.AddLink(model);
            }
            else
            {
                result = DataProvider.AdminDataProvider.UpdateLink(model);
            }
            if (result)
            {
                TempData["result"] = "操作成功";
            }
            else
            {
                TempData["result"] = "操作失败";
            }
            return RedirectToAction("Links");
        }

        public ActionResult DeleteLink(int id)
        {
            if (DataProvider.AdminDataProvider.DeleteLink(id))
            {
                TempData["result"] = "操作成功";
            }
            else
            {
                TempData["result"] = "操作失败";
            }
            return RedirectToAction("Links");
        }
        #endregion

    }
}
