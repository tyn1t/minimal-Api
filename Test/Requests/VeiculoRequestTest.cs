using System.Net;
using System.Text;
using System.Text.Json;
using MininalApi.Dominio.DTO;
using MininalApi.Dominio.ModelViews;
using Test.Helpers;

namespace Test.Domian.Entidades;

[TestClass]
public class VeiculoRequetTest
{

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }
    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task  TestrGetSetPropriedades()
    {
        // Arrenge
        var loginDTO = new LoginDTO{
            Email = "adm@teste.com",
            Senha  = "123456"

        };

        var veiculoDTO = new VeiculoDTO{
            Nome = "carro1",
            Marca = "Carro+",
            Ano = 2020
        };

        var content  = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");
        Console.WriteLine(content);

        // Act 
        var response = await Setup.client.PostAsync("/administradores/login", content ?? null);
    
        // Assert
        
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var GetTokenresult = await response.Content.ReadAsByteArrayAsync();
        var GetToken = JsonSerializer.Deserialize<AdministradorLogado>(GetTokenresult, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        var apiKey = GetToken?.Token;
        Setup.client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var contentTEst  = new StringContent(JsonSerializer.Serialize(veiculoDTO), Encoding.UTF8, "Application/json");
        var responseVeiculo = await Setup.client.PostAsync("/veiculos", contentTEst ?? null);
      
        var result = await responseVeiculo.Content.ReadAsByteArrayAsync();
        
        var veiculocadastrado = JsonSerializer.Deserialize<VeiculoDTO>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        
        
        Assert.IsNotNull(veiculocadastrado?.Nome);
        Assert.IsNotNull(veiculocadastrado?.Marca);
        Assert.IsNotNull(veiculocadastrado?.Ano);

    }
}