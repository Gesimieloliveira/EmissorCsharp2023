using System;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nosbor.FluentBuilder.Lib;
using NSubstitute;

namespace UnitTest.CupomFiscal.Dominio
{
    [TestClass]
    public class AtivarContingenciaDominioTeste
    {
        private ContingenciaNfce _contingencia;
        private IAtivarContingenciaDominio _ativarContingenciaDominio;

        [TestInitialize]
        public void Inicializar()
        {
            var dataAgora = DateTime.Now;

            _contingencia = FluentBuilder<ContingenciaNfce>.New()
                .With(c => c.Id, 1)
                .With(c => c.EntrouEm, dataAgora)
                .With(c => c.FinalizaEm, dataAgora.AddMinutes(40));


            _ativarContingenciaDominio = Substitute.For<AtivarContingenciaDominio>();
        }


        [TestMethod]
        public void Ativar_contingencia_agora()
        {
            _ativarContingenciaDominio.Ativar().Returns(_contingencia);
            var contingencia = _ativarContingenciaDominio.Ativar();

            Assert.AreEqual(1, contingencia.Id);
        }
    }
}