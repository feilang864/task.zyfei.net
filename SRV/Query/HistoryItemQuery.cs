using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;

namespace FFLTask.SRV.Query
{
    public class HistoryItemQuery
    {
        private IQueryable<HistoryItem> _querySource;
        public HistoryItemQuery(IQueryable<HistoryItem> querySource)
        {
            _querySource = querySource;
        }

        public IQueryable<HistoryItem> Get(IQueryable<Task> tasks, Status status)
        {
            return _querySource
                .Where(h => tasks.Contains(h.Belong)
                    && h.Status == status);
        }
    }
}
