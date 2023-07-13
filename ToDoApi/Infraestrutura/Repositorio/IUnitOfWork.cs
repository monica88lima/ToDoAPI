using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Repositorio
{
    public interface IUnitOfWork
    {
        ITarefaRepositorio TarefaRepositorio { get; }

        void Commit();
    }
}
