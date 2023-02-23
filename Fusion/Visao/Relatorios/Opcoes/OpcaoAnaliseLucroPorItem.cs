using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoAnaliseLucroPorItem : OpcaoRelatorioFixo<RAnaliseLucroPorItem>
    {
        public override string Descricao { get; } = RAnaliseLucroPorItem.Descricao;
        public override string Grupo { get; } = "Analises";

        protected override RAnaliseLucroPorItem CriaRelatorio()
        {
            return new RAnaliseLucroPorItem(SessaoManager);
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