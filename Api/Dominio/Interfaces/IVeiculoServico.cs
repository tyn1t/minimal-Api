using MininalApi.Dominio.Entidades;
using MininalApi.Dominio.Servicos;

namespace MininalApi.Dominio.Interfaces
{
    public interface IVeiculoSevirco
    {
        List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null);
        Veiculo? BuscaPorId(int id);

        void Incluir(Veiculo veiculo);
        
        void Atualizar(Veiculo veiculo);
        void Apagar(Veiculo veiculo);
    }
}