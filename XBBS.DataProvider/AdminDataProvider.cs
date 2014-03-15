using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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
                var res = db.Query<Models.ViewModel.UserListViewModel>("SELECT u.uid,u.username,u.email,g.group_name,u.money FROM jexus_users u INNER JOIN jexus_user_groups g on u.gid=g.gid ").Skip(pageCount * ((startPage >= 1 ? startPage : 1) - 1)).Take(pageCount).OrderBy(s=>s.Uid);
                list.AddRange(res);
            }
            return list;
        }

        public static List<Models.UserGroup> GetUserGroupsList()
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var res = db.Query<Models.UserGroup>("Select * from jexus_user_groups").ToList();
                return res;
            }
        }

        public static int GetUserTotalCount()
        {
            string sql = "SELECT count(*) as totalcount FROM jexus_users u INNER JOIN jexus_user_groups g on u.gid=g.gid ";
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
    }
}
