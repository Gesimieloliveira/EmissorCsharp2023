using FusionCore.Repositorio.Base;

namespace FusionCore.FusionAdm.PedidoVenda
{
    public class PedidoVendaPreferencia : EntidadeBase<short>
    {
        private PedidoVendaPreferencia()
        {

        }

        public PedidoVendaPreferencia(string idMaquina) : this()
        {
            IdentificadorMaquina = idMaquina;
        }

        public short Id { get; private set; }
        public string IdentificadorMaquina { get; private set; }
        public string Impressora { get; set; } = "";
        public bool ImprimeAposFinalizar { get; set; }
        public bool VisualizarAposFinalizar { get; set; } = true;
        public bool ImprimeDuasVias { get; set; }

        public int ObterQuantidadeVias()
        {
            return ImprimeDuasVias ? 2 : 1;
        }

        protected override short ChaveUnica => Id;
    }
}