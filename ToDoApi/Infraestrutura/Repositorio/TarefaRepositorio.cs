using Dominio;
using Infraestrutura.Contexto;
using Infraestrutura.Paginacao;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<Tarefa>>GetTarefaEspeficica(Expression<Func<Tarefa, bool>> predicate)
        {
            return await _context.Set<Tarefa>().Where(predicate).ToListAsync();
        }

        public async Task<PaginacaoList<Tarefa>>GetTarefas(TarefaParametros tarefaparametro)
        {
            return await PaginacaoList<Tarefa>.ToListPagina(Get().OrderBy(on => on.Titulo),
                                                      tarefaparametro.NumeroPagina,tarefaparametro.QuantidadeDeRegistros);
        }
    }
}
