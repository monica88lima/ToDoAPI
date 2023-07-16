using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Paginacao
{
    public class PaginacaoList<T>:List<T>
    {
        public int PaginaAtual { get; private set; }
        public int TotalDePaginas { get; private set; }
        public int QuantidadeDeRegistros{ get; private set; }
        public int ContarRegistros { get; private set; }

        public bool PaginaAnterior => PaginaAtual > 1;
        public bool ProximaPagina => PaginaAtual < TotalDePaginas;

        public PaginacaoList(List<T> items, int qtdTotal, int pgAtual, int qtdRegistros)
        {
            ContarRegistros = qtdTotal;
            PaginaAtual = pgAtual;
            QuantidadeDeRegistros = qtdRegistros;
            TotalDePaginas = (int) Math.Ceiling(qtdTotal / (double) qtdRegistros);

            AddRange(items);
        }

        public static PaginacaoList<T> ToListPagina(IQueryable<T> fonte, int numeroPagina, int qtdRegistro)
        {
            var quantidade = fonte.Count();
            var items = fonte.Skip((numeroPagina - 1) * qtdRegistro).Take(qtdRegistro).ToList();
            return new PaginacaoList<T>(items, quantidade, numeroPagina, qtdRegistro);
        }
    }
}
