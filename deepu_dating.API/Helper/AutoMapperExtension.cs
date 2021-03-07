using System.Linq;
using System.Reflection.Emit;
using AutoMapper;
using deepu_dating.API.DTO;
using deepu_dating.API.Models;

namespace deepu_dating.API.Helper
{
    public class AutoMapperExtension: Profile
    {
        public AutoMapperExtension()
        {
            CreateMap<userData,MemberDto>()
            .ForMember(dest =>dest.Url,opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x=>x.IsMain).Url));

            CreateMap<Photo,PhotoDto>();

            CreateMap<updateDto,userData>();

            CreateMap<RegisterDto,userData>();

            CreateMap<Message,MessageDto>()
            .ForMember(dest => dest.SenderPhotoUrl,
             opt => opt.MapFrom(src => src.Sender.Photos.SingleOrDefault(c =>c.IsMain).Url))
             .ForMember(dest => dest.RecipientPhotoUrl,
             opt => opt.MapFrom(src => src.Recipient.Photos.SingleOrDefault(c =>c.IsMain).Url));;
        }
        
    }
}