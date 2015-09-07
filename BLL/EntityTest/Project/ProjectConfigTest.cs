using System.Collections.Generic;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using NUnit.Framework;

namespace FFLTask.BLL.EntityTest
{
    [TestFixture]
    public class ProjectConfigTest
    {
        [Test]
        public void GetDifficulties()
        {
            ProjectConfig config = new ProjectConfig();
            IList<TaskDifficulty> difficulties;

            #region StrDifficulties is null

            difficulties = config.GetDifficulties();
            Assert.That(difficulties.Count, Is.EqualTo(5));
            Assert.That(difficulties.Contains(TaskDifficulty.Easiest));
            Assert.That(difficulties.Contains(TaskDifficulty.Easy));
            Assert.That(difficulties.Contains(TaskDifficulty.Common));
            Assert.That(difficulties.Contains(TaskDifficulty.Hard));
            Assert.That(difficulties.Contains(TaskDifficulty.Hardest));

            #endregion

            #region StrDifficulties not null

            config.SetDifficulties(
                new List<TaskDifficulty> 
                {
                    TaskDifficulty.Easy,
                    TaskDifficulty.Hard
                });

            difficulties = config.GetDifficulties();
            Assert.That(difficulties.Count, Is.EqualTo(2));
            Assert.That(difficulties.Contains(TaskDifficulty.Easy));
            Assert.That(difficulties.Contains(TaskDifficulty.Hard));

            #endregion
        }

        [Test]
        public void SetDifficulties()
        {
            ProjectConfig config = new ProjectConfig();

            var difficulties = new List<TaskDifficulty> { TaskDifficulty.Easy, TaskDifficulty.Hard };
            config.SetDifficulties(difficulties);

            var result_difficulties = config.GetDifficulties();
            Assert.That(result_difficulties.Count, Is.EqualTo(2));
            Assert.That(result_difficulties.Contains(TaskDifficulty.Easy));
            Assert.That(result_difficulties.Contains(TaskDifficulty.Hard));
        }

        [Test]
        public void GetPrioritys()
        {
            ProjectConfig config = new ProjectConfig();
            IList<TaskPriority> prioritys;

            #region StrPrioritys is null

            prioritys = config.GetPrioritys();
            Assert.That(prioritys.Count, Is.EqualTo(5));
            Assert.That(prioritys.Contains(TaskPriority.Highest));
            Assert.That(prioritys.Contains(TaskPriority.High));
            Assert.That(prioritys.Contains(TaskPriority.Common));
            Assert.That(prioritys.Contains(TaskPriority.Low));
            Assert.That(prioritys.Contains(TaskPriority.Lowest));

            #endregion

            #region StrPrioritys not null

            config.SetPrioritys(
                new List<TaskPriority> 
                {
                    TaskPriority.Lowest,
                    TaskPriority.High
                });

            prioritys = config.GetPrioritys();
            Assert.That(prioritys.Count, Is.EqualTo(2));
            Assert.That(prioritys.Contains(TaskPriority.Lowest));
            Assert.That(prioritys.Contains(TaskPriority.High));

            #endregion
        }

        [Test]
        public void SetPrioritys()
        {
            ProjectConfig config = new ProjectConfig();

            var prioritys = new List<TaskPriority> { TaskPriority.Highest };
            config.SetPrioritys(prioritys);

            var result_prioritys = config.GetPrioritys();
            Assert.That(result_prioritys.Count, Is.EqualTo(1));
            Assert.That(result_prioritys.Contains(TaskPriority.Highest));
        }

        [Test]
        public void GetQualities()
        {
            ProjectConfig config = new ProjectConfig();
            IList<TaskQuality> qualities;

            #region StrQualities is null

            qualities = config.GetQualities();
            Assert.That(qualities.Count, Is.EqualTo(3));
            Assert.That(qualities.Contains(TaskQuality.Qualified));
            Assert.That(qualities.Contains(TaskQuality.Good));
            Assert.That(qualities.Contains(TaskQuality.Perfect));

            #endregion

            #region StrQualities not null

            config.SetQualities(new List<TaskQuality> { TaskQuality.Good });

            qualities = config.GetQualities();
            Assert.That(qualities.Count, Is.EqualTo(1));
            Assert.That(qualities.Contains(TaskQuality.Good));

            #endregion
        }

        [Test]
        public void SetQualities()
        {
            ProjectConfig config = new ProjectConfig();

            IList<TaskQuality> qualities = new List<TaskQuality> { TaskQuality.Good, TaskQuality.Perfect };
            config.SetQualities(qualities);
            qualities = config.GetQualities();

            var result_qualities = config.GetQualities();
            Assert.That(result_qualities.Count, Is.EqualTo(2));
            Assert.That(result_qualities.Contains(TaskQuality.Good));
            Assert.That(result_qualities.Contains(TaskQuality.Perfect));
        }
    }
}
