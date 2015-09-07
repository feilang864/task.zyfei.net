using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskModel = FFLTask.SRV.ViewModel.Task;
using Entity = FFLTask.BLL.Entity;

namespace FFLTask.SRV.ProdService.ViewModelMap
{
    internal class TaskMap
    {
        internal static void Init()
        {
            Mapper.CreateMap<Entity.Task, TaskModel.LiteItemModel>()
                //TODO: these are caculated?
                .ForMember(t => t.NodeType, opt => opt.Ignore())
                .ForMember(t => t.HasChild, opt => opt.Ignore())
                //TODO: need align the name
                .ForMember(t => t.Virtual, opt => opt.Ignore())
                //TODO: should use Custom value resolvers:
                //https://github.com/AutoMapper/AutoMapper/wiki/Custom-value-resolvers
                .ForMember(t => t.CurrentStatus, opt => opt.Ignore())
                ;
        }
    }
}
