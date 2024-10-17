using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.MsSql;

namespace JornadaMilhas.Teste.Integracao
{
    // IAsyncLifetime para chamadas assíncronas
    public class ContextFixture : IAsyncLifetime
    {
        public JornadaMilhasContext Context { get; private set; }
        // Propriedade que vai representar o container de MySQL
        // Passar a imagem padrão que tem disponibilizada para utilizar para o banco de dados, e na sequência vamos passar um .build, para ele poder executar esse container.
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

        /*
         * Transferir todas as informações de inicialização que estava no construtor para dentro do método de inicialização do IAsyncLifetime
        public ContextFixture()
        {
            // Para evitar que o xUnit crie uma conexão para cada método de teste que executarmos. De modo a compartilhar conexões entre testes utilizando o ClassFixture
            var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
                //.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=JornadaMilhas;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False")
                // Substituir as informações que estávamos utilizando do nosso banco de dados local pela informação da imagem que vamos utilizar no Docker
                .UseSqlServer(_msSqlContainer.GetConnectionString())
                .Options;

            Context = new JornadaMilhasContext(options);
        }
        */

        public async Task InitializeAsync()
        {
            // Roda o Container
            await _msSqlContainer.StartAsync();
            var options = new DbContextOptionsBuilder<JornadaMilhasContext>()
                .UseSqlServer(_msSqlContainer.GetConnectionString())
                .Options;

            Context = new JornadaMilhasContext(options);
            // Executa as migrations no banco de dados dentro do container
            Context.Database.Migrate();
        }

        public async Task DisposeAsync()
        {
            await _msSqlContainer.StopAsync();
        }

        public void CriaDadosFake()
        {
            var rota = new Rota("Curitiba", "São Paulo");

            var fakerOferta = new Faker<OfertaViagem>()
                .CustomInstantiator(f => new OfertaViagem(
                    rota,
                    new PeriodoDataBuilder().Build(),
                    100 * f.Random.Int(1, 100))
                )
                .RuleFor(o => o.Desconto, f => 40)
                .RuleFor(o => o.Ativa, f => true);

            var lista = fakerOferta.Generate(200);
            Context.OfertasViagem.AddRange(lista);
            Context.SaveChanges();
        }

        // Este método reverterá essa base de dados para um estado inicial, afim de evitar interferencia entre cada teste
        public async Task LimpaDadosDoBanco()
        {
            /*
            Context.OfertasViagem.RemoveRange(Context.OfertasViagem);
            Context.Rotas.RemoveRange(Context.Rotas);
            await Context.SaveChangesAsync();
            */

            Context.Database.ExecuteSqlRaw("DELETE FROM OfertasViagem");
            Context.Database.ExecuteSqlRaw("DELETE FROM Rotas");
        }
        /*
         * Para reverter dados da base de testes em cenários mais complexos, temos algumas bibliotecas que podem auxiliar. 
         * A biblioteca Respawn é uma delas, pois permite que você defina um estado inicial conhecido para o banco de dados antes 
         * de cada teste e depois limpe quaisquer alterações feitas durante o testes, deixando o banco de dados em seu estado inicial.
         */
    }
}
