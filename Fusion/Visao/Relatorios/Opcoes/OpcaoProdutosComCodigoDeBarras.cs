using Fusion.FastReport.Relatorios;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoProdutosComCodigoDeBarras : OpcaoRelatorioFixo<RelatorioFixo>
    {
        public override string Descricao { get; } = "Relatório de produtos com o código de barras";
        public override string Grupo { get; } = "Relatório de Estoque";

        protected override RelatorioFixo CriaRelatorio()
        {
            return new RelatorioFixo(
                SessaoManager,
                "FrProdutosComCodigoDeBarras.frx",
                Descricao
            );
        }
    }
}