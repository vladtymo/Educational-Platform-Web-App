using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using SS_EDUP.Core.DTO_s;
using SS_EDUP.Core.Entities;
using SS_EDUP.Core.ViewModels.User;
using SS_EDUP.Infrastructure.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_EDUP.Core.AutoMapper
{
    public class AutoMapperUserProfile : Profile
    {
        public AutoMapperUserProfile()
        {
            CreateMap<AppUser, RegisterUserVM>();
            CreateMap<RegisterUserVM, AppUser>();
            CreateMap<AppUser, AppUserDto>();
            CreateMap<AppUserDto, AppUser>().ForMember(dst => dst.UserName, act => act.MapFrom(src => src.Email));
            CreateMap<UserProfileVM, AppUser>();
            CreateMap<AppUser, UserProfileVM>();
            CreateMap<AppUser, UpdateProfileVM>();
            CreateMap<UpdateProfileVM, AppUser>().ForMember(dst => dst.UserName, act => act.MapFrom(src => src.Email));
            CreateMap<AllUsersVM, AppUser>().ReverseMap();
            CreateMap<AppUser, EditUserVM>().ReverseMap();
            
            

        }
    }
}
