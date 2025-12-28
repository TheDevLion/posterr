using AutoMapper;
using PosterrBackend.Application.DTOs;
using PosterrBackend.Application.RequestModels;
using PosterrBackend.Domain.Entities;

namespace PosterrBackend.Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDTO>();
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<Post, PostDTO>();
        CreateMap<Post, PostDTO>().ReverseMap();
        CreateMap<Repost, RepostDTO>();
        CreateMap<Repost, RepostDTO>().ReverseMap();
        CreateMap<Record, RecordDTO>();
        CreateMap<Record, RecordDTO>().ReverseMap();
        CreateMap<Post, Record>();
        CreateMap<Post, Record>().ReverseMap();
        CreateMap<PostDTO, RecordDTO>();
        CreateMap<PostDTO, RecordDTO>().ReverseMap();
        CreateMap<CreateNewPostRequest, PostDTO>();
        CreateMap<CreateNewRepostRequest, RepostDTO>();
    }
}
