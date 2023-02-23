using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoNcmVencido : OpcaoRelatorioFixo<RNcmVencido>
    {
        public override string Descricao { get; } = "Relatório de ncms vencidos";
        public override string Grupo { get; } = "Ncm";
        protected override RNcmVencido CriaRelatorio()
        {
            return new RNcmVencido(SessaoManager);
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