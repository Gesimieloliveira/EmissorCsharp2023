using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Repositorio.FusionNfce;

namespace FusionNfce.Servicos
{
    public class RestricaoClienteObrigatorio
    {
        public bool NecessarioCliente(Nfce nfce)
        {
            bool passouLimiteConfiguracao;

            using (var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao())
            {
                var repositorio = new RepositorioConfiguracaoFrenteCaixaNfce(sessao);
                var configuracao = repositorio.BuscarUnico();

                passouLimiteConfiguracao = nfce.TotalNfce >= configuracao.ValorMinimoParaForcarClienteNaVenda;
            }

            return passouLimiteConfiguracao && string.IsNullOrWhiteSpace(nfce.Destinatario?.DocumentoUnico);
        }
    }
}