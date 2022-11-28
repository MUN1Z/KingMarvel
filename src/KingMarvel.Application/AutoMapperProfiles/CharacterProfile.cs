using AutoMapper;
using KingMarvel.Application.ViewModels.Response;
using KingMarvel.Domain.Entities;

namespace KingMarvel.Application.AutoMapperProfiles
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<CharacterRequestViewModel, Character>();
            CreateMap<CharacterResponseViewModel, Character>();

            CreateMap<Character, CharacterRequestViewModel>();
            CreateMap<Character, CharacterResponseViewModel>();

            CreateMap<CharacterRequestViewModel, CharacterResponseViewModel>();
            CreateMap<CharacterResponseViewModel, CharacterRequestViewModel>();
        }
    }
}
