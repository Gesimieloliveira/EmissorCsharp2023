using System;
using FusionCore.Repositorio.Base;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.FusionNfce.Preferencias
{
    public class PreferenciaTerminal : EntidadeBase<Guid>
    {
        public static Guid StaticGuid = Guid.Parse("B10F24E1-C0D3-4BE9-BC09-3B7EF07B9DA6");

        public PreferenciaTerminal()
        {
            Id = StaticGuid;
            NomeImpressora = string.Empty;
            OpcaoAdicionarVendedor = true;
        }

        private Guid Id { get; set; }
        protected override Guid ChaveUnica => Id;
        public bool SolicitaInformacaoItem { get; set; }
        public bool VisualizaAntesDeImprimir { get; set; }
        public string NomeImpressora { get; set; }
        public bool SolicitaDadosCartaoPos { get; set; }
        public int LimiteBuscaGirdProduto { get; set; }
        public bool SalvarUltimaBuscaProduto { get; set; }
        public bool OpcaoAdicionarVendedor { get; set; }
        public string NomeFantasiaCustomizado { get; set; }
        public LayoutImpressao LayoutImpressao { get; set; } = LayoutImpressao.Impressao80M;
        public bool NaoImprimir { get; set; }
        public int? TabelaPrecoPadrao { get; set; }
        public bool ConfirmacaoTabelaPadraoAntesVenda { get; set; }

        public bool NaoSolicitaDadosCartaoPos()
        {
            return SolicitaDadosCartaoPos == false;
        }
    }
}