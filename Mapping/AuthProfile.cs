using AutoMapper;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Mapping;

public class AuthProfile : Profile
{
  public AuthProfile()
  {
    CreateMap<User, UserDto>()
    .ForMember(dest => dest.Token, opt => opt.Ignore());
  }
}