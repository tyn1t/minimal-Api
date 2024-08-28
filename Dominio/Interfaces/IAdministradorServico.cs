using MininalApi.Dominio.DTO;
using   MininalApi.Dominio.Entidades;

namespace MininalApi.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        Administrador? Login(LoginDTO loginDTO);
        Administrador Incluir(Administrador administrador);
        Administrador? BuscaPorId(int id);
        List<Administrador> Todos(int? pagina);

    } 
}