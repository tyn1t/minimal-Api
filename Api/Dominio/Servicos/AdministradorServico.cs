
using MininalApi.Dominio.DTO;
using MiminalApi.Infraestrutura.Db;
using MininalApi.Dominio.Interfaces;
using MininalApi.Dominio.Entidades;

namespace MininalApi.Dominio.Servicos
{
    public class AdministradorServico(DbContexto _db) : IAdministradorServico
    {    
        private readonly DbContexto  _contexto = _db;
  
        public Administrador Incluir(Administrador administrador)
        {
            _contexto.Administradores.Add(administrador);
            _contexto.SaveChanges();

            return administrador;
        }
         public Administrador? BuscaPorId(int id)
        {
            return _contexto.Administradores.Where(v => v.Id == id).FirstOrDefault();
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            var adm = _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
            return adm;
        
        }

        public List<Administrador> Todos(int? pagina)
        {
            var query = _contexto.Administradores.AsQueryable();
            
            int itensPorPagina = 10;
            if(pagina != null)
                query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);
        
            return query.ToList();
        }
    }
}