using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace XBBS.WEB
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null, "section", new { controller = "Home", action = "Section", id = UrlParameter.Optional });
            routes.MapRoute(null, "admin", new { controller = "Admin", action = "Settings_Web", id = UrlParameter.Optional });
            routes.MapRoute(null, "admin/Users/{page}", new { controller = "Admin", action = "Users", page = UrlParameter.Optional });
            routes.MapRoute(null, "admin/UserEdit/{uid}", new { controller = "Admin", action = "UserEdit", uid = UrlParameter.Optional });
            routes.MapRoute(null, "admin/UserGroupEdit/{groupName}/{gid}", new { controller = "Admin", action = "UserGroupEdit", groupName = UrlParameter.Optional, gid = UrlParameter.Optional });
            routes.MapRoute(null, "Admin/DeleteGroup/{gid}", new { controller = "Admin", action = "DeleteGroup", gid = UrlParameter.Optional });
            routes.MapRoute(null, "Admin/DeleteUser/{uid}", new { controller = "Admin", action = "DeleteUser", uid = UrlParameter.Optional });
            routes.MapRoute(null, "Admin/AddNodes/{cid}", new { controller = "Admin", action = "AddNodes", cid = UrlParameter.Optional });
            routes.MapRoute(null, "Admin/DeleteNode/{cid}", new { controller = "Admin", action = "DeleteNode", cid = UrlParameter.Optional });
            routes.MapRoute(null, "Admin/SetTop/{istop}/{fid}", new { controller = "Admin", action = "SetTop", istop = UrlParameter.Optional, fid = UrlParameter.Optional });
            routes.MapRoute(null, "Admin/DeleteTopic/{fid}", new { controller = "Admin", action = "DeleteTopic", fid = UrlParameter.Optional });
            routes.MapRoute(null, "admin/{action}", new { controller = "Admin", action = "Settings_Web", id = UrlParameter.Optional });


            routes.MapRoute(null, "settings", new { controller = "Settings", action = "Profile", id = UrlParameter.Optional });
            routes.MapRoute(null, "user", new { controller = "Home", action = "User", id = UrlParameter.Optional });
            routes.MapRoute(null, "Topic/{id}/{page}", new { controller = "Forum", action = "Topic", page = 1, id = UrlParameter.Optional });

            routes.MapRoute(null, "node/{id}", new { controller = "Forum", action = "Node", id = UrlParameter.Optional });

            routes.MapRoute(name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}