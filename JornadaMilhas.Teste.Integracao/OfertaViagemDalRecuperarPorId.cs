using JornadaMilhas.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace JornadaMilhas.Teste.Integracao
{
    //Aplicar o CollectionFixture para ampliar o compartilhamento de conexões entre as classes de teste
    [Collection(nameof(ContextCollection))]
    public class OfertaViagemDalRecuperarPorId // : IClassFixture<ContextFixture>
    {
        private readonly JornadaMilhasContext context;

        public OfertaViagemDalRecuperarPorId(ITestOutputHelper output, ContextFixture fixture)
        {
            // Utilizar o ContextFixure para prover uma unica instancia da conexão com o banco de dados para todos os métodos
            context = fixture.Context;
            output.WriteLine(context.GetHashCode().ToString());
        }

        [Fact]
        public void RetornaNuloQuandoIdInexistente()
        {
            //arrange
            var dal = new OfertaViagemDAL(context);

            //act
            var ofertaRecuperada = dal.RecuperarPorId(-2);

            //assert
            Assert.Null(ofertaRecuperada);
        }
    }
}
