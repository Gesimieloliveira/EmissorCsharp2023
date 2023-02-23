using System.Collections.Generic;
using FusionCore.FusionAdm.MdfeEletronico.EventoPagamento.Entidades;
using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.MdfeEletronico.Autorizador
{
    public class MdfeAutorizacaoInformacaoPagamento : EntidadeBase<int>
    {
        public int Id { get; set; }
        public MDFeEletronico Mdfe { get; set; }

        public string NomeContratante { get; set; }
        public string DocumentoUnicoContratante { get; set; }
        public IList<MdfeAutorizacaoComponentePagamentoFrete> ComponentePagamentoFrete { get; set; } = new List<MdfeAutorizacaoComponentePagamentoFrete>();
        public IList<MdfeAutorizacaoParcela> Parcelas { get; set; } = new List<MdfeAutorizacaoParcela>();
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