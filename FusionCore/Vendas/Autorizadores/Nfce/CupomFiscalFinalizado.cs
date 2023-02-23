using System;
using FusionCore.Repositorio.Base;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class CupomFiscalFinalizado : EntidadeBase<int>
    {
        private CupomFiscalFinalizado()
        {
            // nhibernate
            CriadoEm = DateTime.Now;
        }

        public CupomFiscalFinalizado(CupomFiscal cupomFiscal
            , string chave, string protocolo
            , DateTime autorizadaEm, string xmlAutorizado) : this()
        {
            CupomFiscal = cupomFiscal;
            Chave = chave;
            Protocolo = protocolo;
            AutorizadaEm = autorizadaEm;
            XmlAutorizado = xmlAutorizado;
        }

        public int CupomFiscalId { get; private set; }
        public DateTime CriadoEm { get; private set; }
        public string Chave { get; private set; }
        public string Protocolo { get; private set; }
        public DateTime AutorizadaEm { get; private set; }
        public string XmlAutorizado { get; private set; }
        public CupomFiscal CupomFiscal { get; private set; }
        protected override int ChaveUnica => CupomFiscalId;
    }
}