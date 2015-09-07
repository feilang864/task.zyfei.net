using System;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;

namespace FFLTask.SRV.Query
{
    public static class MessageQuery
    {
        public static IQueryable<Message> Project(this IQueryable<Message> source, int? projectId)
        {
            if (projectId.HasValue)
            {
                return source.Where(m => m.Project.Id == projectId);
            }
            return source;
        }

        public static IQueryable<Message> Sort(this IQueryable<Message> source, MessageSort sortedBy, bool des)
        {
            if (sortedBy == MessageSort.PublishTime)
            {
                source = source.OrderBy(m => m.CreateTime);
            }
            else if (sortedBy == MessageSort.ReadTime)
            {
                source = source.OrderBy(m => m.ReadTime);
            }

            if (des)
            {
                source = source.AsEnumerable().Reverse().AsQueryable();
            }
            return source;
        }

        public static IQueryable<Message> To(this IQueryable<Message> source, int? userId)
        {
            if (userId.HasValue)
            {
                return source.Where(m => m.Addressee.Id == userId);
            }
            return source;
        }

        public static IQueryable<Message> From(this IQueryable<Message> source, int? userId)
        {
            if (userId.HasValue)
            {
                return source.Where(m => m.Addresser.Id == userId);
            }
            return source;
        }

        public static IQueryable<Message> CanShow(this IQueryable<Message> source, MessageFor hideMessageFor)
        {
            if (hideMessageFor == MessageFor.Addresser)
            {
                return source.Where(m => !m.HideForAddresser);
            }
            else if (hideMessageFor == MessageFor.Addressee)
            {
                return source.Where(m => !m.HideForAddressee);
            }
            else
            {
                throw new ArgumentException("unknown filter for hidding messages.");
            }
        }

        public static IQueryable<Message> HasRead(this IQueryable<Message> source)
        {
            return source.Where(m => m.ReadTime.HasValue);
        }

        public static IQueryable<Message> NotRead(this IQueryable<Message> source)
        {
            return source.Where(m => !m.ReadTime.HasValue);
        }

        #region Combined

        public static IQueryable<Message> Get(this IQueryable<Message> source,
            int? addresserId,
            int? addresseeId,
            int? projectId,
            MessageFor hideMessageFor)
        {
            return source
                .From(addresserId)
                .To(addresseeId)
                .Project(projectId)
                .CanShow(hideMessageFor);
        }

        #endregion

    }
}
