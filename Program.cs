using MiminalApi.Dominio.DTOs;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Olá pessoal! tudo certo ok");

app.MapPost("/login", (LoginDTO loginDTO) => {
    if(loginDTO.Email == "admin@teste.com" && loginDTO.Senha == "123456")
       return Results.Ok("Login secesso");
    else 
        return Results.Unauthorized();
});

app.Run();
