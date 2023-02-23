using System;
using FusionCore.FusionAdm.Configuracoes;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Configuracao.Model
{
    public class ConfiguracaoBalancaModel : AutoSaveModel
    {
        private Balanca _configuracao;

        public byte CasasDecimais
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        public bool Ativo
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public byte TamanhoCodigo
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        public byte DigitoVerificador
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        public ModoDeOperacao ModoDeOperacao
        {
            get => GetValue<ModoDeOperacao>();
            set => SetValue(value);
        }

        public byte InicioQuantificador
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        protected override void OnInicializa()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioBalanca(sessao);
                _configuracao = repositorio.BuscarUnicaBalanca();
            }

            RefreshControls();
        }

        private void RefreshControls()
        {
            if (_configuracao == null)
            {
                return;
            }

            CasasDecimais = _configuracao.CasasDecimais;
            Ativo = _configuracao.Ativo;
            TamanhoCodigo = _configuracao.TamanhoCodigo;
            DigitoVerificador = _configuracao.DigitoVerificador;
            ModoDeOperacao = _configuracao.ModoDeOperacao;
            InicioQuantificador = _configuracao.InicioQuantificador;
        }

        protected override void OnSalvaAlteracoes()
        {
            _configuracao.DigitoVerificador = DigitoVerificador;
            _configuracao.TamanhoCodigo = TamanhoCodigo;
            _configuracao.ModoDeOperacao = ModoDeOperacao;
            _configuracao.Ativo = Ativo;
            _configuracao.CasasDecimais = CasasDecimais;
            _configuracao.AlteradoEm = DateTime.Now;
            _configuracao.InicioQuantificador = InicioQuantificador;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioBalanca(sessao);
                repositorio.Salvar(_configuracao);
            }
        }
    }
}