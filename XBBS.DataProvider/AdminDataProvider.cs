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

    }
}
