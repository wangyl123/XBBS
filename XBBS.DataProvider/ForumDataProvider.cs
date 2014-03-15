using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XBBS.DataProvider
{
    public class ForumDataProvider
    {

        public static List<Models.Category> GetAllCategory()
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Query<Models.Category>("").ToList();
            }
        }


        public static bool AddForum(Models.Forums form)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Insert(form) != null;
            }

        }

        public static List<Models.Forums> GetForums(int cid)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.Query<Models.Forums>("WHERE cid=@0", cid).ToList();
            }
        }


        public static List<Models.Forums> GetLastForums(int top, int skip = 0)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.SkipTake<Models.Forums>(skip, top, "  ORDER BY addtime DESC").ToList();
            }
        }
        public static Models.Forums GetForum(int id)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var f = db.SingleOrDefault<Models.Forums>("WHERE fid=@0", id);
                if (f != null)
                {
                    f.Views++;
                    db.Update(f, new string[] { "views" });
                }
                return f;
            }
        }

        public static Models.Category GetCategory(int id)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.SingleOrDefault<Models.Category>("WHERE cid=@0", id);
            }
        }

        public static bool UpdateForum(Models.Forums forum)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                forum.UpdateTime = DateTime.Now;
                db.Save(forum);
                return true;
            }
        }

        public static bool AddComment(Models.Comment comment)
        {
            var f = GetForum(comment.FId);
            if (f == null) return false;
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                f.LastReply = DateTime.Now;
                f.Comments++;
                f.Ruid = comment.UId;
                db.Update(f, new string[] { "lastreply", "comments","ruid" });

                return db.Insert(comment) != null;
            }
        }

        public static int TotalComment(int fid)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                return db.ExecuteScalar<int>("SELECT COUNT(1) FROM stb_comments  WHERE fid=@0", fid);
            }

        }

        public static List<Models.Comment> GetComments(int fid, int pageSize, int pageIndex, ref int total)
        {
            using (PetaPoco.Database db = new PetaPoco.Database("sqlconnection"))
            {
                var pg = db.Page<Models.Comment>(pageIndex, pageSize, "WHERE fid =@0 ", fid);
                total = (int)pg.TotalPages;
                return pg.Items;
            }
        }
    }
}
