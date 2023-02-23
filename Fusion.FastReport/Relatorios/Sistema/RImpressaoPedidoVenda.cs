using Fusion.FastReport.Repositorios;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.Helpers.Basico;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Sistema
{
    public class RImpressaoPedidoVenda : RelatorioBase
    {
        private int _pedidoId;

        public RImpressaoPedidoVenda(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemTemplate<RImpressaoPedidoVenda>("FrPedidoVenda.frx");
        }

        public void ComPedidoId(int id)
        {
            _pedidoId = id;
        }

        protected override void PrepararDados()
        {
            using (var sessao = SessaoManager.CriaStatelessSession())
            {
                var repositorio = new RepositorioPedido(sessao);
                var estado = repositorio.GetEstadoDoPedido(_pedidoId);

                RegistraParametro("PedidoId", _pedidoId);
                RegistraParametro("EstadoPedido", estado.GetDescription());

                switch (estado)
                {
                    case EstadoAtual.Cancelado:
                        RegistraParametro("WatermarkText", "CANCELADO");
                        break;
                    case EstadoAtual.Faturado:
                        RegistraParametro("WatermarkText", "FATURADO");
                        break;
                }
            }
        }
    }
}