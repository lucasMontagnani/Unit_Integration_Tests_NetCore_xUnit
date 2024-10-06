using JornadaMilhasV1.Modelos;

namespace JornadaMilhas.Teste
{
    public class OfertaViagemConstrutor
    {
        [Fact]
        public void RetornaOfertaValidaQuandoDadosValidos()
        {           //ReturnsTrueGivenValidOfertaViage() OR Construtor_ValidInput_ReturnsTrue()
            // -- AAA Patern (Arrange, Action & Assert) --
            // Cen�rio de Teste -> Arrange
            Rota rota = new ("Minas Gerais", "Rio de Janeiro");
            Periodo periodo = new (new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));
            double preco = 100.0;
            bool validacao = true;
            // A��o do Teste -> Action
            OfertaViagem ofertaViagem = new (rota, periodo, preco);
            // Valida��o do Teste -> Assert
            Assert.Equal(validacao, ofertaViagem.EhValido);
            // O m�todo Equal() receber� dois par�metros: o par�metro esperado e o par�metro que deve ser verificado.
        }

        [Theory] // Theory � uma nota��o do XUnit que permite trabalhar com v�rios cen�rios diferentes dentro do mesmo teste, tendo a mesma expectativa.
        [InlineData("", null, "2024-01-01", "2024-01-02", 0, false)]
        [InlineData("OrigemTeste", "DestinoTeste", "2024-02-01", "2024-02-05", 100, true)]
        [InlineData(null, "S�o Paulo", "2024-01-01", "2024-01-02", -1, false)]
        [InlineData("Vit�ria", "S�o Paulo", "2024-01-01", "2024-01-01", 0, false)]
        [InlineData("Rio de Janeiro", "S�o Paulo", "2024-01-01", "2024-01-02", -500, false)]
        public void RetornaEhValidoDeAcordoComDadosDeEntrada(string origem, string destino, string dataIda, string dataVolta, double preco, bool validacao)
        {
            //arrange
            Rota rota = new Rota(origem, destino);
            Periodo periodo = new Periodo(DateTime.Parse(dataIda), DateTime.Parse(dataVolta));

            //act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            //assert
            Assert.Equal(validacao, oferta.EhValido);
        }

        [Fact]
        public void RetornaMensagemDeErroDeRotaOuPeriodoInvalidosQuandoRotaNula()
        {           //ReturnsErrorMessageGivenRotaOrPeriodoNull() OR Construtor_RotaNull_ReturnsErrorMessage()
            Rota rota = null;
            Periodo periodo = new (new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));
            double preco = 100.0;

            OfertaViagem oferta = new (rota, periodo, preco);

            Assert.Contains("A oferta de viagem n�o possui rota ou per�odo v�lidos.", oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-250)]
        public void RetornaMensagemDeErroDeDataInconsistenteQuandoPeriodoInvalido(double preco)
        {           //ReturnsErrorMessageGivenInvalidPeriodo OR Construtor_InvalidPeriodo_ReturnsErrorMessage()
            Rota rota = new("Minas Gerais", "Rio de Janeiro");
            Periodo periodo = new (new DateTime(2024, 2, 5), new DateTime(2024, 2, 1));
            //double preco = 100.0;

            OfertaViagem oferta = new (rota, periodo, preco);

            Assert.Contains("Erro: Data de ida n�o pode ser maior que a data de volta.", oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }

        [Fact]
        public void RetornaMensagemDeErroDePrecoInvalidoQuandoPrecoMenorQueZero()
        {           //ReturnsErrorMessageGivenPrecoLessThanZero OR Construtor_NegativePreco_ReturnsErrorMessage()
            // arrange
            Rota rota = new("Minas Gerais", "Rio de Janeiro");
            Periodo periodo = new(new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));
            double preco = -100.0;

            // act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            // assert
            Assert.Contains("O pre�o da oferta de viagem deve ser maior que zero.", oferta.Erros.Sumario);
        }

        [Fact]
        public void RetornaTresErrosDeValidacaoQuandoRotaPeriodoPrecoInvalidos()
        {
            //arrange
            int quantidadeEsperada = 3;
            Rota rota = null;
            Periodo periodo = new Periodo(new DateTime(2024, 6, 1), new DateTime(2024, 5, 10));
            double preco = -100;

            //act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            //assert
            Assert.Equal(quantidadeEsperada, oferta.Erros.Count());
        }
    }
}