using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Task;
using FFLTask.SRV.ViewModelMap;
using NUnit.Framework;

namespace FFLTask.SRV.ViewModelMapTest
{
    public class TaskMapTest
    {
        [Test]
        public void TaskRelationModel_FilledBy_Task()
        {
            Task root = new Task();
            Task parent_parent = new Task();
            Task parent = new Task();
            Task current = new Task();
            Task brothers_1 = new Task();
            Task brothers_2 = new Task();
            Task child_1 = new Task();
            Task child_2 = new Task();
            Task child_3 = new Task();

            root.MockId(1);
            parent_parent.MockId(2);
            parent.MockId(3);
            current.MockId(4);
            brothers_1.MockId(5);
            brothers_2.MockId(6);
            child_1.MockId(7);
            child_2.MockId(8);
            child_3.MockId(9);

            parent_parent.Parent = root;
            parent.Parent = parent_parent;
            current.Parent = parent;
            brothers_1.Parent = parent;
            brothers_2.Parent = parent;
            child_1.Parent = current;
            child_2.Parent = current;
            child_3.Parent = current;

            root.Children = new List<Task> { parent_parent };
            parent_parent.Children = new List<Task> { parent };
            parent.Children = new List<Task> 
            {
                brothers_1,
                current,
                brothers_2
            };
            current.Children = new List<Task>
            {
                child_1,
                child_2,
                child_3
            };

            TaskRelationModel model = new TaskRelationModel();
            model.FilledBy(current);

            areEqual(model.Current, current);

            Assert.That(model.Ancestor.Count, Is.EqualTo(3));
            var first = model.Ancestor.First;
            areEqual(first.Value, root);
            var second = first.Next;
            areEqual(second.Value, parent_parent);
            var third = second.Next;
            areEqual(third.Value, parent);

            Assert.That(model.Brothers.Count, Is.EqualTo(2));
            contains(model.Brothers, brothers_1);
            contains(model.Brothers, brothers_1);

            Assert.That(model.Children.Count, Is.EqualTo(3));
            contains(model.Children, child_1);
            contains(model.Children, child_2);
            contains(model.Children, child_3);
        }

        private void areEqual(LiteItemModel model, Task task)
        {
            Assert.That(model.Id, Is.EqualTo(task.Id));
        }

        private void contains(IList<LiteItemModel> models, Task task)
        {
            Assert.That(models.SingleOrDefault(x => x.Id == task.Id), Is.Not.Null);
        }
    }
}
