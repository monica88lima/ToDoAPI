using AutoMapper;
using Dominio;

namespace ToDoApi.DTO.Mappings
{
    public class MapeandoPerfil:Profile
    {
        public MapeandoPerfil()
        {
            CreateMap<Tarefa, TarefaDto>().ReverseMap();
        }
        
        
    }
}
