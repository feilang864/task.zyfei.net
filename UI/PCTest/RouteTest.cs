using System.Reflection;
using System.Web;
using System.Web.Routing;
using FFLTask.UI.PC;
using Moq;
using NUnit.Framework;

namespace FFLTask.UI.PCTest
{
    [TestFixture]
    public class RouteTest
    {
        [Test]
        public void Auth()
        {
            test_route_match("~/Auth/Grant", "Auth", "Grant");
            test_route_match("~/Auth/View", "Auth", "View");
        }

        [Test]
        public void Account()
        {
            test_route_match("~/Log/on", "Log", "On");
            test_route_match("~/Register", "Register", "Index");
            test_route_match("~/User/8", "User", "Summary", new { userId = "8" });
            test_route_match("~/User/Profile", "User", "Profile");
        }

        [Test]
        public void Help()
        {
            test_route_match("~/Help", "Help", "Welcome");
            test_route_match("~/Help/Account", "Help", "Account");
            test_route_match("~/Help/Project", "Help", "Project");
            test_route_match("~/Help/Team", "Help", "Team");
            test_route_match("~/Help/Task", "Help", "Task");
            test_route_match("~/Help/Task/Comment", "Help", "TaskChild", 
                new { child = "Comment" });
            test_route_match("~/Help/Task/Progress", "Help", "TaskChild", 
                new { child = "Progress" });
            test_route_match("~/Help/Task/Relation", "Help", "TaskChild", 
                new { child = "Relation" });
            test_route_match("~/Help/Task/Search", "Help", "TaskChild", 
                new { child = "Search" });
            test_route_match("~/Help/Welcome", "Help", "Welcome");
        }

        [Test]
        public void Message()
        {
            test_route_match("~/Message/FromMe", "Message", "FromMe");
            test_route_match("~/Message/FromMe/Page-5", "Message", "FromMe", new { pageIndex = "5" });
            test_route_match("~/Message/ToMe", "Message", "ToMe");
            test_route_match("~/Message/ToMe/Page-3", "Message", "ToMe", new { pageIndex = "3" });
        }

        [Test]
        public void Project()
        {
            test_route_match("~/Project/create", "Project", "Create");
            test_route_match("~/Project/dismiss", "Project", "Dismiss");
            test_route_match("~/Project/search", "Project", "Search");
            test_route_match("~/Project/SearchPopup", "Project", "SearchPopup");
            test_route_match("~/Project/_SearchResult", "Project", "_SearchResult");
            test_route_match("~/Project/23", "Project", "Summary", new { projectId = "23" });
            test_route_match("~/Project/Join/8", "Project", "Join", new { projectId = "8" });
            test_route_match("~/Project/HasChild/18", "Project", "HasChild", new { projectId = "18" });
            test_route_match("~/Project/Edit/18", "Project", "Edit", new { projectId = "18" });
            test_route_match("~/Project/_NextProjectNode/17", "Project", "_NextProjectNode", new { projectId = "17" });
            test_route_match("~/Project/Summary/17", "Project", "Summary", new { projectId = "17" });
        }

        [Test]
        public void Task()
        {
            test_route_match("~/Task/Publish", "Task", "Publish");
            test_route_match("~/Task/Publish", "Task", "Publish", httpMethod: "POST");
            test_route_match("~/Task/New", "Task", "New");
            test_route_match("~/Task/List", "Task", "List");
            test_route_match("~/Task/Search", "Task", "Search");
            test_route_match("~/Task/List/8", "Task", "List", new { projectId = "8" });
            test_route_match("~/Task/Edit/7", "Task", "Edit", new { taskId = "7" });
            test_route_match("~/Task/Summary/8", "Task", "Summary", new { taskId = "8" });
            test_route_match("~/Task/Clone/10", "Task", "Clone", new { taskId = "10" });
            test_route_match("~/Task/History/12", "Task", "History", new { taskId = "12" });
            test_route_match("~/Task/Sequence/12", "Task", "Sequence", new { taskId = "12" });
            test_route_match("~/Task/_Sum", "Task", "_Sum");
            test_route_match("~/Task/_SearchResult/future", "Task", "_SearchResult", new { taskName = "future" });
        }

        [Test]
        public void Team()
        {
            test_route_match("~/Team/Search", "Team", "Search");
            test_route_match("~/Team/Transfer/12/accepter/3", "Team", "Transfer",
                new { userId = "12", role = "accepter", projectId = "3" });
        }

        [Test]
        public void User()
        {
            test_route_match("~/User/HasUnknownMessage", "User", "HasUnknownMessage");
        }

        #region Private Methods

        private HttpContextBase create_http_context(string targetUrl = null, string httpMethod = "GET")
        {
            //create the mock request
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            //create the mock respons
            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            //create the mock contex, using the request and response
            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockContext.Object;
        }

        private void test_route_match(string url, string controller, string action, object routeProperties = null, string httpMethod = "GET")
        {
            //arrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            //act - process the route
            RouteData result = routes.GetRouteData(create_http_context(url, httpMethod));

            //assert
            Assert.IsNotNull(result);
            test_incoming_route_result(result, controller, action, routeProperties);
        }

        private void test_incoming_route_result(RouteData routeResult, string controller, string action, object propertySet = null)
        {
            Assert.That(routeResult.Values["controller"], Is.EqualTo(controller).IgnoreCase);
            Assert.That(routeResult.Values["action"], Is.EqualTo(action).IgnoreCase);

            if (propertySet != null)
            {
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
                foreach (var pi in propInfo)
                {
                    Assert.That(routeResult.Values.ContainsKey(pi.Name.ToLower()));
                    Assert.That(routeResult.Values[pi.Name], Is.EqualTo(pi.GetValue(propertySet, null)).IgnoreCase);
                }
            }
        }

        private void test_route_fail(string url)
        {
            //arrange
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            //act - process the route
            RouteData result = routes.GetRouteData(create_http_context(url));

            //assert
            Assert.IsTrue(result == null || result.Route == null);
        }

        #endregion
    }
}
