using System.Collections.Generic;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades
{
    public class MDFeEventoPagamento : EntidadeBase<int>
    {
        public int Id { get; set; }
        public MDFeEletronico Mdfe { get; set; }
        public string QuantidadeViagens { get; set; }
        public string NumeroReferenciaViagens { get; set; }

        public IList<InformacaoPagamento> InformacaoPagamentoLista { get; set; }
        protected override int ChaveUnica => Id;
        public bool Autorizado { get; set; }
        public string XmlRetorno { get; set; } 
    }
}