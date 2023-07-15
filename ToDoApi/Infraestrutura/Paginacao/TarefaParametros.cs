using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Paginacao
{
    public class TarefaParametros
    {
        const int quantidadeMaxRegistros = 50;
        public int NumeroPagina { get; set; } = 1;
        private int _quantidadeRegistros = 10;

        public int QuantidadeDeRegistros { get
            {
                return _quantidadeRegistros;
            }
            set
            {
                _quantidadeRegistros = (value > quantidadeMaxRegistros) ? quantidadeMaxRegistros : value;
            }
        }

    }
}
