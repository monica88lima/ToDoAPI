using AutoMapper;
using FluentAssertions;
using Infraestrutura.Contexto;
using Infraestrutura.Paginacao;
using Infraestrutura.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using ToDoApi.Controllers;
using ToDoApi.DTO;
using ToDoApi.DTO.Mappings;
using Xunit;

namespace ToDoApiXunitTest
{
    public class TarefasUnitTesteController
    {
        private readonly IUnitOfWork repos;
        private static readonly IConfiguration conf;
        private readonly IMapper mapper;
       
        public static DbContextOptions<AppDbContext> dbContextOptions { get; }


        public static string connectionString = "Server=127.0.0.1;Database=TODO;User Id=sa;Password=N3wS3nh@;TrustServerCertificate=true";
        static TarefasUnitTesteController()
        {

            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
            conf = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        }
        public TarefasUnitTesteController()
        {
            var config = new MapperConfiguration(Cfg =>
            {
                Cfg.AddProfile(new MapeandoPerfil());
            });
            mapper = config.CreateMapper();
            var context = new AppDbContext(dbContextOptions);
            repos = new UnitOfWork(context);


        }

        /// <summary>
        /// Teste do método post com parametros válidos
        /// </summary>
        [Fact]
        public async Task Salva_PostTarefa()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);

            var trf = new TarefaDto()
            {
                Titulo = "Teste Unitário",
                Descricao = "Salvando Teste",
                DataInicio = new DateTime(2023, 7, 15),
                DataFim = new DateTime(2023, 7, 15)
            };

            //Act
            var data =await controller.Salvar(trf);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(data);

        }

        /// <summary>
        /// Teste método put com informações válida
        /// </summary>
        [Fact]
        public async Task Alterar_PutTarefa()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            int id = 1011;
            var trf = new TarefaDto()
            {
                TarefaId = 1011,
                Titulo = " Teste Unitario ",
                Descricao = "Alterando",
                DataInicio = new DateTime(2023, 7, 15),
                DataFim = new DateTime(2023, 7, 15)
            };

            //Act
            var data = await controller.Alterar(id, trf);

            //Assert
            Assert.IsType<NoContentResult>(data);

        }
        /// <summary>
        /// Teste de Consulta de todas as tarefas
        /// </summary>
        [Fact]
        public async Task Consulta_TarefaID_RetornaTudo()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            TarefaParametros parametros = new();
            
            //Act
            var data = await controller.Consulta(parametros);

            //Assert
            Assert.IsType<List<TarefaDto>>(data.Value);
        }
       

        /// <summary>
        /// Teste de Get enviando um ID válido
        /// </summary>
        [Fact]
        public async Task Consulta_TarefaID_RetornaId()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            int id = 1010;

            //Act
            var data = await controller.Consulta(id);

            //Assert
            Assert.IsType<OkObjectResult>(data.Result);
        }
        /// <summary>
        /// Teste de consulta por ID com ID inválido, inexistente
        /// </summary>
        [Fact]
        public async Task Consulta_TarefaID_RetornoNotFound()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            int id = 99999;

            //Act
            var data = await controller.Consulta(id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(data.Result);
        }
        /// <summary>
        /// Teste de data_valida
        /// </summary>
        [Fact]
        public async Task Consulta_DataCricao_Valida()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            DateTime dataC = new DateTime(2023, 07, 09);

            //Act
            var data = await controller.ConsultaPorDataCriacao(dataC);

            //Assert
            Assert.IsType<OkObjectResult>(data.Result);
        }
        /// <summary>
        /// Teste com uma data futura inexistente
        /// </summary>
        [Fact]
        public async Task Consulta_DataCricao_semRegistroNotFound()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            DateTime dataInvalida = new DateTime(2025, 09, 12);

            //Act
            var data = await controller.ConsultaPorDataCriacao(dataInvalida);

            //Assert
            Assert.IsType<NotFoundObjectResult>(data.Result);
        }
        /// <summary>
        /// Teste de consulta para validar se o retorno estava de acordo com os registros do BD.
        /// </summary>
        [Fact]
        public async Task Consulta_Geral_ValidarRetornosConsultas()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            TarefaParametros parametros = new() ;

            //Act
            var data = await controller.Consulta(parametros);

            //Assert
            Assert.IsType<List<TarefaDto>>(data.Value);
            var trf = data.Value.Should().BeAssignableTo<List<TarefaDto>>().Subject;

            Assert.Equal("Almoçar", trf[1].Titulo);
            Assert.Equal("Ingerir alimentos as 12h", trf[1].Descricao);

            Assert.Equal(" Teste Unitario ", trf[0].Titulo);
            Assert.Equal("Alterando", trf[0].Descricao);
        }
        /// <summary>
        /// Teste de pesquisa por lista, esperado uma lista cm diversas tarefas
        /// </summary>
        [Fact]
        public async Task Consulta_TarefaTitulo_RetornaListaTarefas()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            string titulo = "faz";

            //Act
            var data = await controller.ConsultaPorNome(titulo);

            //Assert
            Assert.IsType<OkObjectResult>(data.Result);
        }
        /// <summary>
        /// Teste com pesquisas invalidas, para verificar o fluxo ( palavra vazia, menor que 3 letras.
        /// </summary>
        [Fact]
        public async Task Consulta_TarefaTitulo_RetornaBadRequest()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            string titulo2Char = "xi";
            string tituloVazio = "";

            //Act
            var data = await controller.ConsultaPorNome(titulo2Char);
            var data2 = await controller.ConsultaPorNome(tituloVazio);

            //Assert
            Assert.IsType<BadRequestObjectResult>(data.Result);
            Assert.IsType<BadRequestObjectResult>(data2.Result);
        }
        /// <summary>
        /// Titulo de tarefa inexistente
        /// </summary>
        [Fact]
        public async Task Consulta_TarefaTitulo_RetornaNotFound()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            string titulo = "Brigadeiro";

            //Act
            var data = await controller.ConsultaPorNome(titulo);

            //Assert
            Assert.IsType<NotFoundObjectResult>(data.Result);
            
        }
        /// <summary>
        /// Teste por range com entradas válidas
        /// </summary>
        [Fact]
        public async Task Consulta_ConsultaPorPeriodo_RetornaLista()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            DateTime dataI= new DateTime(2023, 07, 01);
            DateTime dataF = new DateTime(2023, 07, 09);

            //Act
            var data = await controller.ConsultaPorPeriodo(dataI, dataF);

            //Assert
            Assert.IsType<OkObjectResult>(data.Result);

        }
        /// <summary>
        /// Teste de consulta por Range, considerando a data Inicial maior que a Data Final da tarefa cadastrada
        /// </summary>
        [Fact]
        public async Task Consulta_ConsultaPorPeriodo_RetornaBadRequest()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            DateTime dataI = new DateTime(2023, 07, 31);
            DateTime dataF = new DateTime(2023, 07, 09);

            //Act
            var data = await controller.ConsultaPorPeriodo(dataI, dataF);

            //Assert
            Assert.IsType<BadRequestObjectResult>(data.Result);

        }
        /// <summary>
        /// Teste de consulta por rande de data, considerando um periodo sem tarefa cadastrada
        /// </summary>
        [Fact]
        public async Task Consulta_ConsultaPorPeriodo_RetornaNotFound()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            DateTime dataI = new DateTime(2045, 07, 31);
            DateTime dataF = new DateTime(2072, 07, 09);

            //Act
            var data = await controller.ConsultaPorPeriodo(dataI, dataF);

            //Assert
            Assert.IsType<NotFoundObjectResult>(data.Result);

        }
              
        /// <summary>
        /// Teste para validar o retorno de um atentativa de alteração com im ID divergente da Tarefa enviada.
        /// </summary>
        [Fact]
        public async Task Alterar_PutTarefa_RetornoBadRequest()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            int id = 1010;
            var trf = new TarefaDto()
            {
                TarefaId = 1011,
                Titulo = "Teste Unitario",
                Descricao = "Limpar dentro e fora",
                DataInicio = new DateTime(2023, 7, 15),
                DataFim = new DateTime(2023, 7, 15)
            };

            //Act
            var data = await controller.Alterar(id, trf);

            //Assert
            Assert.IsType<BadRequestResult>(data);

        }
       /// <summary>
       /// Teste para deletar com dados válidos
       /// </summary>
        [Fact]
        public async Task Deletar_RetornoOk()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            int id = 2009;
           

            //Act
            var data = await controller.Deletar(id);

            //Assert
            Assert.IsType<NoContentResult>(data);

        }
        /// <summary>
        /// Teste para validar o envio de um ID inexistente, retorno esperado notfound
        /// </summary>
        [Fact]
        public async Task Deletar_RetornoNotFound()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);
            int id = 3;


            //Act
            var data = await controller.Deletar(id);

            //Assert
            Assert.IsType<NotFoundObjectResult>(data);

        }
        [Fact]
        public void autor()
        {
            //Arrange
            var controller = new TarefaController(repos, mapper, conf);



            //Act
            var data = controller.Autor();


            //Assert

            Assert.Equal("Autor: Monica,Banco de Dados:SqlServer,Link Projeto Git:lnk", data);
        }
                

    }  
}
