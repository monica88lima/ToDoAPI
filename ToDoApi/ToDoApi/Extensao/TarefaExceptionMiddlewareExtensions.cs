using Dominio;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ToDoApi.Extensao
{
    public static class TarefaExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new DetalheErros()
                        {
                            StatusCode = context.Response.StatusCode,
                            Mensagem= contextFeature.Error.Message,
                            Trace = contextFeature.Error.StackTrace

                        }.ToString());
                    }

                });
            });
        }
    }
}
