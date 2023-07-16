using AutoMapper;
using Dominio;
using Infraestrutura.Contexto;
using Infraestrutura.Paginacao;
using Infraestrutura.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ToDoApi.DTO;
using ToDoApi.Filtros;

namespace ToDoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]

    public class TarefaController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public TarefaController(IUnitOfWork repositorio, IMapper mapper, IConfiguration configuration)
        {
            _uof = repositorio;
            _configuration = configuration;
            _mapper = mapper;
        }
       /// <summary>
       /// Exibe a consuta ordenada por titulo, com todos os registros de Tarefas, podendo ser paginada 
       /// </summary>
       /// <param name="tarefaparametro"></param> Informações sobre a quantidade de páginas
       /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(404)]
        [ServiceFilter(typeof(TarefaFiltros))]
        public ActionResult<IEnumerable<TarefaDto>> Consulta([FromQuery] TarefaParametros tarefaparametro)
        {
            var tarefa = _uof.TarefaRepositorio.GetTarefas(tarefaparametro);
            var metadata = new
            {
                tarefa.ContarRegistros,
                tarefa.QuantidadeDeRegistros,
                tarefa.PaginaAtual,
                tarefa.TotalDePaginas,
                tarefa.ProximaPagina,
                tarefa.PaginaAnterior

            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
            var tarefaDto = _mapper.Map<List<TarefaDto>>(tarefa);
            if (tarefa.Count == 0)
            {
                return NotFound("Não há tarefa Cadastrada!");
            }
            return (tarefaDto);

        }
        /// <summary>
        /// Obtém tarefa por ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Um objeto tarefa</returns>
        [HttpGet("{id}", Name = "ObterTarefa")]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<TarefaDto> Consulta(int id)
        {
            try
            {
                var tarefa = _uof.TarefaRepositorio.GetById(x => x.TarefaId == id);

                if (tarefa is null)
                {
                    return NotFound($"Tarefa com id: {id} Não localizada!");
                }
                var tarefaDto = _mapper.Map<TarefaDto>(tarefa);
                return Ok(tarefaDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        /// <summary>
        /// Obtém uma lista de Tarefas que contém a palavra pesquisada no título
        /// </summary>
        /// <param name="titulo">Palavras que podem estar no titulo da tarefa</param>
        /// <returns>Uma lista do objeto Tarefa</returns>
        [HttpGet("titulo")]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<TarefaDto>> ConsultaPorNome(string titulo)
        {
            try
            {
                if(string.IsNullOrEmpty(titulo)|| titulo.Length < 3)
                {
                    return BadRequest($"A pesquisa deve contém no minimo 3 caracteres.");
                }
                var tarefa = _uof.TarefaRepositorio.GetTarefaEspeficica(x => x.Titulo.Contains(titulo)).ToList();
                if (tarefa.Count == 0)
                {
                    return NotFound($"Nenhuma tarefa localizada!Com este titulo: {titulo}");
                }
                var tarefaDto = _mapper.Map<List<TarefaDto>>(tarefa);
                return Ok(tarefaDto);

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        /// <summary>
        /// Obtém uma lista de tarefas que foram criadas em uma determinada data
        /// </summary>
        /// <param name="data"> data no formato yyyy-MM-dd</param>
        /// <returns>Uma lista com objetos que tenha a data de criação informada no parametro</returns>
        [HttpGet("data")]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<TarefaDto>> ConsultaPorDataCriacao(DateTime data)
        {
            try
            {
                var tarefa = _uof.TarefaRepositorio.GetTarefaEspeficica(x => x.DataCriacao == data).ToList();
                if (tarefa.Count == 0)
                {
                    return NotFound($"Nenhuma tarefa localizada nesta data: {data}!");
                }
                var tarefaDto = _mapper.Map<List<TarefaDto>>(tarefa);
                return Ok(tarefaDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        /// <summary>
        /// Obtém uma lista do objeto Tarefa, que contenham a data de inicio e fim, dentre as informadas nos parametros
        /// </summary>
        /// <param name="datainicial">data que foi registrada como inicio da tarefa, formato da data yyyy-MM-dd</param>
        /// <param name="datafinal">data que foi registrada como o dia fim da tarefa,formato da data yyyy-MM-dd</param>
        /// <returns>Retorna de lista de objetos com as pertence ao range indicado</returns>
        [HttpGet("periodo")]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<IEnumerable<TarefaDto>> ConsultaPorPeriodo(DateTime datainicial, DateTime datafinal)
        {
            try
            {
                if(datainicial > datafinal)
                {
                    return BadRequest($"Período inválido, a data inicial({datainicial}) deve ser igual ou maior que a data do término{(datafinal)} da atividade");
                }
                var tarefa = _uof.TarefaRepositorio.GetTarefaEspeficica(x => x.DataInicio >= datainicial && x.DataFim <= datafinal).ToList();
                if (tarefa.Count == 0)
                {
                    return NotFound($"Nenhuma tarefa localizada!Para este periodo: {datainicial} /{datafinal}");
                }
                var tarefaDto = _mapper.Map<List<TarefaDto>>(tarefa);
                return Ok(tarefaDto);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        /// <summary>
        /// Inclui uma nova tarefa
        /// </summary>
        /// <param name="tarefaDto"> </param>
        /// <returns>A tarefa conforme cadastrada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TarefaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Salvar(TarefaDto tarefaDto)
        {
            try
            {
                var tarefa = _mapper.Map<Tarefa>(tarefaDto);
                if (tarefa is null)
                {
                    return BadRequest();
                }
                _uof.TarefaRepositorio.Add(tarefa);
                _uof.Commit();

                var tarefaDTO = _mapper.Map<TarefaDto>(tarefa);

                return new CreatedAtRouteResult("ObterTarefa",
                    new { id = tarefa.TarefaId }, tarefaDTO);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }
        /// <summary>
        /// Altera informações de uma tarefa já cadastrada
        /// </summary>
        /// <param name="id"> Id da Tarefa que deseja alterar</param>
        /// <param name="tarefaDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult Alterar(int id, TarefaDto tarefaDto)
        {
            try
            {
                if (id != tarefaDto.TarefaId)
                {
                    return BadRequest();
                }
                var tarefa = _mapper.Map<Tarefa>(tarefaDto);
                _uof.TarefaRepositorio.Update(tarefa);
                _uof.Commit();

                return NoContent();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }
        /// <summary>
        /// Remove a tarefa informada pelo ID
        /// </summary>
        /// <param name="id">Id da tarefa de deseja apagar</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Deletar(int id)
        {
            try
            {
                var tarefa = await  _uof.TarefaRepositorio.GetById(x => x.TarefaId == id);
                if (tarefa == null)
                {
                    return NotFound("Tarefa não localizada!");
                }
                _uof.TarefaRepositorio.Delete(tarefa);
                _uof.Commit();

                return NoContent();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }


        }
        [HttpGet("/Autor")]
        [ServiceFilter(typeof(TarefaFiltros))]
        public string Autor()
        {

            var autor = _configuration["Autor"];
            var BD = _configuration["Banco de Dados"];
            var detalhes = _configuration["Repositorio Git"];
            return $"Autor: {autor},Banco de Dados:{BD},Link Projeto Git:{detalhes}";
        }

    }
}
