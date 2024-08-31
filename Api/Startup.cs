using  MininalApi;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiminalApi.Infraestrutura.Db;
using MininalApi.Dominio.DTO;
using MininalApi.Dominio.Entidades;
using MininalApi.Dominio.Interfaces;
using MininalApi.Dominio.ModelViews;
using MininalApi.Dominio.Servicos;




public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        key = Configuration?.GetSection("Jwt")?.ToString() ?? "";

    }

    private string key;
    public IConfiguration Configuration { get; set;} = default!;

    public void ConfigureServices(IServiceCollection services)
    {

        services.AddAuthentication(option => {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
            option.TokenValidationParameters = new TokenValidationParameters{
                ValidateLifetime =true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateIssuer = false,
                ValidateAudience = false,
            });

        services.AddAuthorization();

        services.AddScoped<IAdministradorServico, AdministradorServico>();
        services.AddScoped<IVeiculoSevirco, VeiculoServico>();


        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => {
            options.AddSecurityDefinition("Bearer", new  OpenApiSecurityScheme{
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                In = ParameterLocation.Header,
                Description = "Insira o token Aqui"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement{
                {
                new OpenApiSecurityScheme{
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
                }
            });
        });

        services.AddDbContext<DbContexto>(options => {
            options.UseMySql(
                Configuration.GetConnectionString("Mysql"),
                ServerVersion.AutoDetect(Configuration.GetConnectionString("Mysql"))
            );
        });

    }
    


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseCors();
        

        app.UseEndpoints(endpoints => {
            #region Home
            endpoints.MapGet("/", () => Results.Json(new Home())).AllowAnonymous().WithTags("Home");
            #endregion 

            #region  Administradores
            string GeratTokenJwt(Administrador administrador){
                if(string.IsNullOrEmpty(key)) return string.Empty;

                
                var Securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(Securitykey, SecurityAlgorithms.HmacSha256);
                
                var claims = new List<Claim>()
                {
                    new("Email", administrador.Email),
                    new("Perfil", administrador.Perfil),
                    new(ClaimTypes.Role, administrador.Perfil)

                };
                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            endpoints.MapPost("/administradores/login", ([FromBody] LoginDTO loginDTO, IAdministradorServico administradorServico) => {
                var adm = administradorServico.Login(loginDTO);
                if(adm != null)
                {
                string token = GeratTokenJwt(adm);
                return Results.Ok(new AdministradorLogado
                {
                    Email = adm.Email,
                    Perfil = adm.Perfil,
                    Token = token
                });
                }
                else 
                    return Results.Unauthorized();
            }).AllowAnonymous().WithTags("Adminstradores");

            endpoints.MapGet("/administradores", ([FromQuery] int? pagina, IAdministradorServico administradorServico) => {
                var adms = new List<AdministradorModelViews>();
                var administradores = administradorServico.Todos(pagina);
                foreach(var adm in administradores)
                {
                    adms.Add(new AdministradorModelViews{
                        Id = adm.Id,
                        Email = adm.Email,
                        Perfil = adm.Perfil
                    });
                }
                return Results.Ok(adms);
            }).RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
            .WithTags("Adminstradores");

            endpoints.MapGet("/Adminstradores/{id}", ([FromRoute] int id, IAdministradorServico administradorServico) => {
                var administrador = administradorServico.BuscaPorId(id);
                if(administrador == null) return Results.NotFound();

                return Results.Ok(new AdministradorModelViews{
                        Id = administrador.Id,
                        Email = administrador.Email,
                        Perfil = administrador.Perfil
                    });
            }).RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
            .WithTags("Adminstradores");


            endpoints.MapPost("/administradores", ([FromBody]  AdministradorDTO administradorDTO, IAdministradorServico administradorServico) => {
                
                var validacao = new ErrosDeValidacao();
                
                if(string.IsNullOrEmpty(administradorDTO.Email))
                    validacao.Mensagens.Add("Email não pode ser vazio");
                if(string.IsNullOrEmpty(administradorDTO.Senha))
                    validacao.Mensagens.Add("senha não pode ser vazio");
                if(string.IsNullOrEmpty(administradorDTO.Perfil.ToString()))
                    validacao.Mensagens.Add("Perfil não pode ser vazio");

                if(validacao.Mensagens.Count() > 0)
                    return Results.BadRequest(validacao);
                

                var administrador = new Administrador{
                    Email = administradorDTO.Email,
                    Senha = administradorDTO.Senha,
                    Perfil = administradorDTO.Perfil.ToString(),
                };
                    administradorServico.Incluir(administrador);


                return Results.Created($"/administradores/{administrador}",new AdministradorModelViews{
                        Id = administrador.Id,
                        Email = administrador.Email,
                        Perfil = administrador.Perfil
                    });
                
            }).RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
            .WithTags("Adminstradores");
            #endregion

            #region Veculos

            static ErrosDeValidacao validaDTO(VeiculoDTO veiculoDTO)
            {   
                var validacao = new ErrosDeValidacao();

                if(string.IsNullOrEmpty(veiculoDTO.Nome))
                    validacao.Mensagens.Add("O nome não poder ser vazio");
                if(string.IsNullOrEmpty(veiculoDTO.Marca))
                    validacao.Mensagens.Add("O Marca não poder ser vazio");
                
                if(veiculoDTO.Ano < 1950)
                    validacao.Mensagens.Add("Veiculo muito antigo, aceite somete anos superiore a 1950");
                
                return validacao;
            }

            endpoints.MapPost("/veiculos", ([FromBody] VeiculoDTO veiculoDTO, IVeiculoSevirco veiculoServico) => {

                var validacao = validaDTO(veiculoDTO);
                if(validacao.Mensagens.Count() > 0)
                    return Results.BadRequest(validacao);
                var veiculo = new Veiculo{
                    Nome = veiculoDTO.Nome,
                    Marca = veiculoDTO.Marca,
                    Ano = veiculoDTO.Ano
                };
                veiculoServico.Incluir(veiculo);
                return Results.Created($"/veiculo/{veiculo.Id}", veiculo);
            }).RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
            .WithTags("Veiculo");


            endpoints.MapGet("/veiculos", ([FromQuery] int? pagina, IVeiculoSevirco veiculoSevirco) => {
                var veiculos = veiculoSevirco.Todos(pagina);
                return Results.Ok(veiculos);
            }).RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
            .WithTags("Veiculo");


            endpoints.MapGet("/veiculos/{id}", ([FromRoute] int id, IVeiculoSevirco veiculoSevirco) => {
                var veiculo = veiculoSevirco.BuscaPorId(id);
                if(veiculo == null) return Results.NotFound();
                return Results.Ok(veiculo);
            }).RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
            .WithTags("Veiculo");


            endpoints.MapPut("/veiculos/{id}", ([FromRoute] int id, VeiculoDTO veiculoDTO, IVeiculoSevirco veiculoServico) => {
                
                var veiculo = veiculoServico.BuscaPorId(id);
                if(veiculo == null) return Results.NotFound();
                
                veiculo.Nome = veiculoDTO.Nome;
                veiculo.Marca = veiculoDTO.Marca;
                veiculo.Ano = veiculoDTO.Ano;

                veiculoServico.Atualizar(veiculo);
                return Results.Ok(veiculo);

            }).RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
            .WithTags("Veiculo");

            endpoints.MapDelete("/veiculos/{id}", ([FromRoute] int id, IVeiculoSevirco veiculoServico) => {
                
                var veiculo = veiculoServico.BuscaPorId(id);
                if(veiculo == null) return Results.NotFound();

                veiculoServico.Apagar(veiculo);

                return Results.NoContent();
            }).RequireAuthorization()
            .RequireAuthorization(new AuthorizeAttribute {Roles = "Adm"})
            .WithTags("Veiculo");
            #endregion
        });
    }
}