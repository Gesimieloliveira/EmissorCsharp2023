using System.Windows.Input;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.VisaoModel;
using FusionNfce.AutorizacaoSatFiscal;
using FusionNfce.AutorizacaoSatFiscal.Criadores;
using FusionNfce.AutorizacaoSatFiscal.Ext;
using FusionWPF.Base.Utils.Dialogs;
using OpenAC.Net.Sat;

namespace FusionNfce.Visao.Principal.CodigoAtivacaoSAT
{
    public class TrocaCodigoAtivacaoSATFormModel : ViewModel
    {
        private OpcaoCodigoAtivacao _opcaoCodigoAtivacao = OpcaoCodigoAtivacao.CodigoAtivacao;
        private string _codigoAtivacaoAtual;
        private string _novoCodigoAtivacao;
        private string _confirmacaoNovoCodigoAtivacao;

        public OpcaoCodigoAtivacao OpcaoCodigoAtivacao
        {
            get { return _opcaoCodigoAtivacao; }
            set
            {
                if (value == _opcaoCodigoAtivacao) return;
                _opcaoCodigoAtivacao = value;
                PropriedadeAlterada();
            }
        }

        public string CodigoAtivacaoAtual
        {
            get { return _codigoAtivacaoAtual; }
            set
            {
                if (value == _codigoAtivacaoAtual) return;
                _codigoAtivacaoAtual = value;
                PropriedadeAlterada();
            }
        }

        public string NovoCodigoAtivacao
        {
            get { return _novoCodigoAtivacao; }
            set
            {
                if (value == _novoCodigoAtivacao) return;
                _novoCodigoAtivacao = value;
                PropriedadeAlterada();
            }
        }

        public string ConfirmacaoNovoCodigoAtivacao
        {
            get { return _confirmacaoNovoCodigoAtivacao; }
            set
            {
                if (value == _confirmacaoNovoCodigoAtivacao) return;
                _confirmacaoNovoCodigoAtivacao = value;
                PropriedadeAlterada();
            }
        }

        public ICommand TrocarCommand => GetSimpleCommand(TrocarAction);

        private void TrocarAction(object obj)
        {
            if (Validacoes()) return;


            var acbrSat = CriaAcbrSat.Criar();

            new AtivarSat(acbrSat).Ativar();

            SatResposta resposta;

            using (acbrSat)
            {
                var operacao = (int) OpcaoCodigoAtivacao;

                resposta = acbrSat.TrocarCodigoDeAtivacao(CodigoAtivacaoAtual,
                    operacao,
                    NovoCodigoAtivacao);

                if (resposta.CodigoDeRetorno == 18000)
                {
                    SalvarEmissorComNovoCodigo(NovoCodigoAtivacao);
                }
            }

            if (resposta.MensagemSEFAZ.IsNotNullOrEmpty())
            {
                DialogBox.MostraInformacao("Mensagem Sefaz: " + resposta.MensagemSEFAZ);
            }


            var mensagemExtra = string.Empty;

            if (resposta.RetornoLst.Count >= 4 && resposta.RetornoLst[3].IsNotNullOrEmpty())
            {
                mensagemExtra = resposta.RetornoLst[3] + "\r\n";
            }

            DialogBox.MostraInformacao(mensagemExtra
                    + resposta.MensagemDoCodigoDeRetorno().Mensagem + "\nObservação: "
                    + resposta.MensagemDoCodigoDeRetorno().Observacao
                    + "\n" + resposta.MensagemRetorno);
        }

        private void SalvarEmissorComNovoCodigo(string novoCodigoAtivacao)
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao();
            var transacao = sessao.BeginTransaction();

            using (sessao)
            using (transacao)
            {
                SessaoSistemaNfce.Configuracao.EmissorFiscal.EmissorFiscalSat.CodigoAtivacao = novoCodigoAtivacao;

                var repositorio = new RepositorioEmissorFiscalNfce(sessao);
                repositorio.SalvarESincronizar(SessaoSistemaNfce.Configuracao.EmissorFiscal);

                transacao.Commit();
            }
        }

        private bool Validacoes()
        {
            if (CodigoAtivacaoAtual.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Código ativação atual não pode ser vazio");
                return true;
            }

            if (NovoCodigoAtivacao.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Novo código ativação atual não pode ser vazio");
                return true;
            }

            if (ConfirmacaoNovoCodigoAtivacao.IsNullOrEmpty())
            {
                DialogBox.MostraInformacao("Confirmação novo código ativação atual não pode ser vazio");
                return true;
            }


            return false;
        }
    }
}