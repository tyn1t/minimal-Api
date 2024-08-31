using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiminalApi.Infraestrutura.Db;
using MininalApi.Dominio.Entidades;
using MininalApi.Dominio.ModelViews;
using MininalApi.Dominio.Servicos;

namespace Test.Domian.Entidades;

[TestClass]
public class AdministradorServicoTest 
{

    private DbContexto CriarContextoDeTeste()
    {
        var AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var path = Path.GetFullPath(Path.Combine(AssemblyPath ?? "", "..",  "..", ".."));

        var builder =new ConfigurationBuilder()
            .SetBasePath(path ?? Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
        var Configuration = builder.Build();    

        return new DbContexto(Configuration);


    }

    [TestMethod]
    public void TestrGetPropried()
    {


        // Arrenge
        var context = CriarContextoDeTeste();
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE Administradores");

        var adm =new Administrador();
            adm.Email = "teste@teste.com";
            adm.Senha = "teste";
            adm.Perfil = "adm";

        var administradorServico = new AdministradorServico(context);

        //  Act
        administradorServico.Incluir(adm);

        // Assert
        Assert.AreEqual(1, administradorServico.Todos(1).Count);
        Assert.AreEqual( "teste@teste.com", adm.Email);
        Assert.AreEqual("teste", adm.Senha);
        Assert.AreEqual("adm", adm.Perfil );

       
    }

}