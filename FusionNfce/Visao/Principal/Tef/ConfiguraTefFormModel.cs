using System;
using System.Windows.Input;
using FusionCore.FusionNfce.Sessao;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.FusionNfce.Tef;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionNfce.Visao.Principal.Tef
{
    public class ConfiguraTefFormModel : ViewModel
    {
        public ConfiguraTefFormModel()
        {
            CarregarInformacoes();
        }

        private bool _isAtivarTef;
        private string _arqReq = @"C:\TEF_DIAL\req\intpos.001";
        private string _arqResp = @"C:\TEF_DIAL\resp\intpos.001";
        private string _arqSts = @"C:\TEF_DIAL\resp\intpos.sts";
        private string _arqTemp = @"C:\TEF_DIAL\req\intpos.tmp";
        private string _registroCertificado;
        private Operadora _operadora = Operadora.TefDialHomologacao;

        public bool IsAtivarTef
        {
            get => _isAtivarTef;
            set
            {
                _isAtivarTef = value;
                PropriedadeAlterada();
            }
        }

        public string ArqReq
        {
            get => _arqReq;
            set
            {
                _arqReq = value;
                PropriedadeAlterada();
            }
        }

        public string ArqResp
        {
            get => _arqResp;
            set
            {
                _arqResp = value;
                PropriedadeAlterada();
            }
        }

        public string ArqSts
        {
            get => _arqSts;
            set
            {
                _arqSts = value;
                PropriedadeAlterada();
            }
        }

        public string ArqTemp
        {
            get => _arqTemp;
            set
            {
                _arqTemp = value;
                PropriedadeAlterada();
            }
        }

        public string RegistroCertificado
        {
            get => _registroCertificado;
            set
            {
                _registroCertificado = value;
                PropriedadeAlterada();
            }
        }

        public Operadora Operadora
        {
            get => _operadora;
            set
            {
                _operadora = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandSalvar => GetSimpleCommand(SalvarAction);


        private void SalvarAction(object obj)
        {
            try
            {
                if (ArqReq.IsNullOrEmpty())
                    throw new InvalidOperationException("Adicionar caminho requisição");

                if (ArqResp.IsNullOrEmpty())
                    throw new InvalidOperationException("Adicionar caminho resposta");

                if (ArqSts.IsNullOrEmpty())
                    throw new InvalidOperationException("Adicionar caminho sts");

                if (ArqTemp.IsNullOrEmpty())
                    throw new InvalidOperationException("Adicionar caminho temporario");

                RegistroCertificado = RegistroCertificado.TrimOrEmpty();


                var configTef = new ConfigTef
                {
                    ArquivoRequisicao = ArqReq,
                    ArquivoResposta = ArqResp,
                    ArquivoSts = ArqSts,
                    ArquivoTemporario = ArqTemp,
                    IsAtivo = IsAtivarTef,
                    RegistroCertificado = RegistroCertificado,
                    Operadora = Operadora
                };

                using (var sessao = GerenciaSessaoNfce.ObterSessao(GerenciaSessaoNfce.SessaoVenda).AbrirSessao())
                using (var transacao = sessao.BeginTransaction())
                {
                    new RepositorioConfigTef(sessao).Salvar(configTef);

                    transacao.Commit();
                }

                SessaoSistemaNfce.ConfigTef = configTef;


                DialogBox.MostraInformacao("Configuração TEF Salva com sucesso!");
                OnFechar();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void CarregarInformacoes()
        {
            var configTef = SessaoSistemaNfce.ConfigTef;

            ArqReq = configTef.ArquivoRequisicao;
            ArqResp = configTef.ArquivoResposta;
            ArqSts = configTef.ArquivoSts;
            ArqTemp = configTef.ArquivoTemporario;
            IsAtivarTef = configTef.IsAtivo;
            Operadora = configTef.Operadora;
        }
    }
}