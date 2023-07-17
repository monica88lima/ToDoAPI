using Infraestrutura.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorio
{
    public class UnitOfWork:IUnitOfWork
    {
        private TarefaRepositorio _tarefaRepo;
        public AppDbContext _context;

        public UnitOfWork(AppDbContext contexto)
        {
            _context = contexto;
        }
        public ITarefaRepositorio TarefaRepositorio
        {
            get { return _tarefaRepo = _tarefaRepo ?? new TarefaRepositorio(_context); }
        }
     
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}
