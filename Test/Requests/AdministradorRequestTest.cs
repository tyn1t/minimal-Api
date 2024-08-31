using System.Net;
using System.Text;
using System.Text.Json;
using MininalApi.Dominio.DTO;
using MininalApi.Dominio.ModelViews;
using Test.Helpers;

namespace Test.Domian.Entidades;

[TestClass]
public class AdministradoresRequetTest
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

        var content  = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "Application/json");

        // Act 
        var response = await Setup.client.PostAsync("/administradores/login", content ?? null);
    
        // Assert
        
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadAsByteArrayAsync();
        
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        
        
        Assert.IsNotNull(admLogado?.Email);
        Assert.IsNotNull(admLogado?.Perfil);
        Assert.IsNotNull(admLogado?.Token);

         Console.WriteLine(admLogado.Token);
    }
}