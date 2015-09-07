using System.Collections.Generic;
using FFLTask.GLB.Global.Enum;

namespace FFLTask.SRV.ServiceInterface
{
    public interface ITaskService
    {
        bool CanAutoAccept(int taskId);
        bool CanAutoComplete(int taskId);
        bool ParentIsOffspring(int childId, int parentId);

        int Create(FFLTask.SRV.ViewModel.Task.NewModel model, int creatorId);
        int GetCount(FFLTask.SRV.ViewModel.Task.ListModel model);

        string GetTitle(int taskId);

        void Accept(FFLTask.SRV.ViewModel.Task.EditModel model);
        void Assign(FFLTask.SRV.ViewModel.Task.EditModel model);
        void Comment(FFLTask.SRV.ViewModel.Task.EditModel model);
        void Expire(int taskId);
        void Handle(FFLTask.SRV.ViewModel.Task.EditModel model);
        void Own(int taskId, int ownerId);
        void Refuse(FFLTask.SRV.ViewModel.Task.EditModel model);
        void Remove(int taskId, string comment);
        void Resume(int taskId, string comment);
        void UpdateTaskProperty(FFLTask.SRV.ViewModel.Task.EditModel model);
        void UpdateTaskSequence(IList<int> taskIds);
        void UploadFiles(IList<string> files, int taskId, int uploaderId);

        FFLTask.SRV.ViewModel.Task._SumModel GetSum(FFLTask.SRV.ViewModel.Task.ListModel model);
        FFLTask.SRV.ViewModel.Task.EditModel GetEdit(int taskId, FFLTask.SRV.ViewModel.Account.UserModel currentUser);
        FFLTask.SRV.ViewModel.Task.FullItemModel Get(int taskId);
        FFLTask.SRV.ViewModel.Task.LiteItemModel GetLite(int taskId);
        FFLTask.SRV.ViewModel.Task.SequenceModel GetSequence(int taskId);
        FFLTask.SRV.ViewModel.Task.TaskRelationModel GetRelation(int taskId);

        IList<FFLTask.SRV.ViewModel.Task.FullItemModel> Get(string sort, bool des, FFLTask.SRV.ViewModel.Task.ListModel model);
        IList<FFLTask.SRV.ViewModel.Task.LiteItemModel> GetStartWith(string title);
        IList<FFLTask.SRV.ViewModel.Task.TaskHistoryItemModel> GetHistory(int taskId);
    }
}
