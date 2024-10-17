using JornadaMilhas.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace JornadaMilhas.Teste.Integracao
{
    [Collection(nameof(ContextCollection))]
    public class OfertaViagemDalRecuperarTodas
    {
        private readonly JornadaMilhasContext context;

        public OfertaViagemDalRecuperarTodas(ITestOutputHelper output, ContextFixture fixture)
        {
            context = fixture.Context;
            output.WriteLine(context.GetHashCode().ToString());
        }
    }
}
