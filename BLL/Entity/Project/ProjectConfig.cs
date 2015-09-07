using System;
using System.Collections.Generic;
using System.Linq;
using FFLTask.GLB.Global.Enum;
using Newtonsoft.Json;

namespace FFLTask.BLL.Entity
{
    public class ProjectConfig
    {
        protected internal virtual string StrDifficulties { get; set; }
        public virtual IList<TaskDifficulty> GetDifficulties()
        {
            return translate<TaskDifficulty>(StrDifficulties);
        }
        public virtual void SetDifficulties(IList<TaskDifficulty> difficulties)
        {
            StrDifficulties = translate<TaskDifficulty>(difficulties);
        }

        protected internal virtual string StrPrioritys { get; set; }
        public virtual IList<TaskPriority> GetPrioritys()
        {
            return translate<TaskPriority>(StrPrioritys);
        }
        public virtual void SetPrioritys(IList<TaskPriority> prioritys)
        {
            StrPrioritys = translate<TaskPriority>(prioritys);
        }

        protected internal virtual string StrQualities { get; set; }
        public virtual IList<TaskQuality> GetQualities()
        {
            return translate<TaskQuality>(StrDifficulties);
        }
        public virtual void SetQualities(IList<TaskQuality> qualities)
        {
            StrDifficulties = translate<TaskQuality>(qualities);
        }

        private IList<T> translate<T>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
            {
                return Enum.GetValues(typeof(T)).Cast<T>().ToList();
            }
            return JsonConvert.DeserializeObject<IList<T>>(jsonStr);
        }

        private string translate<T>(IList<T> collection)
        {
            return JsonConvert.SerializeObject(collection);
        }
    }
}
