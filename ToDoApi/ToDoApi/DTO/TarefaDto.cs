using System.ComponentModel.DataAnnotations;

namespace ToDoApi.DTO
{
    public class TarefaDto
    {
        [Required]
        public int TarefaId { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Titulo { get; set; }

        [StringLength(250)]
        public string? Descricao { get; set; }
               

        [DataType(DataType.Date)]
        public DateTime? DataInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataFim { get; set; }

    }
}
