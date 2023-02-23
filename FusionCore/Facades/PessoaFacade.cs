using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Sincronizador;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;

namespace FusionCore.Facades
{
    public static class PessoaFacade
    {
        private static ISessaoManager SessaoManager { get; } = new SessaoManagerAdm();

        public static void Salvar(PessoaEntidade pessoa)
        {
            using (var sessao = SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPessoa(sessao);
                repositorio.SalvaAlteracoes(pessoa);

                var servico = new SincronizacaoPendenteServico(
                    sessao,
                    pessoa.EntidadeSincronizavel,
                    pessoa.Referencia
                );

                servico.Salvar();

                transacao.Commit();
            }
        }

        public static void SalvarTelefone(PessoaTelefone telefone)
        {
            using (var sessao = SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPessoa(sessao);
                repositorio.SalvaAlteracoes(telefone);

                transacao.Commit();
            }
        }

        public static void DeletaTelefone(PessoaTelefone telefone)
        {
            using (var sessao = SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPessoa(sessao);
                repositorio.Remove(telefone);

                transacao.Commit();
            }
        }

        public static void SalvarEndereco(PessoaEndereco endereco)
        {
            using (var sessao = SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPessoa(sessao);
                repositorio.SalvaAlteracoes(endereco);

                transacao.Commit();
            }
        }

        public static void DeletaEndereco(PessoaEndereco endereco)
        {
            using (var sessao = SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPessoa(sessao);
                repositorio.SalvaAlteracoes(endereco);
                repositorio.Remove(endereco);

                transacao.Commit();
            }
        }

        public static void SalvarEmail(PessoaEmail email)
        {
            using (var sessao = SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPessoa(sessao);
                repositorio.SalvaAlteracoes(email);

                transacao.Commit();
            }
        }

        public static void DeletarEmail(PessoaEmail email)
        {
            using (var sessao = SessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioPessoa(sessao);
                repositorio.SalvaAlteracoes(email);
                repositorio.Remove(email);

                transacao.Commit();
            }
        }
    }
}