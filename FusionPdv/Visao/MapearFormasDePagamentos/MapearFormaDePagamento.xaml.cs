using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionPdv.Servicos.Ecf;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionPdv.Visao.MapearFormasDePagamentos
{
    public partial class MapearFormaDePagamento
    {
        private readonly MapearFormaDePagamentoModel _mapearFormaDePagamentoModel;
        private bool _onClosingChamado;

        public MapearFormaDePagamento()
        {
            InitializeComponent();
            _mapearFormaDePagamentoModel = new MapearFormaDePagamentoModel();
            DataContext = _mapearFormaDePagamentoModel;
        }

        private void MapearFormaDePagamento_OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.F2:
                    Close();
                    break;
            }
        }

        private void BtFechar_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtAdicionarFormaPagamento_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                new AdicionarMeioPagamento(_mapearFormaDePagamentoModel.FormaPagamentosEcf).ShowDialog();
                _mapearFormaDePagamentoModel.AtualizarListaDeFormaPagamentoEcf();
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        private void MapearFormaDePagamento_OnClosing(object sender, CancelEventArgs e)
        {
            AtualizaSessaoDoSistema();

            _onClosingChamado = true;
        }

        private static void AtualizaSessaoDoSistema()
        {
            SessaoSistema.FormasPagamentoEcf = new EcfPegarTiposPagamentos().TipoPagamento();
            SessaoSistema.AliquotasDoEcf = new EcfPegarAliquotas().Aliquotas();

            using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
            {
                var listaInterno = new FormaDePagamentoRepositorio(sessao).BuscaTodos();
                SessaoSistema.FormasPagamentoInterno = listaInterno;
            }
        }

        private void DataGridEcfDt_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                var itemEditado = e.Row.Item as FormaPagamentoEcfDt;
                _mapearFormaDePagamentoModel.SalvarFormaPagamentoDt(itemEditado);
            }
            catch (InvalidOperationException ex)
            {
                DialogBox.MostraAviso(ex.Message);
                if (_onClosingChamado)
                {
                    new MapearFormaDePagamento().ShowDialog();
                }
            }
        }
    }
}