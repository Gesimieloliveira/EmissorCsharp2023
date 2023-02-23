using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.CupomFiscal.Dominio
{
    [TestClass]
    public class ContingenciaTeste
    {
        private static ContingenciaNfce CriaContingenciaEAtiva()
        {
            var contingencia = new ContingenciaNfce();
            contingencia.Ativar();
            return contingencia;
        }

        [TestMethod]
        public void Contingencia_ativa_40_minutos()
        {
            var contingencia = CriaContingenciaEAtiva();
            var quarentaMinutos = contingencia.FinalizaEm - contingencia.EntrouEm;

            Assert.AreEqual(40, quarentaMinutos.Minutes);
        }
    }
}