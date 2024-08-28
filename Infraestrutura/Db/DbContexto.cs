using Microsoft.EntityFrameworkCore;
using MininalApi.Dominio.Entidades;

namespace MiminalApi.Infraestrutura.Db
{
    public class DbContexto : DbContext
    { 
        private readonly IConfiguration _configuracaoAppSettings;
        public DbContexto(IConfiguration configuracao_AppSettings)
        {
            _configuracaoAppSettings = configuracao_AppSettings;
        }

        public DbSet<Administrador> Administradores { get; set; } = default!;
        
        public DbSet<Veiculo> Veiculos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>().HasData(
                new Administrador {
                    Id = 1,
                    Email = "admin@teste.com",
                    Senha = "123456",
                    Perfil = "Adm"
                }
            );
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {   if(!optionsBuilder.IsConfigured)
            {
            var stringConexao = _configuracaoAppSettings.GetConnectionString("Mysql")?.ToString();
            if(!string.IsNullOrEmpty(stringConexao))
            {
                optionsBuilder.UseMySql(
                    stringConexao,
                    ServerVersion.AutoDetect(stringConexao)
                );
            }
            }
        }
    }
}