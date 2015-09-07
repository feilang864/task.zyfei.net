
using FFLTask.GLB.Global.Enum;
namespace FFLTask.BLL.NHMap.Task
{
    class TaskMap : EntityMap<BLL.Entity.Task>
    {
        public TaskMap()
        {
            References(m => m.Publisher).ForeignKey("FK_Task_Publisher");
            References(m => m.EditingBy).ForeignKey("FK_Task_EditingBy");
            References(m => m.Accepter).ForeignKey("FK_Task_Accepter");

            Map(m => m.IsVirtual);
            Map(m => m.Title).Index("IX_Task_Title");
            Map(m => m.Body).Length(Const.Length_Text);
            References(m => m.Project).ForeignKey("FK_Task_Project");

            Map(m => m.CurrentStatus).Index("IX_CurrentStatus");

            Map(m => m.Sequence);
            Map(m => m.Difficulty).CustomType<TaskDifficulty?>().Index("IX_Difficulty");
            Map(m => m.Priority).CustomType<TaskPriority?>().Index("IX_Priority");
            Map(m => m.ExpectWorkPeriod).Index("IX_ExpectWorkPeriod");
            Map(m => m.ExpectCompleteTime).Index("IX_ExpectComplete");

            Map(m => m.AssignTime).Index("IX_Assign");

            Map(m => m.OwnTime).Index("IX_Own");
            References(m => m.Owner).ForeignKey("FK_Task_Owner");
            References(m => m.Parent).ForeignKey("FK_Task_Parent");
            HasMany(m => m.Children).KeyColumn("Parent_Id").Inverse();

            Map(m => m.ActualCompleteTime).Index("IX_ActualComplete");
            HasMany(m => m.WorkPeriods).KeyColumn("Task_id").Inverse();
            Map(m => m.ActualWorkPeriod).Index("IX_ActualWorkPeriod");
            Map(m => m.OverDue).Index("IX_OverDue");
            Map(m => m.Delay).Index("IX_Delay");

            Map(m => m.AcceptTime).Index("IX_Accept");
            Map(m => m.HasAccepted).Index("IX_HasAccepted");
            Map(m => m.Quality).CustomType<TaskQuality?>().Index("IX_Quality");

            Map(m => m.LatestUpdateTime).Index("IX_LatestUpdate");

            HasMany(m => m.Histroy).Inverse().KeyColumn("Belong_id");
            HasMany(m => m.Attachments).Inverse().KeyColumn("Task_id"); ;
        }
    }
}
