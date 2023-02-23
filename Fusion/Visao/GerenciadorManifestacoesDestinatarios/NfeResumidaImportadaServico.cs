using FusionCore.GerenciarManifestacoesEletronicas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using NHibernate;

namespace Fusion.Visao.GerenciadorManifestacoesDestinatarios
{
    public class NfeResumidaImportadaServico
    {
        private readonly string _chave;

        public NfeResumidaImportadaServico(string chave)
        {
            _chave = chave;
        }

        public void Importada()
        {
            using (var sessao = new SessaoManagerAdm().CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfeResumida = new RepositorioDistribuicaoDFe(sessao);

                if (VerificaSeNFeResumidaFoiImportada(sessao))
                {
                    var nfeResumida = repositorioNfeResumida.BuscarNfeResumidaPela(_chave);

                    if (nfeResumida == null) return;

                    nfeResumida.ImportacaoConcluida();
                    repositorioNfeResumida.Salvar(nfeResumida);
                }

                transacao.Commit();
            }
        }

        public void Deletou()
        {
            using (var sessao = new SessaoManagerAdm().CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorioNfeResumida = new RepositorioDistribuicaoDFe(sessao);
                var nfeResumida = repositorioNfeResumida.BuscarNfeResumidaPela(_chave);

                if (nfeResumida == null) return;

                nfeResumida.DeletarImportacao();

                repositorioNfeResumida.Salvar(nfeResumida);

                transacao.Commit();
            }
        }

        private bool VerificaSeNFeResumidaFoiImportada(ISession sessao)
        {
            var repositorioImportacaoNfe = new RepositorioNotaFiscalCompra(sessao);

            return repositorioImportacaoNfe.JaExisteChaveIgual(_chave);
        }
    }
}