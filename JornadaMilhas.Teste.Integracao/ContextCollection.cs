using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Teste.Integracao
{
    [CollectionDefinition(nameof(ContextCollection))]
    public class ContextCollection : ICollectionFixture<ContextFixture>
    {
        // Collection Fixture permite compartilhar uma instância de setup, como a conexão ao banco de dados, entre múltiplos testes em diferentes classes de teste.
    }
}
