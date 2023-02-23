using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoProdutoComNcmVencido : OpcaoRelatorioFixo<RProdutosComNcmVencido>
    {
        public override string Descricao { get; } = "Relatório de produtos com ncm vencidos";
        public override string Grupo { get; } = "Ncm";
        protected override RProdutosComNcmVencido CriaRelatorio()
        {
            return new RProdutosComNcmVencido(SessaoManager);
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