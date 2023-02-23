using System;
using System.Collections.ObjectModel;
using System.Windows;
using FusionCore.FusionAdm.Fiscal.NF;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models
{
    public sealed class FlyoutReferenciaCfModel : ViewModel
    {
        private readonly Nfeletronica _nfe;
        private readonly ISessaoManager _manager;

        public bool IsOpen
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public int? NumeroEcf
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public int? NumeroCoo
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public ReferenciaCf ItemSelecionado
        {
            get { return GetValue<ReferenciaCf>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<ReferenciaCf> Referencias { get; } = new ObservableCollection<ReferenciaCf>();

        public FlyoutReferenciaCfModel(Nfeletronica nfe, ISessaoManager manager)
        {
            _nfe = nfe;
            _manager = manager;

            AtualizarListaReferencias();
        }

        public void VincularCupomFiscal()
        {
            try
            {
                if (NumeroEcf == null || NumeroEcf <= 0)
                    throw new InvalidOperationException("Preciso que informe o Número do ECF");

                if (NumeroCoo == null || NumeroCoo <= 0)
                    throw new InvalidOperationException("Preciso que informe o Número do COO");

                var referencia = new ReferenciaCf(_nfe, (int) NumeroEcf, (int) NumeroCoo);

                using (var sessao = _manager.CriaSessao())
                {
                    var repositorio = new RepositorioNfe(sessao);
                    repositorio.Persistir(referencia);

                    _nfe.AdicionaReferencia(referencia);
                }

                AtualizarListaReferencias();
                Clear();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }

        private void Clear()
        {
            NumeroEcf = null;
            NumeroCoo = null;
        }

        private void AtualizarListaReferencias()
        {
            Referencias.Clear();
            _nfe?.ReferenciasCf.ForEach(Referencias.Add);
        }

        public void DesvincularCupomSelecionado()
        {
            var result = DialogBox.MostraConfirmacao("Deseja remover a referencia?");

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                using (var sessao = _manager.CriaSessao())
                {
                    var repositorio = new RepositorioNfe(sessao);
                    repositorio.Deletar(ItemSelecionado);

                    _nfe.RemoveReferencia(ItemSelecionado);
                }

                AtualizarListaReferencias();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraInformacao(e.Message);
            }
        }
    }
}