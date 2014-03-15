using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XBBS.DataProvider
{
    public class SettingsDataProvider
    {
        public static bool UpdateUserInfo(Models.User user)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var u = db.Query<Models.User>("WHERE uid=@0 ", user.Uid).SingleOrDefault();
                u = user;
                return db.Update(u) >= 0;
            }
        }

        public static Models.User GetUserInfo(int uid)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var u = db.Query<Models.User>("WHERE uid=@0 ", uid).SingleOrDefault();
                return u;
            }
        }
    }
}
