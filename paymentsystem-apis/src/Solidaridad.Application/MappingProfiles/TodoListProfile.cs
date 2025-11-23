using AutoMapper;
using Solidaridad.Application.Models.TodoList;
using Solidaridad.Core.Entities;

namespace Solidaridad.Application.MappingProfiles;

public class TodoListProfile : Profile
{
    public TodoListProfile()
    {
        CreateMap<CreateTodoListModel, TodoList>();

        CreateMap<TodoList, TodoListResponseModel>();
    }
}
