using Fusion.FastReport.Relatorios.Sistema.Caixa;
using Fusion.Visao.Relatorios.Comum;

namespace Fusion.Visao.Relatorios.Opcoes.Sistema
{
    public class OpcaoListagemDeCaixasFechados : OpcaoRelatorioSistema<RListagemDeCaixasFechados>
    {
        public override string Descricao { get; } = "Listagem de caixas fechados";

        protected override RListagemDeCaixasFechados CriaRelatorio()
        {
            return new RListagemDeCaixasFechados(SessaoManager);
        }

        protected override void OnDevEditarDesenho(string nomeArquivo)
        {
            using (var report = CriaRelatorio())
            {
                report.DevEditarDesenho(nomeArquivo);
            }
        }
    }
}