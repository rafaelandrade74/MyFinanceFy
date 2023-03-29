using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyFinanceFy.Models;
using System.Reflection.Emit;

namespace MyFinanceFy.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Painel>? Paineis { get; set; }
        public DbSet<PainelUsuario>? PainelUsuarios { get; set; }
        public DbSet<PainelDados>? PainelDados { get; set; }
        public DbSet<Categoria>? Categorias { get; set; }
        public DbSet<PainelDadosRelModel>? PainelDadosView { get; set; }
        



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>().Property(u => u.UserName).HasMaxLength(128);
            modelBuilder.Entity<IdentityRole>().Property(r => r.Name).HasMaxLength(128);
            modelBuilder.Entity<IdentityRole>().Property(r => r.NormalizedName).HasMaxLength(128);
            modelBuilder.Entity<Usuario>().Property(r => r.NormalizedUserName).HasMaxLength(128);
            modelBuilder.Entity<Usuario>(b =>
            {
                b.ToTable("AspNetUsers");
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
            modelBuilder.Entity<Categoria>().HasData(
                new Categoria
                {
                    Id = "02c8b76c-8921-45f7-a4e2-0e9b5c535422",
                    Nome = "Cartão de credito",
                    Cor = "Red"
                },
                new Categoria
                {
                    Id = "737bb14e-2275-48b6-b95c-3a72db4d8e6a",
                    Nome = "Conta corrente",
                    Cor = "Blue"
                },
                new Categoria
                {
                    Id =  "24054a77-5697-4756-a8d2-afadb219d4eb",
                    Cor = "na",
                    Nome = "Energia"
                },
                new Categoria
                {
                    Id = "fe8f0229-bdc3-49f2-9ecc-37f06ca62aa2",
                    Cor = "na",
                    Nome = "Agua"
                },
                new Categoria
                {
                    Id = "9526c063-297b-4982-beb6-5b6bdc4609b1",
                    Cor = "na",
                    Nome = "Condominio"
                },
                new Categoria
                {
                    Id =  "c9eb1dd7-891f-442d-bf7a-ed2f789caf1d",
                    Cor = "na",
                    Nome = "Internet"
                },
                new Categoria
                {
                    Id = "0dd5b867-0fd0-47c8-95f7-7f8805c6039c",
                    Cor = "na",
                    Nome = "Plano celular"
                },
                new Categoria
                {
                    Id = "261e7f7c-f813-46a4-b069-c7f64770c341",
                    Cor = "na",
                    Nome = "Emprestimo"
                },
                new Categoria
                {
                    Id = "c83ddd39-e9e4-4e88-b9a4-6de3dd7f16a0",
                    Cor = "na",
                    Nome = "Financiamento"
                },
                new Categoria
                {
                    Id = "98217e13-ca66-4b0c-9eb7-a60b8e322423",
                    Cor = "na",
                    Nome = "Consorcio"
                },
                new Categoria
                {
                    Id = "2e9b6b14-3f82-430f-ae3e-aea068bdedaa",
                    Cor = "na",
                    Nome = "Curso"
                },
                new Categoria
                {
                    Id = "f1dac129-8821-4120-b8e7-b62423061dea",
                    Cor = "na",
                    Nome = "Mercado"
                },
                new Categoria
                {
                    Id = "48d06bf7-120e-47e5-8a88-3bf137edd84f",
                    Cor = "na",
                    Nome = "Petshop"
                },
                new Categoria
                {
                    Id = "ca5e5e44-c95e-4e27-a368-ea809da85772",
                    Cor = "na",
                    Nome = "Farmacia"
                },
                new Categoria
                {
                    Id = "a56a0fa5-e2fa-48fe-a0d9-83867522d272", 
                    Cor = "na",
                    Nome = "Veterinario"
                },
                new Categoria
                {
                    Id = "848c9d42-f376-43c7-94d7-873427f38b20",
                    Cor = "na",
                    Nome = "Salário"
                },
                new Categoria
                {
                    Id = "bc36edab-dc0c-42a7-bd7d-2d95ad945733",
                    Cor = "na",
                    Nome = "13° Salário - 1° Parcela"
                },
                new Categoria
                {
                    Id = "08748f77-7ed5-4b54-b024-741411fc5b7d", 
                    Cor = "na",
                    Nome = "13° Salário - 2° Parcela"
                },
                new Categoria
                {
                    Id = "e1ac1652-ff93-4eef-adca-7b6dd14b1923",
                    Cor = "na",
                    Nome = "Poupança"
                },
                new Categoria
                {
                    Id = "1c10d76e-c74f-4e69-b880-8e19b380726a",
                    Cor = "na",
                    Nome = "Faxina"
                },
                new Categoria
                {
                    Id = "747b8940-c591-42e4-93a2-4ab07310acf2",
                    Cor = "na",
                    Nome = "Salão de beleza/Barbearia"
                },
                new Categoria
                {
                    Id = "b6ff3574-e54b-454d-af92-e0dfde834e0c", 
                    Cor = "na",
                    Nome = "Bonus"
                },
                new Categoria
                {
                    Id = "46f071d6-1a13-4cbd-960f-632105c4120c",
                    Cor = "na",
                    Nome = "Outros"
                }
            );
            modelBuilder.Entity<PainelDadosRelModel>().ToView("view_fin_painel_dados_rel").HasNoKey();
            
        }
    }
}