using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FFLTask.BLL.Entity;

namespace FFLTask.BLL.EntityTest
{
    class TaskLockTest
    {
        [Test]
        public void BeginEdit()
        {
            Task task = new Task();
            User user_1 = new User();

            task.BeginEdit(user_1);
            Assert.That(task.EditingBy, Is.EqualTo(user_1));
        }

        [Test]
        public void EndEdit()
        {
            Task task = new Task();
            User user_1 = new User();

            task.BeginEdit(user_1);
            task.EndEdit();
            Assert.That(task.EditingBy, Is.Null);
        }
    }
}
