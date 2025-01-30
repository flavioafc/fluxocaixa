using ApiControleLancamentos.Domain.Entities;
using ApiControleLancamentos.Domain.Enums;
using ApiControleLancamentos.Domain.Exceptions;

namespace ApiControleLancamentoTests
{
    public class LancamentoTests
    {

        [Fact]
        public void DeveCriarLancamentoCredito_QuandoValorPositivo()
        {
            // Arrange
            var valor = 100m;
            var tipo = TipoLancamento.Credito;
            var descricao = "Venda realizada";
            var data = DateTime.Now.AddDays(-1);

            // Act
            var lancamento = new Lancamento(valor, tipo, descricao, data);

            // Assert
            Assert.Equal(valor, lancamento.Valor);
            Assert.Equal(tipo, lancamento.Tipo);
            Assert.Equal(descricao, lancamento.Descricao);
            Assert.Equal(StatusLancamento.Ativo, lancamento.Status);
            Assert.Equal(data, lancamento.Data);
        }


        [Fact]
        public void DeveCriarLancamento_QuandoDadosValidos()
        {
            // Arrange
            var valor = 100.0m;
            var tipo = TipoLancamento.Credito;
            var descricao = "Venda de produto";
            var data = DateTime.Now.AddDays(-1);

            // Act
            var lancamento = new Lancamento(valor, tipo, descricao, data);

            // Assert
            Assert.Equal(valor, lancamento.Valor);
            Assert.Equal(tipo, lancamento.Tipo);
            Assert.Equal(descricao, lancamento.Descricao);
            Assert.Equal(StatusLancamento.Ativo, lancamento.Status);
            Assert.Equal(data, lancamento.Data);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void NaoDeveCriarCredito_ComValorZeroOuNegativo(decimal valorInvalido)
        {
            // Arrange
            var tipo = TipoLancamento.Credito;

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Lancamento(valorInvalido, tipo, "Erro", DateTime.Now));

            Assert.Equal("Para crédito, o valor deve ser maior que zero.", ex.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-15)]
        public void NaoDeveCriarDebito_ComValorZeroOuNegativo(decimal valorInvalido)
        {
            var tipo = TipoLancamento.Debito;

            var ex = Assert.Throws<DomainException>(() =>
                new Lancamento(valorInvalido, tipo, "Erro", DateTime.Now));

            Assert.Equal("Para débito, o valor deve ser maior que zero.", ex.Message);
        }


        [Fact]
        public void DeveCriarLancamentoDebito_QuandoValorPositivo()
        {
            // Arrange
            var valor = 50m;
            var tipo = TipoLancamento.Debito;
            var data = DateTime.Now;

            // Act
            var lancamento = new Lancamento(valor, tipo, "Pgto. conta", data);

            // Assert
            Assert.Equal(valor, lancamento.Valor);
            Assert.Equal(tipo, lancamento.Tipo);
        }


        [Fact]
        public void DeveCancelarLancamento_QuandoStatusAtivo()
        {
            // Arrange
            var lancamento = new Lancamento(100m, TipoLancamento.Credito, "Ok", DateTime.Now);

            // Act
            lancamento.Cancelar();

            // Assert
            Assert.Equal(StatusLancamento.Cancelado, lancamento.Status);
        }

        [Fact]
        public void DeveCriarEstorno_ComValorNegativo()
        {
            // Arrange
            var valor = -100m;
            var tipo = TipoLancamento.Estorno;

            // Act
            var lancamento = new Lancamento(valor, tipo, "Reembolso", DateTime.Now);

            // Assert
            Assert.Equal(valor, lancamento.Valor);
            Assert.Equal(tipo, lancamento.Tipo);
        }

        [Fact]
        public void NaoDeveCriarEstorno_ComValorPositivo()
        {
            var valorPositivo = 100m;
            var tipo = TipoLancamento.Estorno;

            var ex = Assert.Throws<DomainException>(() =>
                new Lancamento(valorPositivo, tipo, "Estorno inválido", DateTime.Now));

            Assert.Equal("Para estorno, o valor deve ser negativo (reversão).", ex.Message);
        }



        [Fact]
        public void NaoDeveCancelarLancamento_SeJaCancelado()
        {
            // Arrange
            var lancamento = new Lancamento(100m, TipoLancamento.Credito, "Ok", DateTime.Now);
            lancamento.Cancelar();

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => lancamento.Cancelar());
            Assert.Equal("Lançamento já está cancelado.", ex.Message);
        }

        [Fact]
        public void NaoDevePermitirDataFutura()
        {
            // Arrange
            var dataFutura = DateTime.Now.AddDays(1);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() =>
                new Lancamento(200m, TipoLancamento.Credito, "Teste Data", dataFutura));

            Assert.Equal("Data de lançamento não pode ser futura.", ex.Message);
        }

    }
}