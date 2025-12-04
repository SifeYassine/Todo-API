using AutoMapper;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Mapping;

public class TodoProfile : Profile
{
    public TodoProfile()
    {
        CreateMap<Todo, TodoDto>();
        CreateMap<CreateTodoDto, Todo>();
        CreateMap<UpdateTodoDto, Todo>()
        .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
        .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
        .ForMember(dest => dest.IsComplete, opt => opt.Condition(src => src.IsComplete.HasValue));
    }
}