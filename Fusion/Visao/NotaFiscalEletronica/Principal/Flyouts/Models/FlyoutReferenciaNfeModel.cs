using System;
using System.Collections.ObjectModel;
using System.Windows;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models
{
    public sealed class FlyoutReferenciaNfeModel : ViewModel
    {
        private readonly Nfeletronica _nfe;
        private readonly ISessaoManager _manager;
        public ObservableCollection<ReferenciaNfe> Referencias { get; } = new ObservableCollection<ReferenciaNfe>();

        public string ChaveReferenciar
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool IsOpen
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public ReferenciaNfe ItemSelecionado
        {
            get { return GetValue<ReferenciaNfe>(); }
            set { SetValue(value); }
        }

        public FlyoutReferenciaNfeModel(Nfeletronica nfe, ISessaoManager manager)
        {
            _nfe = nfe;
            _manager = manager;

            AtualizarListaReferencias();
        }

        private void AtualizarListaReferencias()
        {
            Referencias.Clear();
            _nfe?.Referencias.ForEach(Referencias.Add);
        }

        public void RegistraChaveReferenciada()
        {
            try
            {
                var chave = new ChaveSefaz(ChaveReferenciar);

                if (!chave.IsValida())
                {
                    throw new InvalidOperationException("A chave que me informou está errada");
                }

                var referencia = new ReferenciaNfe(_nfe, chave);

                using (var sessao = _manager.CriaSessao())
                {
                    var repositorio = new RepositorioNfe(sessao);
                    repositorio.Persistir(referencia);

                    _nfe.AdicionaReferencia(referencia);
                }

                AtualizarListaReferencias();
                Clear();
            }
            catch (Exception e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private void Clear()
        {
            ChaveReferenciar = string.Empty;
        }

        public void RemoveChaveReferenciada()
        {
            var result = DialogBox.MostraConfirmacao("Deseja remover a referencia?");

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            using (var sessao = _manager.CriaSessao())
            {
                var repositorio = new RepositorioNfe(sessao);
                repositorio.Deletar(ItemSelecionado);

                _nfe.RemoveReferencia(ItemSelecionado);
            }

            AtualizarListaReferencias();
        }
    }
}