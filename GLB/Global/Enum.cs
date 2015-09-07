using System.ComponentModel;

namespace FFLTask.GLB.Global.Enum
{
    public enum Token
    {
        Founder,
        Assinger,
        Owner
    }

    public enum PrivilegeInProject
    {
        [Description("您还没有加入该项目")]
        NotJoin = 0,
        [Description("您还没有该项目的发布权限")]
        NotPublish = 1,
        [Description("您有该项目的发布权限")]
        HasPublish = 2
    }

    public enum SearchBy
    {
        [Description("编号")]
        Id,

        [Description("名称")]
        Name
    }

    public enum Status
    {
        [Description("发布")]
        Publish = 1,

        [Description("分配")]
        Assign = 2,

        [Description("承接")]
        Own = 3,

        [Description("开始")]
        BeginWork = 31,

        [Description("暂停")]
        Pause = 32,

        [Description("完成")]
        Complete = 33,

        [Description("放弃")]
        Quit = 34,

        [Description("更新")]
        Update = 36,

        [Description("取消")]
        Remove = 37,

        [Description("协商")]
        Doubt = 38,

        [Description("验收")]
        Accept = 41,

        [Description("拒收")]
        RefuseAccept = 42,

        [Description("异议")]
        Dissent = 43
    }

    public enum RedirectPage
    {
        Current,
        Other,
        Child,
        Brother,
        Parent,
        List,
        Next,
        Previous,
        Close
    }

    public enum NodeType
    {
        [Description("根")]
        Root,
        [Description("枝")]
        Branch,
        [Description("叶")]
        Leaf
    }

    public enum TaskProcess
    {
        [Description("发布")]
        Publish,
        [Description("分配/承接")]
        Assign,
        [Description("进度")]
        InProcess,
        [Description("验收")]
        Accept,
        [Description("留言")]
        Comment,
        [Description("取消")]
        Remove,
        [Description("恢复")]
        Resume
    }

    public enum MessageSort
    {
        [Description("发布时间")]
        PublishTime,
        [Description("读取时间")]
        ReadTime
    }

    public enum MessageFor
    {
        Addresser,
        Addressee
    }

    public enum MessageMark
    {
        Read,
        Delete
    }

    public enum TokenForTaskUser
    {
        [Description("发布人")]
        Publisher,
        [Description("承接人")]
        Owner,
        [Description("验收人")]
        Accepter
    }

    public enum TaskDifficulty
    {
        [Description("很容易")]
        Easiest = 1,
        [Description("容易")]
        Easy = 2,
        [Description("一般")]
        Common = 3,
        [Description("困难")]
        Hard = 4,
        [Description("很困难")]
        Hardest = 5,
    }

    public enum TaskQuality
    {
        [Description("合格")]
        Qualified = 1,
        [Description("良好")]
        Good = 2,
        [Description("完美")]
        Perfect = 3
    }

    public enum TaskPriority
    {
        [Description("最高")]
        Highest = 5,
        [Description("高")]
        High = 4,
        [Description("中")]
        Common = 3,
        [Description("低")]
        Low = 2,
        [Description("最低")]
        Lowest = 1
    }

    public enum ListColumn
    {
        [Description("编号")]
        ID,
        [Description("种类")]
        NodeType,
        [Description("标题")]
        Title,
        [Description("发布人")]
        Publisher,
        [Description("优先级")]
        Priority,
        [Description("难度")]
        Difficulty,
        [Description("预计耗时")]
        ExpectedWorkPeriod,
        [Description("实际耗时")]
        ActualWorkPeriod,
        [Description("超期耗时")]
        OverDue,
        [Description("预计完成日期")]
        ExpectedComplete,
        [Description("实际完成日期")]
        ActualComplete,
        [Description("逾期")]
        Delay,
        [Description("状态")]
        Status,
        [Description("承接人")]
        Owner,
        [Description("验收人")]
        Accepter,
        [Description("发布时间")]
        PublishTime,
        [Description("分配时间")]
        AssignTime,
        [Description("承接时间")]
        OwnTime,
        [Description("最后更新时间")]
        LastUpdateTime,
        [Description("质量")]
        Quality,
    }

    public enum Role
    {
        [Description("发布人")]
        Publisher,
        [Description("承接人")]
        Owner,
        [Description("验收人")]
        Accepter
    }
}
