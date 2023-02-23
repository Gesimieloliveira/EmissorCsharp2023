using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoDocumentoReceber : OpcaoRelatorioFixo<RDocumentoReceber>
    {
        public override string Descricao { get; } = "Documento a Receber";
        public override string Grupo { get; } = "Financeiro";
        protected override RDocumentoReceber CriaRelatorio()
        {
            return new RDocumentoReceber(SessaoManager);
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