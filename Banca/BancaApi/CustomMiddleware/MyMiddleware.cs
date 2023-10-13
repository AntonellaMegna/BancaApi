namespace BancaApi.CustomMiddleware
{
    public class MyMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await context.Response.WriteAsync("primo middleware");
          //  await next(context);// passo al componente  successivo e gli passo l'oggetto HttpContext modificato
        }
    }
}
