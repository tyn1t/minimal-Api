
using MininalApi.Dominio.Entidades;

namespace Test.Domian.Entidades;

[TestClass]
public class VeiculoTest
{
    [TestMethod]
    public void TestGetPropried()
    {
        // arrange 
        var veiculo = new Veiculo();
        // act

        veiculo.Id = 10;
        veiculo.Nome = "Cruze";
        veiculo.Marca = "Chevrolet";
        veiculo.Ano = 2005;
        // assert
        Assert.AreEqual("Cruze",  veiculo.Nome);
        Assert.AreEqual("Chevrolet",  veiculo.Marca);
        Assert.AreEqual(2005,veiculo.Ano);
    }
}