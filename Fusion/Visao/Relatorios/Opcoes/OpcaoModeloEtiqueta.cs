using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoModeloEtiqueta : OpcaoRelatorioFixo<RModeloEtiqueta>
    {
        public override string Descricao { get; } = "Etiqueta de produtos padrão";
        public override string Grupo { get; } = "Etiquetas";

        protected override RModeloEtiqueta CriaRelatorio()
        {
            return new RModeloEtiqueta(SessaoManager);
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