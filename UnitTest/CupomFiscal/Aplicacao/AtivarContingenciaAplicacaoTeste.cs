using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Aplicacao;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace UnitTest.CupomFiscal.Aplicacao
{
    [TestClass]
    public class AtivarContingenciaAplicacaoTeste
    {
        private IAtivarContingenciaDominio _ativarContingenciaDominio;
        private ITodasContingenciasNfce _todasContingencias;
        private IAtivarContingenciaAplicacao _entrarContingencia;

        [TestInitialize]
        public void Inicializar()
        {
            _todasContingencias = Substitute.For<ITodasContingenciasNfce>();
            _ativarContingenciaDominio = Substitute.For<IAtivarContingenciaDominio>();

            _entrarContingencia = Substitute.ForPartsOf<AtivarContingenciaAplicacao>(_ativarContingenciaDominio, _todasContingencias);
        }

        [TestMethod]
        public void Entrar_contingencia()
        {
            _entrarContingencia.Ativar();

            _todasContingencias.Received(1).ExisteContingenciaEmAberto();
            _ativarContingenciaDominio.Received(1).Ativar();
            _todasContingencias.Received(1).Salvar(Arg.Any<ContingenciaNfce>());
        }

        [TestMethod]
        public void Entrar_contingencia_mas_ja_existe_contingencia_em_aberto()
        {
            _todasContingencias.ExisteContingenciaEmAberto().Returns(true);

            _entrarContingencia.Ativar();

            _todasContingencias.Received(1).ExisteContingenciaEmAberto();
            _ativarContingenciaDominio.Received(0).Ativar();
            _todasContingencias.Received(0).Salvar(Arg.Any<ContingenciaNfce>());
        }
    }
}