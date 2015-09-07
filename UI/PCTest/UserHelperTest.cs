using NUnit.Framework;
using System.Web;
using Moq;
using FFLTask.UI.PC;
using FFLTask.UI.PC.CustomException;
using FFLTask.UI.PC.WebHelper;
using Global.Core.ExtensionMethod;

namespace FFLTask.UI.PCTest
{
    [TestFixture]
    public class UserHelperTest
    {
        UserHelper userHelper;

        [SetUp]
        public void SetUp()
        {
            userHelper = new UserHelper();
        }

        [Test]
        public void Current_User_Id_Normal()
        {
            set_request_context(userHelper);

            int user_id = 23;

            HttpCookie cookie = add_user_id_into_cookie(user_id);
            add_encrypt_into_user_id_cookie(cookie, user_id);
            add_user_id_cookie_into_request_context(cookie);

            Assert.That(userHelper.CurrentUserId, Is.EqualTo(user_id));
        }

        [Test]
        public void Current_User_Id_Null()
        {
            set_request_context(userHelper);
            Assert.That(userHelper.CurrentUserId, Is.Null);
        }

        [Test]
        [ExpectedException(typeof(CookieParseException),
            ExpectedMessage = "cookie for user id(id=23) has no encryption info.")]
        public void Current_User_Id_No_Encrypt()
        {
            set_request_context(userHelper);

            int user_id = 23;

            var cookie = add_user_id_into_cookie(user_id);
            add_user_id_cookie_into_request_context(cookie);

            int? retrieved_id = userHelper.CurrentUserId;
        }

        [Test]
        [ExpectedException(typeof(CookieParseException),
            ExpectedMessage = "cookie for user id(id=23) has wrong encryption info.")]
        public void Current_User_Id_Wrong_Encrypt()
        {
            set_request_context(userHelper);

            int user_id = 23;

            var cookie = add_user_id_into_cookie(user_id);

            string encrypted = user_id.ToString().Md5Encypt() + "wrong!";
            cookie.Values.Add(CookieKey.EncryptUserId, encrypted);

            add_user_id_cookie_into_request_context(cookie);

            int? retrieved_id = userHelper.CurrentUserId;
        }

        private void set_request_context(UserHelper userHelper)
        {
            var request = new Mock<HttpRequestBase>();
            request.Setup(x => x.Cookies).Returns(new HttpCookieCollection());

            var context = new Mock<HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            userHelper.Context = context.Object;
        }

        private void add_user_id_cookie_into_request_context(HttpCookie cookie)
        {
            userHelper.Context.Request.Cookies.Add(cookie);
        }

        private void add_encrypt_into_user_id_cookie(HttpCookie cookie, int user_id)
        {
            string encrypted = user_id.ToString().Md5Encypt();
            cookie.Values.Add(CookieKey.EncryptUserId, encrypted);
        }

        private HttpCookie add_user_id_into_cookie(int user_id)
        {
            string cookie_name = CookieKey.UserId;
            HttpCookie cookie = new HttpCookie(cookie_name, user_id.ToString());

            return cookie;
        }
    }
}
