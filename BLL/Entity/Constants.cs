
namespace FFLTask.BLL.Entity
{
    //TODO: all constants should be renamed as upper case and contacted with "_", see task: 2397
    public class Constants
    {
        public const string CommentAutoOwn = "因子任务开始而自动承接";
        public const string CommentAutoBegin = "因子任务开始而自动开始工作";
        public const string CommentAutoComplete = "因子任务全部完成而自动完成";
        public const string CommentAutoAcceptForOwnerIsAccepter = "因承接人和验收人为同一人而自动验收合格";
        public const string CommentAutoAcceptForChildren = "因子任务全部合格而自动验收合格";

        public const string CHANGE_PUBLISHER = "更改发布人";
        public const string CHANGE_OWNER = "更改承接人";
        public const string CHANGE_ACCEPTER = "更改验收人";

        public const string DescriptionPublish = "发布任务";
        public const string DescriptionUpdate = "更新任务";
        public const string DescriptionAssign = "分配任务";
        public const string DescriptionCancelAssign = "取消分配任务";
        public const string DescriptionOwn = "主动承接任务";
        public const string DescriptionBeginWork = "开始工作";
        public const string DescriptionRefuseOwn = "拒绝承接任务";
        public const string DescriptionPause = "暂停任务";
        public const string DescriptionDoubt = "发布质疑";
        public const string DescriptionComplete = "完成任务";
        public const string DescriptionGiveup = "放弃任务";
        public const string DescriptionRefuseAccept = "拒绝验收";
        public const string DescriptionAccept = "验收任务";
        public const string DescriptionDissent = "质疑拒收";
        public const string DescriptionRemove = "取消任务";
        public const string DescriptionResume = "恢复任务";
        public const string DescriptionComment = "留言";
    }
}
