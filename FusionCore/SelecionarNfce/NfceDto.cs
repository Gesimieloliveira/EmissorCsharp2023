using System;
using FusionCore.FusionNfce.Fiscal.Flags;
using FusionCore.Repositorio.Base;
using FusionCore.Tributacoes.Flags;
using FusionCore.Vendas.Autorizadores.Nfce;

namespace FusionCore.SelecionarNfce
{
    public class NfceDto : EntidadeBase<int>
    {
        public int Id { get; set; }
        public short Serie { get; set; }
        public int NumeroFiscal { get; set; }
        public decimal TotalFiscal { get; set; }
        public Status Status { get; set; }
        public SituacaoFiscal SituacaoFiscal { get; set; }
        public int IdEmitente { get; set; }
        public string RazaoSocialEmitente { get; set; }
        public string NomeCliente { get; set; }
        public RegimeTributario RegimeTributario { get; set; }
        public bool PontoDeVendaNfce { get; set; }
        public bool Faturamento { get; set; }


        public int IdCliente { get; set; }

        protected override int ChaveUnica => Id;

        public ClienteDto ObterCliente()
        {
            if (IdCliente == 0) return null;

            var cliente = new ClienteDto(NomeCliente, IdCliente);
            return cliente;
        }

        public EmitenteDto ObterEmitente()
        {
            return new EmitenteDto(RazaoSocialEmitente, IdEmitente, RegimeTributario);
        }

        public void ResolveStatus()
        {
            switch (SituacaoFiscal)
            {
                case SituacaoFiscal.Aberta:
                    Status = Status.Aberta;
                    break;
                case SituacaoFiscal.Cancelado:
                    Status = Status.Cancelada;
                    break;
                case SituacaoFiscal.Autorizada:
                    Status = Status.Transmitida;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void VemDePontoDeVendaNfce()
        {
            PontoDeVendaNfce = true;
        }

        public void VemDeFaturamento()
        {
            Faturamento = true;
        }
    }
}