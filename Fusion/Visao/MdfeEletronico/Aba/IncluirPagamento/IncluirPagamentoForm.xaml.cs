using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Fusion.Parcelamento;
using Fusion.Visao.MdfeEletronico.IncluirPagamento;
using FusionCore.Sessao;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.Parcelamento;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.MdfeEletronico.Aba.IncluirPagamento
{
    public partial class IncluirPagamentoForm
    {
        private ParcelamentoDialog _childAtual;
        private readonly IncluirPagamentoFormModel _model;

        public IncluirPagamentoForm(IncluirPagamentoFormModel model)
        {
            _model = model;
            _model.Fechar += delegate
            {
                Close();
            };
            InitializeComponent();
            DataContext = _model;
        }

        private void IncluirPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.IncluirPagamento();
            }
            catch (InvalidOperationException exception)
            {
                DialogBox.MostraAviso(exception.Message);
            }
        }

        private async void GerarParcelas_OnClick(object sender, RoutedEventArgs e)
        {
            var factory = new ParcelamentoFactory(new SessaoManagerAdm());
            var dialog = factory.CriaDialog(_model.ValorTotalContrato);

            dialog.Contexto.ParceladoComSucesso += (send, args) =>
            {
                _model.ParcelasPagamento = new ObservableCollection<ParcelaDTO>(args.Parcelas.Select(x => new ParcelaDTO
                {
                    Valor = x.Valor,
                    VencimentoEm = x.Vencimento,
                    Numero = x.Numero
                }).ToList());
            };

            _childAtual = dialog;

            await this.ShowChildWindowAsync(_childAtual, ChildWindowManager.OverlayFillBehavior.FullWindow);
        }

        private void AdicionarComponente_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _model.AdicionarComponente();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
        }

        private void DeletarComponente(object sender, RoutedEventArgs e)
        {
            _model.DeletarComponente();
        }

        private void IncluirPagamentoForm_OnContentRendered(object sender, EventArgs e)
        {
            _model.Inicializar();
        }
    }
}
