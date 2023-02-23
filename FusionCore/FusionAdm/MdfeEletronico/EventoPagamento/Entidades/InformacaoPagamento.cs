using System.Collections.Generic;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades
{
    public class InformacaoPagamento : EntidadeBase<int>
    {
        public int Id { get; set; }
        public MDFeEventoPagamento EventoPagamento { get; set; }

        public InformacaoPagamento() : this(0)
        {

        }

        public InformacaoPagamento(decimal valorTotalContrato)
        {
            ValorTotalContrato = valorTotalContrato;
            ComponentePagamentoFrete = new List<ComponentePagamentoFrete>();
            Parcelas = new List<MdfeParcela>();
        }

        public string NomeContratante { get; set; }
        public string DocumentoUnicoContratante { get; set; }
        public IList<ComponentePagamentoFrete> ComponentePagamentoFrete { get; set; }
        public IList<MdfeParcela> Parcelas { get; set; }
        public string ContaBancaria { get; set; }
        public string AgenciaBancaria { get; set; }
        public string CnpjIpef { get; set; }
        public bool InformarApenasCnpjIpef { get; set; }
        public IndicadorPagamento IndicadorPagamento { get; set; }
        public decimal ValorTotalContrato { get; set; }
        protected override int ChaveUnica => Id;

        public string ObterCnpj()
        {
            return DocumentoUnicoContratante.Length == 14 ? DocumentoUnicoContratante : null;
        }

        public string ObterCpf()
        {
            return DocumentoUnicoContratante.Length == 11 ? DocumentoUnicoContratante : null;
        }

        public string ObterIdEstrangeiro()
        {
            switch (DocumentoUnicoContratante.Length)
            {
                case 14:
                case 11:
                    return null;
                default:
                    return DocumentoUnicoContratante;
            }
        }
    }
}