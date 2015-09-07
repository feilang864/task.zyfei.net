using AutoMapper;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Team;

namespace FFLTask.SRV.ProdService.ViewModelMap
{
    internal class TeamMap
    {
        internal static void Init()
        {
            Mapper.CreateMap<Task, TransferItemModel>()
                .ForMember(t=>t.Selected, opt=>opt.Ignore());
        }
    }
}
