using FFLTask.GLB.Global.Enum;

namespace FFLTask.SRV.ServiceInterface
{
    public interface IMessageService
    {
        FFLTask.SRV.ViewModel.Message.ToMeModel GetToMe(int userId, MessageSort sortedBy, bool des, FFLTask.SRV.ViewModel.Shared.PagerModel pager, int? projectId = null, int? addresserId = null);
        FFLTask.SRV.ViewModel.Message.FromMeModel GetFromMe(int userId, MessageSort sortedBy, bool des, FFLTask.SRV.ViewModel.Shared.PagerModel pager, int? projectId = null, int? addresseeId = null);

        int GetToMeCount(int userId, int? projectId, int? addresserId);
        int GetFromMeCount(int userId, int? projectId, int? addresseeId);

        void DeleteForAddresser(int messageId);
        void DeleteForAddressee(int messageId);
        void Read(int messageId);
    }
}
