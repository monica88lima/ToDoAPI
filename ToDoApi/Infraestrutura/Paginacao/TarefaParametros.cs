using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Paginacao
{
    public class TarefaParametros
    {
        const int tamanhoMaximo = 50;
        public int NumeroPagina { get; set; } = 1;
        private int _tamanhoPagina = 10;

        public int TamanhoPg { get
            {
                return _tamanhoPagina;
            }
            set
            {
                _tamanhoPagina = (value > tamanhoMaximo)? tamanhoMaximo : value;
            }
        }

    }
}
