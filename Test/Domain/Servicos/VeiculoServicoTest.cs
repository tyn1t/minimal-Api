using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiminalApi.Infraestrutura.Db;
using MininalApi.Dominio.Entidades;
using MininalApi.Dominio.Servicos;

namespace Test.Domian.Entidades;

[TestClass]
public class VeiculoServicoTest 
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
        context.Database.ExecuteSqlRaw("TRUNCATE TABLE veiculos");

        var veiculo =new Veiculo();
            veiculo.Nome = "Carroteste";
            veiculo.Marca = "carro-";
            veiculo.Ano = 2002;

        var veiculoServico = new VeiculoServico(context);

        //  Act
        veiculoServico.Incluir(veiculo);

        // Assert

        Assert.AreEqual(1, veiculoServico.Todos(1).Count);
        Assert.AreEqual( "Carroteste", veiculo.Nome);
        Assert.AreEqual("carro-", veiculo.Marca);
        Assert.AreEqual(2002, veiculo.Ano);

       
    }

}