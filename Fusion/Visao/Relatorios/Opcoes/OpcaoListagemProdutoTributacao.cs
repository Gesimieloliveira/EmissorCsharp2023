using Fusion.FastReport.Relatorios.Fixos;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes
{
    public class OpcaoListagemProdutoTributacao : OpcaoRelatorioFixo<RListagemProdutoTributacao>
    {
        public override string Descricao { get; } = "Relatório de produtos no estoque com as tributações";
        public override string Grupo { get; } = "Relatório de Estoque";

        protected override RListagemProdutoTributacao CriaRelatorio()
        {
            return new RListagemProdutoTributacao(SessaoManager);
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