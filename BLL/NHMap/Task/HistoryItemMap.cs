
namespace FFLTask.BLL.NHMap.Task
{
    class HistoryItemMap : EntityMap<BLL.Entity.HistoryItem>
    {
        public HistoryItemMap()
        {
            References(m => m.Belong).ForeignKey("FK_HistroyItem_Belong");
            References(m => m.Executor).ForeignKey("FK_HistoryItem_Executor");
            Map(m => m.Description);
            Map(m => m.Comment).Length(Const.Length_Text);
            Map(x => x.Status);
        }
    }
}
