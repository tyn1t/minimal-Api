using MininalApi.Dominio.Enuns;

namespace MininalApi.Dominio.DTO
{
    public class AdministradorDTO
    {
        public string Email { get; set;} = default!;
        public string Senha { get; set; } = default!;
        
        public Perfil Perfil { get; set; } = default!;
    }
}