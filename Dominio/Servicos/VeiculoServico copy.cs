using MiminalApi.Infraestrutura.Db;
using MininalApi.Dominio.Interfaces;
using MininalApi.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace MininalApi.Dominio.Servicos
{
    public class VeiculoServico(DbContexto _db) : IVeiculoSevirco
    {    
        private readonly DbContexto _contexto = _db;

        public void Apagar(Veiculo veiculo)
        {
             _contexto.Veiculos.Remove(veiculo);
            _contexto.SaveChanges();
        }

        public void Atualizar(Veiculo veiculo)
        {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
        }

        public Veiculo? BuscaPorId(int id)
        {
            return _contexto.Veiculos.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Incluir(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
        }       
       

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            var query = _contexto.Veiculos.AsQueryable();
            if(!string.IsNullOrEmpty(nome))
            {
                query = query.Where(v => EF.Functions.Like(v.Nome.ToLower(), $"%{nome}%"));
            }
            int itensPorPagina = 10;

            if(pagina != null)
            {
                query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);
            }
            return query.ToList();
            
        }

        
    }
}