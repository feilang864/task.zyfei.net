using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserModel = FFLTask.SRV.ViewModel.User;
using Entity = FFLTask.BLL.Entity;


namespace FFLTask.SRV.ProdService.ViewModelMap
{
    internal class UserMap
    {
        internal static void Init()
        {
            Mapper.CreateMap<Entity.Profile, UserModel.ProfileModel>();
            Mapper.CreateMap<UserModel.ProfileModel, Entity.Profile>();
        }
    }
}
