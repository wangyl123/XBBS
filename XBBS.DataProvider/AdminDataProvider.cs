using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XBBS.Models;

namespace XBBS.DataProvider
{
    public class AdminDataProvider
    {
        public static Dictionary<string, string> GetSettingsDict()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var list = db.Query<Models.Settings>(string.Empty);

                foreach (var item in list)
                {
                    dic.Add(item.Title, item.Value);
                }
            }
            return dic;
        }

        public static bool UpdateSettings(Dictionary<string, string> dic)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                try
                {
                    db.BeginTransaction();

                    foreach (var d in dic)
                    {
                        var r = db.Query<Models.Settings>(" where title=@0", d.Key).SingleOrDefault();
                        r.Value = d.Value;
                        db.Update(r);
                    }

                    db.CompleteTransaction();
                    return true;
                }
                catch
                {
                    db.AbortTransaction();
                    return false;
                }
            }
        }

        public static List<Models.ViewModel.UserListViewModel> GetUserList(int startPage = 1, int pageCount = 10)
        {
            List<Models.ViewModel.UserListViewModel> list = new List<Models.ViewModel.UserListViewModel>();
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var res = db.Query<Models.ViewModel.UserListViewModel>("SELECT u.uid,u.username,u.email,g.group_name,u.money FROM users u INNER JOIN user_groups g on u.gid=g.gid order by uid ").Skip(pageCount * ((startPage >= 1 ? startPage : 1) - 1)).Take(pageCount).OrderBy(s => s.Uid);
                list.AddRange(res);
            }
            return list;
        }

        public static List<Models.ViewModel.UserListViewModel> UserSearch(string key)
        {
            List<Models.ViewModel.UserListViewModel> list = new List<Models.ViewModel.UserListViewModel>();
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var res = db.Query<Models.ViewModel.UserListViewModel>("SELECT u.uid,u.username,u.email,g.group_name,u.money FROM users u INNER JOIN user_groups g on u.gid=g.gid where u.username like '%" + key + "%' order by uid ");
                list.AddRange(res);
            }
            return list;
        }

        public static List<Models.UserGroup> GetUserGroupsList()
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var res = db.Query<Models.UserGroup>("Select * from user_groups").ToList();
                return res;
            }
        }

        public static int GetUserTotalCount()
        {
            string sql = "SELECT count(*) as totalcount FROM users u INNER JOIN user_groups g on u.gid=g.gid ";
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                DataSet ds = new DataSet();
                db.Fill(ds, sql);
                try
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0]["totalcount"]);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static User GetUserInfo(int uid)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var model = db.Query<User>("Where uid=@0", uid).SingleOrDefault();
                return model;
            }
        }

        public static bool UpdateUserInfo(User model)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var entity = db.Query<User>("Where uid=@0", model.Uid).SingleOrDefault();
                /*
                var properties = typeof(User).GetProperties();
                foreach (var p in properties)
                {
                    var value = p.GetValue(model, null);
                    if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
                    {
                        p.SetValue(entity, value, null);
                    }
                }
                 * */

                entity.Email = model.Email;
                entity.Homepage = model.Homepage;
                entity.Location = model.Location;
                entity.QQ = model.QQ;
                entity.Signature = model.Signature;
                entity.Introduction = model.Introduction;
                entity.Money = model.Money;
                entity.NickName = model.NickName;
                if (string.IsNullOrWhiteSpace(model.NickName))
                {
                    entity.NickName = model.UserName;
                }
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    entity.Password = model.Password;
                }

                db.BeginTransaction();
                try
                {
                    if (entity.Gid != model.Gid)
                    {
                        var mentity = db.Query<UserGroup>("Where gid=@0", model.Gid).SingleOrDefault();
                        var eentity = db.Query<UserGroup>("where gid=@0", entity.Gid).SingleOrDefault();
                        mentity.UserNum = mentity.UserNum + 1;
                        eentity.UserNum = eentity.UserNum - 1;
                        entity.Gid = model.Gid;
                        if (db.Update(entity) > 0 && db.Update(mentity) > 0 && db.Update(eentity) > 0)
                        {
                            db.CompleteTransaction();
                            return true;
                        }
                        else
                        {
                            db.AbortTransaction();
                            return false;
                        }
                    }
                    else
                    {
                        if (db.Update(entity) > 0)
                        {
                            db.CompleteTransaction();
                            return true;
                        }
                        else
                        {
                            db.AbortTransaction();
                            return false;
                        }
                    }
                }
                catch
                {
                    db.AbortTransaction();
                    return false;
                }
            }
        }

        public static List<UserGroup> GetUserGroupList()
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Query<UserGroup>("").ToList();
            }
        }

        public static bool AddUserGroup(string groupName)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var model = new UserGroup()
                {
                    GroupName = groupName,
                    GroupType = 3,
                    UserNum = 0
                };
                return db.Insert(model) != null;
            }
        }

        public static bool UpdateUserGroup(string groupName, object gid)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var model = db.Query<UserGroup>("where gid=@0", Convert.ToInt32(gid)).SingleOrDefault();
                model.GroupName = groupName;
                return db.Update(model) > 0;
            }
        }

        public static bool DeleteUserGroup(int gid)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var model = db.Query<UserGroup>("where gid=@0", gid).SingleOrDefault();
                return db.Delete(model) > 0;
            }
        }

        public static bool DeleteUser(int uid)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var model = db.Query<User>("Where uid=@0", uid).SingleOrDefault();
                return db.Delete(model) > 0;
            }
        }

        public static List<Category> GetAllCategoryList()
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var list = db.Query<Category>("").ToList();
                return list;
            }
        }

        public static List<Category> GetFirstLevelCategoryList()
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var list = db.Query<Category>("where pid='0'").ToList();
                return list;
            }
        }

        public static bool AddCategory(Category model)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                model.ListNum = 0;
                return db.Insert(model) != null;
            }
        }

        public static Category GetCategoryInfo(int cid)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Query<Category>("where cid=@0", cid).SingleOrDefault();
            }
        }

        public static bool UpdateCategory(Category model)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var entity = GetCategoryInfo(model.CId);
                entity.CName = model.CName;
                entity.Content = model.Content;
                entity.Keywords = model.Keywords;
                entity.Master = model.Master;
                entity.Permit = model.Permit;
                entity.PId = model.PId;
                return db.Update(entity) > 0;
            }
        }

        public static bool DeleteCategory(int cid)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Delete(db.Query<Category>("where cid=@0", cid).SingleOrDefault()) > 0;
            }
        }

        public static int GetUserCount()
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Query<int>("select count(uid) from users").SingleOrDefault();
            }
        }

        public static int GetForumsCount()
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Query<int>("SELECT count(fid) FROM forums where is_hidden=0").SingleOrDefault();
            }
        }

        public static int GetCommentsCount()
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Query<int>("SELECT count(id) FROM comments c inner JOIN  forums f on f.fid=c.fid where f.is_hidden=0").SingleOrDefault();
            }
        }

        public static List<Models.ViewModel.TopicManageModel> GetTopicsList(int startPage = 1, int pageCount = 10)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Query<Models.ViewModel.TopicManageModel>("select f.fid,f.title,f.addtime,c.cname,u.username,c.cid,u.uid,count(co.id) as replycount,f.is_top  from forums f INNER JOIN categories c on c.cid=f.cid INNER JOIN users u on u.uid=f.uid left JOIN comments co on co.fid=f.fid where f.is_hidden=0 group by f.fid,f.title,f.addtime,c.cname,u.username,u.uid,c.cid,f.is_top ORDER BY f.is_top DESC, f.addtime DESC").Skip(pageCount * ((startPage >= 1 ? startPage : 1) - 1)).Take(pageCount).ToList();
            }
        }

        public static bool SetTopicTop(int fid, Int16 istop)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var entity = db.Query<Models.Forums>("where fid=@0", fid).SingleOrDefault();
                entity.IsTop = istop;
                return db.Update(entity) > 0;
            }
        }

        public static bool DeleteTopic(int fid)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var entity = db.Query<Models.Forums>("where fid=@0", fid).SingleOrDefault();
                entity.IsHidden = 1;
                return db.Update(entity) > 0;
            }
        }

        public static bool BatchDeleteTopic(string fids)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var sql = "UPDATE forums SET is_hidden=1 WHERE fid in(" + fids + ") ";
                var result = db.Execute(sql);
                return result > 0;
            }
        }

        public static List<Models.Links> GetLinksList()
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Query<Models.Links>("order by id").ToList();
            }
        }

        public static Models.Links GetLinkInfo(int id)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Query<Models.Links>("where id=@0", id).SingleOrDefault();
            }
        }

        public static bool AddLink(Models.Links model)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Insert(model) != null;
            }
        }

        public static bool UpdateLink(Models.Links model)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var entity = db.Query<Models.Links>("where id=@0", model.Id).SingleOrDefault();
                entity.IsHidden = model.IsHidden;
                entity.Logo = model.Logo;
                entity.Name = model.Name;
                entity.Url = model.Url;
                return db.Update(entity) > 0;
            }
        }

        public static bool DeleteLink(int id)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Delete<Models.Links>(id) > 0;
            }
        }
    }
}
