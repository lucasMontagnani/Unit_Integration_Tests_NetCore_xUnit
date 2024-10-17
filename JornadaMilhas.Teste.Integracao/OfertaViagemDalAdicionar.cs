using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace JornadaMilhas.Teste.Integracao
{
    //Aplicar o CollectionFixture para ampliar o compartilhamento de conex�es entre as classes de teste
    [Collection(nameof(ContextCollection))]
    public class OfertaViagemDalAdicionar // : IClassFixture<ContextFixture>
    {
        private readonly JornadaMilhasContext context;

        public OfertaViagemDalAdicionar(ITestOutputHelper output, ContextFixture fixture)
        {
            // Utilizar o ContextFixure para prover uma unica instancia da conex�o com o banco de dados para todos os m�todos
            context = fixture.Context;
            output.WriteLine(context.GetHashCode().ToString());
        }

        [Fact]
        public void RegistraOfertaNoBanco()
        {
            //arrange
            Rota rota = new Rota("S�o Paulo", "Fortaleza");
            Periodo periodo = new Periodo(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));
            double preco = 350;
            var oferta = new OfertaViagem(rota, periodo, preco);

            var dal = new OfertaViagemDAL(context);

            //act
            dal.Adicionar(oferta);

            //assert
            var ofertaIncluida = dal.RecuperarPorId(oferta.Id);
            Assert.NotNull(ofertaIncluida);
            Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
        }

        [Fact]
        public void RegistraOfertaNoBancoComInformacoesCorretas()
        {
            //arrange
            Rota rota = new Rota("S�o Paulo", "Fortaleza");
            Periodo periodo = new Periodo(new DateTime(2024, 8, 20), new DateTime(2024, 8, 30));
            double preco = 350;

            var oferta = new OfertaViagem(rota, periodo, preco);
            var dal = new OfertaViagemDAL(context);

            //act
            dal.Adicionar(oferta);

            //assert
            var ofertaIncluida = dal.RecuperarPorId(oferta.Id);
            Assert.Equal(ofertaIncluida.Rota.Origem, oferta.Rota.Origem);
            Assert.Equal(ofertaIncluida.Rota.Destino, oferta.Rota.Destino);
            Assert.Equal(ofertaIncluida.Periodo.DataInicial, oferta.Periodo.DataInicial);
            Assert.Equal(ofertaIncluida.Periodo.DataFinal, oferta.Periodo.DataFinal);
            Assert.Equal(ofertaIncluida.Preco, oferta.Preco, 0.001);
        }
    }
}