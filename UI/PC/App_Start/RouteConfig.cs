using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FFLTask.UI.PC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Help
            
            routes.MapRoute(
               "HelpTaskChild",
               "Help/Task/{child}",
                new { controller = "Help", action = "TaskChild"}
            );

            routes.MapRoute(
               "HelpWelcome",
               "Help",
                new { controller = "Help", action = "Welcome" }
            );

            #endregion

            #region Project

            routes.MapRoute(
               "ProjectSummary",
               "Project/{projectId}",
                new { controller = "Project", action = "Summary" },
                new { projectId = @"\d+" }
            );

            routes.MapRoute(
               "ProjectWithProjectId",
               "Project/{action}/{projectId}",
                new { controller = "Project" },
                new { projectId = @"\d+" }
            );

            #endregion

            #region Register

            routes.MapRoute(
                "Register",
                "Register",
                 new { controller = "Register", action = "Index" }
            );

            #endregion

            #region Task

            routes.MapRoute(
               "TaskNew",
               "Task/New/{projectId}",
                new { controller = "Task", action = "New" }
            );

            routes.MapRoute(
               "TaskList",
               "Task/List/{projectId}",
                new { controller = "Task", action = "List", projectId = UrlParameter.Optional }
            );

            routes.MapRoute(
               "Task_SearchResult",
               "Task/_SearchResult/{taskName}",
                new { controller = "Task", action = "_SearchResult" }
            ); 
            
            routes.MapRoute(
               "Task",
               "Task/{action}/{taskId}",
                new { controller = "Task", action = "action" },
                new { taskId = @"\d+" }
            );

            #endregion

            #region Team

            routes.MapRoute(
                "TeamTransfer",
                "Team/Transfer/{userId}/{role}/{projectId}",
                 new { controller = "Team", action = "Transfer", },
                 new { userId = @"\d+", projectId  = @"\d+" }
            );

            #endregion

            #region User

            routes.MapRoute(
               "UserSummary",
               "User/{userId}",
                new { controller = "User", action = "Summary" },
                new { userId = @"\d+" }
            );

            #endregion

            routes.MapRoute(
                name: "Paged",
                url: "{controller}/{action}/Page-{pageIndex}",
                defaults: new { },
                constraints: new { pageIndex = @"\d+" }
            );

            //TODO: seems not a good way to set home page as /Log/On
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Log", action = "On", id = UrlParameter.Optional }
            );


        }
    }
}