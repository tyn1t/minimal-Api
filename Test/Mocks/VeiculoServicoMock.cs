using System.Data.Common;
using MininalApi.Dominio.DTO;
using MininalApi.Dominio.Entidades;
using MininalApi.Dominio.Interfaces;

namespace Test.Mocks
{
    public class VeiculoServicoMock : IVeiculoSevirco
    {
        private static List<Veiculo> veiculos = new List<Veiculo>(){
            new Veiculo{
                 Id = 1,
                 Nome = "Cruze",
                 Marca = "Chevrolet",
                 Ano = 2024,
            }
        };
        public void Apagar(Veiculo veiculo)
        {
            veiculos.Remove(veiculo);
        }

        public void Atualizar(Veiculo veiculo)
        {
            var veiculosAtualizar = veiculos.FirstOrDefault(v => v.Id == veiculo.Id );
            if (veiculosAtualizar != null)
            {
               veiculosAtualizar.Nome = veiculo.Nome;
               veiculosAtualizar.Marca = veiculo.Marca;
               veiculosAtualizar.Ano = veiculo.Ano;
               
            }
            
        }

        public Veiculo? BuscaPorId(int id)
        {
           return veiculos.Find(v => v.Id == id);
        }

        public void Incluir(Veiculo veiculo)
        {
            veiculo.Id = veiculos.Count;
            veiculos.Add(veiculo);
        }

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            return veiculos;
        }
    }
}