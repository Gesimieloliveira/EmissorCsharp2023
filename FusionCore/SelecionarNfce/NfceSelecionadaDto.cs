using FusionCore.Repositorio.Base;

namespace FusionCore.SelecionarNfce
{
    public class NfceSelecionadaDto : EntidadeBase<int>
    {
        private NfceSelecionadaDto() { }

        public int Id { get; set; }
        public short Serie { get; set; }
        public int NumeroFiscal { get; set; }
        public decimal TotalFiscal { get; set; }
        public EmitenteDto Emitente { get; set; }
        public ClienteDto Cliente { get; set; }
        public bool Faturamento { get; set; }
        public bool PontoDeVendaNfce { get; set; }

        protected override int ChaveUnica => Id;

        public static NfceSelecionadaDto Com(NfceDto nfceSelecionada)
        {
            return new NfceSelecionadaDto
            {
                TotalFiscal = nfceSelecionada.TotalFiscal,
                Emitente = nfceSelecionada.ObterEmitente(),
                NumeroFiscal = nfceSelecionada.NumeroFiscal,
                Id = nfceSelecionada.Id,
                Serie = nfceSelecionada.Serie,
                Cliente = nfceSelecionada.ObterCliente(),
                Faturamento = nfceSelecionada.Faturamento,
                PontoDeVendaNfce = nfceSelecionada.PontoDeVendaNfce
            };
        }
    }
}