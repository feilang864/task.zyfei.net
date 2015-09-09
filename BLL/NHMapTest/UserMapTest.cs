using System.Collections.Generic;
using FFLTask.BLL.Entity;
using NUnit.Framework;
using System;

namespace FFLTask.BLL.NHMapTest
{
    [TestFixture]
    public class UserMapTest : BaseMapTest<User>
    {
        [Test]
        public void Normal()
        {
            string user_name = "yezi";
            string real_name = "叶子";
            string password = "1234";
            string authentication_code = "012345";
            Authorization authorization_1 = new Authorization();
            Authorization authorization_2 = new Authorization();
            Message message_1 = new Message();
            Message message_2 = new Message();
            Message message_3 = new Message();


            DateTime birthday = new DateTime(1981, 4, 24);
            string city = "重庆";
            string province = "四川";
            bool female = true;
            string greet = "叶子";
            string interested = "C# Mysql NHibernate";
            string otherContact = "email: ssss@ye.com";
            string phone = "18623151469";
            string qq = "5342132397";
            string selfDescription = "I am a very very good guy, :)";
            Profile profile = new Profile
            {
                Birthday = birthday,
                City = city,
                Province = province,
                Female = female,
                Greet = greet,
                Interested = interested,
                OtherContact = otherContact,
                Phone = phone,
                QQ = qq,
                SelfDescription = selfDescription
            };

            User user = new User
            {
                Name = user_name,
                Password = password,
                RealName = real_name,
                AuthenticationCode = authentication_code,
                Authorizations = new List<Authorization> { authorization_1, authorization_2 },
                MessagesToMe = new List<Message> { message_1 },
                MessagesFromMe = new List<Message> { message_2, message_3 },
                Profile = profile
            };

            authorization_1.User = user;
            authorization_2.User = user;
            message_1.Addressee = user;
            message_2.Addresser = user;
            message_3.Addresser = user;

            User loaded_user = Save(user);

            Assert.That(loaded_user.Name, Is.EqualTo(user_name));
            Assert.That(loaded_user.Password, Is.EqualTo(password));
            Assert.That(loaded_user.RealName, Is.EqualTo(real_name));
            DBAssert.That(loaded_user.AuthenticationCode, Is.EqualTo(authentication_code));
            Assert.That(loaded_user.Authorizations.Count, Is.EqualTo(2));
            Assert.That(loaded_user.MessagesToMe.Count, Is.EqualTo(1));
            Assert.That(loaded_user.MessagesFromMe.Count, Is.EqualTo(2));

            Assert.That(loaded_user.Profile, Is.Not.Null);
            Assert.That(loaded_user.Profile.Birthday, Is.EqualTo(birthday));
            Assert.That(loaded_user.Profile.City, Is.EqualTo(city));
            Assert.That(loaded_user.Profile.Female, Is.EqualTo(female));
            Assert.That(loaded_user.Profile.Greet, Is.EqualTo(greet));
            Assert.That(loaded_user.Profile.Interested, Is.EqualTo(interested));
            Assert.That(loaded_user.Profile.OtherContact, Is.EqualTo(otherContact));
            Assert.That(loaded_user.Profile.Phone, Is.EqualTo(phone));
            Assert.That(loaded_user.Profile.Province, Is.EqualTo(province));
            Assert.That(loaded_user.Profile.QQ, Is.EqualTo(qq));
            Assert.That(loaded_user.Profile.SelfDescription, Is.EqualTo(selfDescription));
        }
    }
}
