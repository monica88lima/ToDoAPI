using Microsoft.AspNetCore.Mvc.Filters;

namespace ToDoApi.Filtros
{
    public class TarefaFiltros : IActionFilter
    {
        private readonly ILogger<TarefaFiltros> _logger;
        public TarefaFiltros(ILogger<TarefaFiltros> logger)
        {

            _logger = logger;

        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Executando: OnActionExecuted");
            _logger.LogInformation($"Data:{DateTime.Now.ToShortDateString()}");
            _logger.LogInformation($"Hora:{DateTime.Now.ToShortTimeString()}");
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Executando: OnActionExecuting");
            _logger.LogInformation($"Data:{DateTime.Now.ToShortDateString()}");
            _logger.LogInformation($"Hora:{DateTime.Now.ToShortTimeString()}");
        }
    }
}
