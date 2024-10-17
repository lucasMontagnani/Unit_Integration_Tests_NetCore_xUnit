using Bogus;
using JornadaMilhas.Dados;
using JornadaMilhasV1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Teste.Integracao
{
    [Collection(nameof(ContextCollection))]
    public class OfertaViagemDalRecuperaMaiorDesconto : IDisposable
    {
        private readonly JornadaMilhasContext _context;
        private readonly ContextFixture _fixture;

        public OfertaViagemDalRecuperaMaiorDesconto(ContextFixture fixture)
        {
            _context = fixture.Context;
            _fixture = fixture;
        }

        public async void Dispose()
        {
            await _fixture.LimpaDadosDoBanco();
        }

        [Fact]
        // destino = são paulo, desconto = 40, preco = 80
        public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
        {
            //arrange
            var rota = new Rota("Curitiba", "São Paulo");
            Periodo periodo = new PeriodoDataBuilder() { DataInicial = new DateTime(2024, 5, 20) }.Build();
            _fixture.CriaDadosFake();

            var ofertaEscolhida = new OfertaViagem(rota, periodo, 80)
            {
                Desconto = 40,
                Ativa = true
            };


            var dal = new OfertaViagemDAL(_context);
            dal.Adicionar(ofertaEscolhida);


            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
            var precoEsperado = 40;

            //act
            var oferta = dal.RecuperaMaiorDesconto(filtro);

            //assert
            Assert.NotNull(oferta);
            Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
        }

        /*
         * Note que este teste interferira no teste acima caso não for tratado apropriadamente
         * Isso acontece, pois o método acima está esperando uma oferta que tenha o desconto 40. Porém, acabamos de criar um novo teste que a menor oferta é 20.
         * Este teste criou uma nova oferta escolhida com desconto maior e adicionou essa oferta na mesma base de dados que o teste anterior está utilizando. 
         * Para solucionar esse problema, precisamos garantir que cada teste retorne a base de dados para o seu estado original antes de executar um novo teste.
         * Faremos isso criando um método que reverterá essa base de dados para um estado inicial na classe ContextFixture.
         */
        [Fact]
        public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto60()
        {
            //arrange
            var rota = new Rota("Curitiba", "São Paulo");
            Periodo periodo = new PeriodoDataBuilder() { DataInicial = new DateTime(2024, 5, 20) }.Build();
            _fixture.CriaDadosFake();

            var ofertaEscolhida = new OfertaViagem(rota, periodo, 80)
            {
                Desconto = 60,
                Ativa = true
            };


            var dal = new OfertaViagemDAL(_context);
            dal.Adicionar(ofertaEscolhida);


            Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
            var precoEsperado = 20;

            //act
            var oferta = dal.RecuperaMaiorDesconto(filtro);

            //assert
            Assert.NotNull(oferta);
            Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
        }
    }
}
