using AlugaSe.WebAPI.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddDomain();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebApi(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AlugaSe API v1");
    c.RoutePrefix = "swagger";
});

app.UseCors("AlugaSeCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
