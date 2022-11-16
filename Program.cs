//criar projeto:
//	dotnet new webabi -minimal -o NomeDoProjeto
//entrar na pasta:
//	cd NomeDoProjeto
//adicionar entity framework no console:
//	dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 6.0
//	dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 6.0
//	dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0
//incluir namespace do entity framework:
//	using Microsoft.EntityFrameworkCore;
//antes de rodar o dotnet run pela primeira vez, rodar os seguintes comandos para iniciar a base de dados:
//	dotnet ef migrations add InitialCreate
//	dotnet ef database update


// GUSTAVO LUIS GUEDES DA ROSA
// JOÃO PEDRO MESSIAS

using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;

namespace Trabalho
{
	class Cliente
    {
    	public int id { get; set; }
		public string? nome { get; set; }
    	public string? email { get; set; }
		public string? telefone { get; set; }
		public string? cpf { get; set; }
		public DateTime? dataNascimento { get; set; }
    }

	class Academia
		{
			public int id { get; set; }
			public string? nomePlano { get; set; }
			public double? valorPlano { get; set; }
            public string? tipoPlano { get; set; }
		}	

	class Assinatura
		{
			public int id { get; set; }
			public int? idCliente { get; set; }
            public string? nomeCliente { get; set; }
			public int? idPlano { get; set; }

            public string? nomePlano { get; set; }
			public DateTime? dataInicio { get; set; }
			public DateTime? dataExpirar { get; set; }
		}

	class BaseAcademia : DbContext
	{
		public BaseAcademia(DbContextOptions options) : base(options)
		{
		}
		
    
		public DbSet<Cliente> Cliente { get; set; } = null!;
    	public DbSet<Academia> Academia { get; set; } = null!;
		public DbSet<Assinatura> Assinatura { get; set; } = null!;
	}
	
	class Program
	{
		static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			
			var connectionString = builder.Configuration.GetConnectionString("Academia") ?? "Data Source=Academia.db";
			builder.Services.AddSqlite<BaseAcademia>(connectionString);

            builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
			var app = builder.Build();
			app.UseCors();
            
			//listar todos os clientes
			app.MapGet("/clientes", (BaseAcademia baseAcademia) => {
				return baseAcademia.Cliente.ToList();
			});
			
			//listar cliente especificop por id
			app.MapGet("/cliente/{id}", (BaseAcademia baseAcademia, int id) => {
				return baseAcademia.Cliente.Find(id);
			});

			//cadastrar cliente
            // nome, cpf, dataNascimento, email, telefone
			app.MapPost("/cliente/cadastrar", (BaseAcademia baseAcademia, Cliente cliente) =>
			{
                int minimunAge = 18;
                if (cliente.dataNascimento > DateTime.Now.AddYears(-minimunAge))
                {
                    return Results.Problem("Cliente deve ter mais de 18 anos");
                }
                if (cliente.nome == null || cliente.nome == "")
                {
                    return Results.Problem("Nome do cliente é obrigatório");
                }
                if (cliente.cpf == null || cliente.cpf == "")
                {
                    return Results.Problem("CPF do cliente é obrigatório");
                }
                var clienteCpf = baseAcademia.Cliente.Where(c => c.cpf == cliente.cpf).FirstOrDefault();
                if (clienteCpf != null)
                {
                   return Results.Problem("CPF já cadastrado");
                }
                if (cliente.email == null || cliente.email == "")
                {
                   return Results.Problem("Email do cliente é obrigatório");
                }
                if (cliente.telefone == null || cliente.telefone == "")
                {
                   return Results.Problem("Telefone do cliente é obrigatório");
                }
                if (cliente.dataNascimento == null)
                {
                    return Results.Problem("Data de nascimento do cliente é obrigatório");
                }
                baseAcademia.Cliente.Add(cliente);
                baseAcademia.SaveChanges();
                return Results.Ok();
            });

			//atualizar cliente
            // nome, email, telefone, cpf
			app.MapPost("/cliente/atualizar/{id}", (BaseAcademia baseAcademia, Cliente clienteAtualizado, int id) =>
			{
				var cliente = baseAcademia.Cliente.Find(id);
                if (cliente == null)
                {
                    return Results.NotFound("Cliente não encontrado");
                }
                if (clienteAtualizado.nome != null) 
                {
                    cliente.nome = clienteAtualizado.nome;
                }
                if (clienteAtualizado.email != null)
                {
                    cliente.email = clienteAtualizado.email;
                }
                if (clienteAtualizado.telefone != null)
                {
                    cliente.telefone = clienteAtualizado.telefone;
                }
                if (clienteAtualizado.cpf != null)
                {
                    cliente.cpf = clienteAtualizado.cpf;
                }
				baseAcademia.SaveChanges();
				return Results.Ok();
			});
			//deletar cliente
			app.MapPost("/cliente/deletar/{id}", (BaseAcademia baseAcademia, int id) =>
			{
				var cliente = baseAcademia.Cliente.Find(id);
                if (cliente == null)
                {
                    return Results.NotFound("Cliente não encontrado");
                }
				baseAcademia.Remove(cliente);
				baseAcademia.SaveChanges();
				return Results.Ok();
			});

            //listar todos os planos
            app.MapGet("/planos", (BaseAcademia baseAcademia) => {
                return baseAcademia.Academia.ToList();
            });

            //listar plano especifico por id
            app.MapGet("/plano/{id}", (BaseAcademia baseAcademia, int id) => {
                return baseAcademia.Academia.Find(id);
            });

            //cadastrar plano
            // nomePlano, valorPlano, tipoPlano (Mensal, Anual)
            app.MapPost("/plano/cadastrar", (BaseAcademia baseAcademia, Academia academia) =>
            {   
                if (academia.nomePlano == null || academia.nomePlano == "")
                {
                     return Results.Problem("Nome do plano é obrigatório");
                } 
                if (baseAcademia.Academia.Any(x => x.nomePlano == academia.nomePlano))
                {
                    return Results.Problem("Nome do plano já existe");
                }
                if (academia.valorPlano == null)
                {
                    return Results.Problem("Valor do plano é obrigatório");
                }
                if (academia.tipoPlano == null || academia.tipoPlano == "")
                {
                    return Results.Problem("Tipo do plano é obrigatório");
                }
              
                baseAcademia.Academia.Add(academia);
                baseAcademia.SaveChanges();
                return Results.Ok();
            });

            //atualizar plano
            // nome, valorPlano, tipoPlano
            app.MapPost("/plano/atualizar/{id}", (BaseAcademia baseAcademia, Academia academiaAtualizada, int id) =>
            {   
                var academia = baseAcademia.Academia.Find(id);
                if (academia == null)
                {
                    return Results.NotFound("Plano não encontrado");
                } 

                if ((academiaAtualizada.tipoPlano == "Anual") || (academiaAtualizada.tipoPlano == "anual") || (academiaAtualizada.tipoPlano == "Mensal") || (academiaAtualizada.tipoPlano == "mensal") ) 
                {
                    if (academiaAtualizada.nomePlano != null)
                    {
                        academia.nomePlano = academiaAtualizada.nomePlano;
                    }
                    if (academiaAtualizada.valorPlano != null)
                    {
                        academia.valorPlano = academiaAtualizada.valorPlano;
                    }
                    if (academiaAtualizada.tipoPlano != null)
                    {
                        academia.tipoPlano = academiaAtualizada.tipoPlano;
                    }
                    
                    baseAcademia.SaveChanges();
                    return Results.Ok();
                } else {
                    return Results.NotFound("Plano inválido");
                }
            });

            //deletar plano
            app.MapPost("/plano/deletar/{id}", (BaseAcademia baseAcademia, int id) =>
            {
                var academia = baseAcademia.Academia.Find(id);
                if (academia == null)
                {
                    return Results.NotFound("Plano não encontrado");
                }
                baseAcademia.Remove(academia);
                baseAcademia.SaveChanges();
                return Results.Ok();
            });
            
            //listar todas as assinaturas
            app.MapGet("/assinaturas", (BaseAcademia baseAcademia) => {
                return baseAcademia.Assinatura.ToList();
            });

            //listar assinatura especifica por id
            app.MapGet("/assinatura/{id}", (BaseAcademia baseAcademia, int id) => {
                return baseAcademia.Assinatura.Find(id);
            });

            //cadastrar assinatura
            //idCliente, idPlano, dataInicio
            app.MapPost("/assinatura/cadastrar/{cpf}", (BaseAcademia baseAcademia, Assinatura assinatura, string Cpf) =>
            {
                var cliente = baseAcademia.Cliente.Where((x) => x.cpf == Cpf).First();
                var plano = baseAcademia.Academia.Find(assinatura.idPlano);
                var tipoPlano = baseAcademia.Academia.Find(assinatura.idPlano).tipoPlano;

                if (cliente == null) {
                    return Results.NotFound("O Cliente informado não existe");
                }
                
                if (plano == null) {
                    return Results.NotFound("Plano informado não existe");
                }
                // Verifica se o cliente já possui uma assinatura
                if (baseAcademia.Assinatura.Any(x => x.idCliente == assinatura.idCliente))
                {
                     return Results.Problem("O cliente já possui uma assinatura");
                }

                if (assinatura.dataInicio == null) 
                {
                    assinatura.dataInicio = DateTime.Now;
                }

                if (tipoPlano == "Mensal" || (tipoPlano == "mensal"))
                {
                    assinatura.dataExpirar  = assinatura.dataInicio + TimeSpan.FromDays(30);
                }
                else if (tipoPlano == "Anual" || (tipoPlano == "anual")) 
                {
                    assinatura.dataExpirar = assinatura.dataInicio + TimeSpan.FromDays(365);
                }
                else
                {
                    return Results.NotFound("Tipo de plano inválido");
                }
                assinatura.nomePlano = plano.nomePlano;
                assinatura.nomeCliente = cliente.nome;
                assinatura.idCliente = cliente.id;
                baseAcademia.Assinatura.Add(assinatura);
                baseAcademia.SaveChanges();
                return Results.Ok();
            });

            //atualizar assinatura
            // idPlano, dataExpirar
            app.MapPost("/assinatura/atualizar/{id}", (BaseAcademia baseAcademia, Assinatura assinaturaAtualizada, int id) =>
            {   
                var assinatura = baseAcademia.Assinatura.Find(id);
                if (assinatura == null)
                {
                    return Results.NotFound("Assinatura não encontrada");
                }
                assinatura.idPlano = assinaturaAtualizada.idPlano;

                if (assinaturaAtualizada.dataExpirar != null)
                {
                    assinatura.dataExpirar = assinaturaAtualizada.dataExpirar;
                }

                var cliente = baseAcademia.Cliente.Find(assinatura.idCliente);
                var plano = baseAcademia.Academia.Find(assinatura.idPlano);

                baseAcademia.SaveChanges();
                return Results.Ok();
            });

            //deletar assinatura
            app.MapPost("/assinatura/deletar/{id}", (BaseAcademia baseAcademia, int id) =>
            {
                var assinatura = baseAcademia.Assinatura.Find(id);
                if (assinatura == null)
                {
                     return Results.NotFound("Assinatura não encontrada");
                }
                var cliente = baseAcademia.Cliente.Find(assinatura.idCliente);
                baseAcademia.Remove(assinatura);
                baseAcademia.SaveChanges();

                return Results.Ok();
            });
        
			app.Run("http://localhost:3000");
		}
	}
}