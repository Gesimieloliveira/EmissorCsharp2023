using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoCompraDetalhada : OpcaoRelatorioFixo<RComprasDetalhado>
    {
        public override string Descricao { get; } = "Relatório de compras detalhado";
        public override string Grupo { get; } = "Analises";

        protected override RComprasDetalhado CriaRelatorio()
        {
            return new RComprasDetalhado(SessaoManager);
        }

        protected override void OnDevEditarDesenho(string arquivoFrx)
        {
            using (var r = CriaRelatorio())
            {
                r.DevEditarDesenho(arquivoFrx);
            }
        }
    }
}