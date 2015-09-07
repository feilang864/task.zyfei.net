using FFLTask.BLL.Entity;
using Global.Core.ExtensionMethod;
using NUnit.Framework;

namespace FFLTask.BLL.EntityTest
{
    class TaskHierarchyTest
    {
        [Test]
        public void AddChild()
        {
            Task parent = new Task();
            Task child_1 = new Task();

            parent.AddChild(child_1);
            Assert.That(child_1.Sequence, Is.EqualTo(1));
            Assert.That(parent.Children.Count, Is.EqualTo(1));
            Assert.That(parent.Children.Contains(child_1));

            Task child_2 = new Task();
            parent.AddChild(child_2);
            Assert.That(child_2.Sequence, Is.EqualTo(2));
            Assert.That(parent.Children.Count, Is.EqualTo(2));
            Assert.That(parent.Children.Contains(child_1));
            Assert.That(parent.Children.Contains(child_2));
        }

        [Test]
        public void RemoveChild()
        {
            Task parent = new Task();
            Task child_1 = new Task();
            Task child_2 = new Task();

            parent.AddChild(child_1);
            parent.AddChild(child_2);

            parent.RemoveChild(child_1);

            Assert.That(parent.Children.Count, Is.EqualTo(1));
            Assert.That(parent.Children.Contains(child_2));
            Assert.That(child_1.Parent, Is.Null);
        }

        [Test]
        public void Remove_And_Add_Child()
        {
            Task parent = new Task();
            Task child_1 = new Task();
            Task child_2 = new Task();

            parent.AddChild(child_1);
            parent.AddChild(child_2);

            parent.RemoveChild(child_1);
            parent.AddChild(child_1);

            Assert.That(parent.Children.Count, Is.EqualTo(2));
            Assert.That(parent.Children.Contains(child_1));
            Assert.That(parent.Children.Contains(child_2));
            Assert.That(child_1.Parent, Is.EqualTo(parent));

            Assert.That(child_1.Sequence, Is.EqualTo(3));
        }

        [Test]
        public void ChangeParent()
        {
            Task parent_1 = new Task();
            Task parent_2 = new Task();
            Task child = new Task();

            parent_1.AddChild(child);
            Assert.That(child.Parent, Is.EqualTo(parent_1));
            Assert.That(parent_1.Children.Count, Is.EqualTo(1));
            Assert.That(parent_1.Children.Contains(child));

            child.ChangeParent(parent_2);
            Assert.That(child.Parent, Is.EqualTo(parent_2));
            Assert.That(parent_1.Children.IsNullOrEmpty(), Is.True);
            Assert.That(parent_2.Children.Count, Is.EqualTo(1));
            Assert.That(parent_2.Children.Contains(child));
        }

        [Test]
        public void ChangeParent_When_New_Parent_Is_Old_Parent()
        {
            Task parent = new Task();
            Task child_1 = new Task();
            Task child_2 = new Task();
            parent.AddChild(child_1);
            parent.AddChild(child_2);

            child_1.ChangeParent(parent);
            Assert.That(child_1.Parent, Is.EqualTo(parent));
            Assert.That(child_1.Sequence, Is.EqualTo(1));
            Assert.That(child_2.Parent, Is.EqualTo(parent));
            Assert.That(child_2.Sequence, Is.EqualTo(2));
            Assert.That(parent.Children.Count, Is.EqualTo(2));
            Assert.That(parent.Children.Contains(child_1));
            Assert.That(parent.Children.Contains(child_1));
        }

        [Test]
        public void ChangeParent_When_Child_Has_No_Parent()
        {
            Task new_parent = new Task();
            Task child = new Task();

            child.ChangeParent(new_parent);
            Assert.That(child.Parent, Is.EqualTo(new_parent));
            Assert.That(new_parent.Children.Count, Is.EqualTo(1));
            Assert.That(new_parent.Children.Contains(child));
        }

        [Test]
        public void RemoveParent()
        {
            Task parent = new Task();
            Task child = new Task();

            //child no parent
            child.RemoveParent();
            Assert.That(child.Parent, Is.Null);

            parent.AddChild(child);

            //child have parent
            child.RemoveParent();
            Assert.That(child.Parent, Is.Null);
            Assert.That(parent.Children.IsNullOrEmpty(), Is.True);
        }

        [Test]
        public void IsOffspring()
        {
            Task child_1 = new Task();
            Task child_2 = new Task();
            Task parent_1 = new Task();
            Task parent_2 = new Task();
            Task grand_parent = new Task();

            Task other = new Task();

            parent_1.AddChild(child_1);
            parent_1.AddChild(child_2);
            grand_parent.AddChild(parent_1);
            grand_parent.AddChild(parent_2);

            Assert.That(child_1.IsOffspring(parent_1), Is.True);
            Assert.That(child_2.IsOffspring(parent_1), Is.True);
            Assert.That(parent_1.IsOffspring(parent_1), Is.False);
            Assert.That(parent_2.IsOffspring(parent_1), Is.False);
            Assert.That(grand_parent.IsOffspring(parent_1), Is.False);
            Assert.That(other.IsOffspring(parent_1), Is.False);

            Assert.That(child_1.IsOffspring(parent_2), Is.False);
            Assert.That(child_2.IsOffspring(parent_2), Is.False);
            Assert.That(parent_1.IsOffspring(parent_2), Is.False);
            Assert.That(parent_2.IsOffspring(parent_2), Is.False);
            Assert.That(grand_parent.IsOffspring(parent_1), Is.False);
            Assert.That(other.IsOffspring(parent_1), Is.False);

            Assert.That(child_1.IsOffspring(grand_parent), Is.True);
            Assert.That(child_2.IsOffspring(grand_parent), Is.True);
            Assert.That(parent_1.IsOffspring(grand_parent), Is.True);
            Assert.That(parent_2.IsOffspring(grand_parent), Is.True);
            Assert.That(grand_parent.IsOffspring(grand_parent), Is.False);
            Assert.That(other.IsOffspring(grand_parent), Is.False);
        }
    }
}
