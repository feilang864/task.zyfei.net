using System;
using System.Collections.Generic;
using System.Linq;
using FFLTask.GLB.Global.Enum;
using Global.Core.ExtensionMethod;
using Global.Core.Helper;

namespace FFLTask.BLL.Entity
{
    public class Task : BaseEntity
    {
        #region Mapped Properties

        public virtual User Publisher { get; set; }
        public virtual User EditingBy { get; set; }
        public virtual User Accepter { get; set; }

        public virtual bool IsVirtual { get; set; }
        public virtual string Title { get; set; }
        public virtual string Body { get; set; }
        public virtual Project Project { get; set; }

        //TODO: why the set can't be private???
        public virtual Status CurrentStatus { get; set; }
        public virtual void MockCurrentStatus(Status mockStatus)
        {
            CurrentStatus = mockStatus;
        }

        public virtual int Sequence { get; set; }
        public virtual TaskDifficulty? Difficulty { get; set; }
        public virtual TaskPriority? Priority { get; set; }
        public virtual int? ExpectWorkPeriod { get; set; }
        public virtual DateTime? ExpectCompleteTime { get; set; }
        public virtual DateTime? AssignTime { get; set; }

        public virtual DateTime? OwnTime { get; set; }
        public virtual User Owner { get; set; }
        public virtual Task Parent { get; set; }
        public virtual IList<Task> Children { get; set; }

        public virtual DateTime? ActualCompleteTime { get; set; }
        public virtual IList<WorkPeriod> WorkPeriods { get; set; }
        public virtual int? ActualWorkPeriod { get; protected set; }
        public virtual void MockActualWorkPeriod(int mockActualWorkPeriod)
        {
            ActualWorkPeriod = mockActualWorkPeriod;
        }

        public virtual int? OverDue { get; set; }
        public virtual int? Delay { get; set; }

        public virtual DateTime? AcceptTime { get; set; }
        public virtual bool HasAccepted { get; set; }
        public virtual TaskQuality? Quality { get; set; }

        public virtual DateTime LatestUpdateTime { get; set; }

        public virtual IList<Attachment> Attachments { get; set; }
        public virtual IList<HistoryItem> Histroy { get; set; }

        //public virtual IList<History> Histories { get; set; }

        #endregion

        #region UnMapped

        public virtual NodeType NodeType
        {
            get
            {
                if (Parent == null)
                {
                    return NodeType.Root;
                }
                else
                {
                    if (Children.IsNullOrEmpty())
                    {
                        return NodeType.Leaf;
                    }
                    else
                    {
                        return NodeType.Branch;
                    }
                }
            }
        }

        #endregion

        #region Methods

        #region Work Progress

        public virtual void Publish()
        {
            CurrentStatus = Status.Publish;
            setLatestUpdate();
            addHistory(Publisher, Constants.DescriptionPublish);
        }

        public virtual void UpdateProperty()
        {
            setLatestUpdate();

            if (CurrentStatus == Status.Doubt)
            {
                CurrentStatus = Status.Update;
            }
            addHistory(Publisher, Constants.DescriptionUpdate);
        }

        public virtual void Assign()
        {
            AssignTime = SystemTime.Now();
            CurrentStatus = Status.Assign;
            setLatestUpdate();
            addHistory(Publisher, Constants.DescriptionAssign);
        }

        public virtual void CancelAssign()
        {
            CurrentStatus = Status.Publish;
            setLatestUpdate();
            addHistory(Publisher, Constants.DescriptionCancelAssign);
            Owner = null;
        }

        public virtual void Remove()
        {
            CurrentStatus = Status.Remove;
            setLatestUpdate();
            addHistory(Publisher, Constants.DescriptionRemove);
        }

        public virtual void Resume()
        {
            CurrentStatus = Histroy.OrderByDescending(x => x.CreateTime).ElementAt<HistoryItem>(1).Status;
            setLatestUpdate();
            addHistory(Publisher, Constants.DescriptionResume);
        }

        public virtual void Own(string description = Constants.CommentAutoOwn)
        {
            OwnTime = SystemTime.Now();
            CurrentStatus = Status.Own;
            setLatestUpdate();
            addHistory(Owner, description);
        }

        public virtual void BeginWork(string description = Constants.DescriptionBeginWork)
        {
            if (Parent != null
                && Parent.IsVirtual
                && Parent.CurrentStatus < Status.BeginWork)
            {
                if (Parent.Owner == null)
                {
                    Parent.Owner = Owner;
                    Parent.Own(Constants.CommentAutoOwn);
                }
                Parent.BeginWork(Constants.CommentAutoBegin);
            }

            workPeriodBegin();
            CurrentStatus = Status.BeginWork;
            setLatestUpdate();
            addHistory(Owner, description);
        }

        public virtual void Complete(string description = Constants.DescriptionComplete)
        {
            ActualCompleteTime = SystemTime.Now();
            workPeriodEnd();
            CurrentStatus = Status.Complete;
            setLatestUpdate();
            addHistory(Owner, description);
            OverDue = ActualWorkPeriod - ExpectWorkPeriod;

            if (ActualCompleteTime.HasValue && ExpectCompleteTime.HasValue)
            {
                Delay = (ActualCompleteTime.Value - ExpectCompleteTime.Value).Hours;
            }

            if (Accepter == Owner)
            {
                Accept(null, Constants.CommentAutoAcceptForOwnerIsAccepter);
            }
        }

        public virtual void AutoCompleteAncestors()
        {
            if (Parent != null && Parent.IsVirtual)
            {
                if (Parent.Children.All(t => t.CurrentStatus == Status.Complete))
                {
                    Parent.Complete(Constants.CommentAutoComplete);
                    Parent.AutoCompleteAncestors();
                }
            }
        }

        public virtual void Pause()
        {
            CurrentStatus = Status.Pause;
            workPeriodEnd();
            setLatestUpdate();
            addHistory(Owner, Constants.DescriptionPause);
        }

        public virtual void Doubt()
        {
            CurrentStatus = Status.Doubt;
            workPeriodEnd();
            setLatestUpdate();
            addHistory(Owner, Constants.DescriptionDoubt);
        }

        public virtual void Quit()
        {
            CurrentStatus = Status.Quit;
            setLatestUpdate();
            addHistory(Owner, Constants.DescriptionGiveup);
            Owner = null;
        }

        public virtual void Accept(TaskQuality? quality, string description = Constants.DescriptionAccept)
        {
            Quality = quality;
            AcceptTime = SystemTime.Now();
            CurrentStatus = Status.Accept;
            setLatestUpdate();
            HasAccepted = true;
            if (quality.HasValue)
            {
                addHistory(Accepter, string.Format("验收通过，质量为{0}", Quality.GetEnumDescription()));
            }
            else
            {
                addHistory(Accepter, description);
            }
        }

        public virtual void AutoAcceptAncestors()
        {
            if (Parent != null && Parent.IsVirtual)
            {
                if (Parent.Children.All(t => t.CurrentStatus == Status.Accept))
                {
                    Parent.Accept(null, Constants.CommentAutoAcceptForChildren);
                    Parent.AutoAcceptAncestors();
                }
            }
        }

        public virtual void RefuseAccept()
        {
            CurrentStatus = Status.RefuseAccept;
            setLatestUpdate();
            HasAccepted = false;
            addHistory(Accepter, Constants.DescriptionRefuseAccept);
        }

        public virtual void Dissent()
        {
            CurrentStatus = Status.Dissent;
            setLatestUpdate();
            addHistory(Owner, Constants.DescriptionDissent);
        }

        public virtual void Expire()
        {

        }

        public virtual void Comment(User author, User addressee, string comment)
        {
            addHistory(author, Constants.DescriptionComment, comment);

            Message message = new Message
            {
                Task = this,
                Project = Project,
                Addresser = author,
                Addressee = addressee,
                Content = comment
            };
            message.Send();
        }

        #endregion

        public virtual void BeginEdit(User user)
        {
            EditingBy = user;
        }

        public virtual void EndEdit()
        {
            EditingBy = null;
        }

        #region Structure

        public virtual void AddChild(Task task)
        {
            task.Parent = this;
            Children = Children ?? new List<Task>();
            if (Children.Count > 0)
            {
                task.Sequence = Children.Max(x => x.Sequence) + 1;
            }
            else
            {
                task.Sequence = 1;
            }
            Children.Add(task);
        }

        public virtual void RemoveChild(Task task)
        {
            Children.Remove(task);
            task.Parent = null;
        }

        public virtual void ChangeParent(Task newParent)
        {
            if (Parent == null)
            {
                newParent.AddChild(this);
            }
            else if (Parent != newParent)
            {
                Parent.RemoveChild(this);
                newParent.AddChild(this);
            }
        }

        public virtual void RemoveParent()
        {
            if (Parent != null)
            {
                Parent.RemoveChild(this);
            }
        }

        #endregion

        public virtual void ChangePublisher(User operater, User successor)
        {
            Publisher = successor;
            setLatestUpdate();
            addHistory(operater, Constants.CHANGE_PUBLISHER);
        }

        public virtual void ChangeOwner(User operater, User successor)
        {
            Owner = successor;
            setLatestUpdate();
            addHistory(operater, Constants.CHANGE_OWNER);
        }

        public virtual void ChangeAccepter(User operater, User successor)
        {
            Accepter = successor;
            setLatestUpdate();
            addHistory(operater, Constants.CHANGE_ACCEPTER);
        }

        public virtual void AddHistory(User executor, string comment)
        {
            addHistory(executor, Constants.DescriptionComment, comment);
        }

        public virtual Task GetPrevious()
        {
            if (Parent == null)
            {
                return null;
            }
            else
            {
                return Parent.Children
                    .Where(t => t.Sequence < Sequence)
                    .OrderBy(t => t.Sequence)
                    .LastOrDefault();
            }
        }

        public virtual Task GetNext()
        {
            if (Parent == null)
            {
                return null;
            }
            else
            {
                return Parent.Children
                    .Where(t => t.Sequence > Sequence)
                    .OrderBy(t => t.Sequence)
                    .FirstOrDefault();
            }
        }

        public virtual bool IsLastInBrothers(Status status)
        {
            if (Parent == null
                || Parent.Children.IsNullOrEmpty()
                || Parent.Children.Count <= 1)
            {
                return false;
            }

            // must use Id == Id, can't use == this
            return Parent.Children.Count(t => t.CurrentStatus == status) + 1 == Parent.Children.Count
               && Parent.Children.Single((t => t.CurrentStatus != status)).Id == Id;
        }

        public virtual bool IsOffspring(Task task)
        {
            return task.IsAncestor(this);
        }

        public virtual bool IsAncestor(Task task)
        {
            //one task can not be his own ancestor 
            if (task == this)
            {
                return false;
            }
            while (task != null)
            {
                if (task == this)
                {
                    return true;
                }
                task = task.Parent;
            }
            return false;
        }

        #region private methods

        private void setLatestUpdate()
        {
            LatestUpdateTime = SystemTime.Now();
        }

        private void workPeriodBegin()
        {
            WorkPeriods = WorkPeriods ?? new List<WorkPeriod>();
            WorkPeriod period = new WorkPeriod
            {
                Task = this,
                Begin = SystemTime.Now()
            };
            WorkPeriods.Add(period);
        }

        private void workPeriodEnd()
        {
            var result = from w in WorkPeriods
                         orderby w.Id descending
                         select w;
            WorkPeriod period = result.FirstOrDefault();

            period.End = SystemTime.Now();
            period.Duration = (period.End - period.Begin).ToMinutes();

            ActualWorkPeriod = WorkPeriods.Sum(w => w.Duration);
        }

        private void addHistory(User executor, string description)
        {
            addHistory(executor, description, null);
        }

        private void addHistory(User executor, string description, string comment)
        {
            Histroy = Histroy ?? new List<HistoryItem>();
            Histroy.Add(new HistoryItem
            {
                Executor = executor,
                Description = description,
                Comment = comment,
                Belong = this,
                Status = CurrentStatus
            });
        }

        #endregion

        #endregion
    }
}
