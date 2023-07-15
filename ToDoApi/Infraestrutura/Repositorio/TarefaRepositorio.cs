using Dominio;
using Infraestrutura.Contexto;
using Infraestrutura.Paginacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorio
{
    public class TarefaRepositorio : Repositorio<Tarefa>, ITarefaRepositorio
    {

        public TarefaRepositorio(AppDbContext contexto):base(contexto) 
        {
            
        }
        public IEnumerable<Tarefa> GetTarefaEspeficica(Expression<Func<Tarefa, bool>> predicate)
        {
            return _context.Set<Tarefa>().Where(predicate);
        }

        public PaginacaoList<Tarefa> GetTarefas(TarefaParametros tarefaparametro)
        {
            return PaginacaoList<Tarefa>.ToListPagina(Get().OrderBy(on => on.Titulo),
                                                      tarefaparametro.NumeroPagina,tarefaparametro.QuantidadeDeRegistros);
        }
    }
}
