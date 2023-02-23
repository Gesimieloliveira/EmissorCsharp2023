using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Fusion.Visao.Base.VisaoModel;
using FusionCore.FusionAdm.Fiscal.Helpers;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.FusionAdm.Fiscal.NF.CCe;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.NfeCartaCorrecao
{
    public sealed class NfeCartaCorrecaoFormModel : FormModelValidationBase<CartaCorrecaoNfe>
    {
        private ObservableCollection<CartaCorrecaoNfe> _itens;
        private readonly Nfeletronica _nfe;
        private readonly SessaoManagerAdm _sessaoManager = new SessaoManagerAdm();

        [Required(ErrorMessage = @"Correção deve ter no mínimo 15 caracteres")]
        [MinLength(15, ErrorMessage = @"Correção deve ter no mínimo 15 caracteres")]
        public string Correcao
        {
            get { return GetValue(() => Correcao); }
            set
            {
                SetValue(value);
                PropriedadeAlterada();
            }
        }

        public CartaCorrecaoNfe ItemSelecionado
        {
            get { return GetValue(() => ItemSelecionado); }
            set
            {
                SetValue(value);
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<CartaCorrecaoNfe> Itens
        {
            get { return _itens; }
            set
            {
                if (Equals(value, _itens)) return;
                _itens = value;
                PropriedadeAlterada();
            }
        }

        public NfeCartaCorrecaoFormModel(Nfeletronica nfe)
        {
            _nfe = nfe;
            _itens = new ObservableCollection<CartaCorrecaoNfe>();
        }

        public event EventHandler<CartaCorrecaoNfe> SucessoEmissao;
        public event EventHandler<string> FalhaEmissao;

        private void OnSucessoEmissao(CartaCorrecaoNfe e)
        {
            Application.Current.Dispatcher.Invoke(() => { SucessoEmissao?.Invoke(this, e); });
        }

        private void OnFalhaEmissao(string e)
        {
            Application.Current.Dispatcher.Invoke(() => { FalhaEmissao?.Invoke(this, e); });
        }

        public override void PreencherViewModel()
        {
            Itens.Clear();

            using (var repositorio = new RepositorioCCe(_sessaoManager.CriaSessao()))
            {
                var todasCce = repositorio.BuscaPelaNfe(_nfe);
                todasCce?.ForEach(Itens.Add);
            }
        }

        public void EmitirCceAsync()
        {
            var correcao = Correcao.TrimSefaz();
            var emissorFisccal = _nfe.Emitente.CarregarDadosEmissor(_sessaoManager);
            var solicitacao = new SolicitacaoCCe(_nfe, correcao);
            var emissorCce = new EmissorCCeSefaz(emissorFisccal.EmissorFiscalNfe, _sessaoManager, _nfe.Finalizacao.Chave.Chave);

            emissorCce.ConfigurarServico();
            emissorCce.Falha += FalhaEmitirCce;
            emissorCce.Sucesso += SucessoEmitirCce;

            emissorCce.EmitirCartaCorrecaoAsync(solicitacao);
        }

        private void FalhaEmitirCce(object sender, FalhaEmissaoCce e)
        {
            if (e.PossuiRetonroEvento)
            {
                OnFalhaEmissao(e.MotivoFalhaEvento);
                return;
            }

            if (e.PossuiRetornoServico)
            {
                OnFalhaEmissao(e.MotivoFalhaServico);
                return;
            }

            OnFalhaEmissao(e.MotivoFalha);
        }

        private void SucessoEmitirCce(object sender, CartaCorrecaoNfe e)
        {
            OnSucessoEmissao(e);
        }

        public void ImprimirCCeSelecionada()
        {
            ImprimirCce(ItemSelecionado);
        }

        public void ImprimirCce(CartaCorrecaoNfe cce)
        {
            try
            {
                CartaCorrecaoHelper.GerarPDF(cce, _nfe.Finalizacao);
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }
    }
}