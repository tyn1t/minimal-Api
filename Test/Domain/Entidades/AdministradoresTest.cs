using MininalApi.Dominio.Entidades;

namespace Test.Domian.Entidades;

[TestClass]
public class AdministradoresTest
{
    [TestMethod]
    public void TestrGetPropried()
    {
        // Arrenge
        var adm =new Administrador();
        
        // Act 
        adm.Id = 1;
        adm.Email = "teste@teste.com";
        adm.Senha = "teste";
        adm.Perfil = "adm";
        // Assert
        Assert.AreEqual(1, adm.Id);
        Assert.AreEqual( "teste@teste.com", adm.Email);
        Assert.AreEqual("teste", adm.Senha);
        Assert.AreEqual("adm", adm.Perfil );
    }
}