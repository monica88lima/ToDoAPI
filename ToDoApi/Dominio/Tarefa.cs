using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Tarefa
    {
        [Required]
        
        public int TarefaId { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Titulo { get; set; }

        [StringLength(250)]
        public string? Descricao { get; set; }

       
        [DataType(DataType.Date)]
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? DataInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataFim { get; set; }


    }
}
