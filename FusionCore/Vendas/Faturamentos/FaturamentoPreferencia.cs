using FusionCore.Repositorio.Base;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace FusionCore.Vendas.Faturamentos
{
    public class FaturamentoPreferencia : Entidade
    {
        private FaturamentoPreferencia()
        {
            Impressora = string.Empty;
        }

        public FaturamentoPreferencia(string identificadorMaquina) : this()
        {
            IdentificadorMaquina = identificadorMaquina;
            LayoutImpressao = LayoutImpressao.Impressao80M;
            PreVisualizar = true;
        }

        protected override int ReferenciaUnica => Id;
        public short Id { get; private set; }
        public string IdentificadorMaquina { get; set; }
        public string Impressora { get; set; }
        public LayoutImpressao LayoutImpressao { get; set; }
        public bool ImprimirFinalizacao { get; set; }
        public bool ImprimirCupom { get; set; }
        public bool PreVisualizar { get; set; }
        public bool VisualizarCupom { get; set; }
        public bool ImprimeDuasVias { get; set; }
        public bool DesabilitarTelaOpcoes { get; set; }
        public LayoutImpressaoPormissoria LayoutImpressaoPromissoria { get; set; }
    }
}