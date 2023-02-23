using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoDocumentoPagar : OpcaoRelatorioFixo<RDocumentoPagar>
    {
        public override string Descricao { get; } = "Documento a Pagar";
        public override string Grupo { get; } = "Financeiro";
        protected override RDocumentoPagar CriaRelatorio()
        {
            return new RDocumentoPagar(SessaoManager);
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