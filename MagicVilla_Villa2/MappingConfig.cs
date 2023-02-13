using AutoMapper;
using MagicVilla_Villa2.Model;
using MagicVilla_Villa2.Model.DTO;

namespace MagicVilla_Villa2
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            CreateMap<Villa, VillaDTO>().ReverseMap();
            CreateMap<Villa, VillaCreateDTO>().ReverseMap();
            CreateMap<Villa,VillaUpdateDTO>().ReverseMap();

            CreateMap<VillaNumber, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaCreateNumberDTO>().ReverseMap();
            CreateMap<VillaNumber, VillaUpdateNumberDTO>().ReverseMap();
        }
    }
}
