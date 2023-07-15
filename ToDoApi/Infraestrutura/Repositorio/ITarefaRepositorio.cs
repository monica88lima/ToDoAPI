using Dominio;
using Infraestrutura.Paginacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorio
{
    public interface ITarefaRepositorio:IRepositorio<Tarefa>
    {
        IEnumerable<Tarefa> GetTarefaEspeficica(Expression<Func<Tarefa, bool>> predicate);

        PaginacaoList<Tarefa> GetTarefas(TarefaParametros tarefaparametro);

    }
}
