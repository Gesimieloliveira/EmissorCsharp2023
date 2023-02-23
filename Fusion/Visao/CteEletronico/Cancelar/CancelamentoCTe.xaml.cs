using System;
using System.Windows;
using FusionCore.DFe.XmlCte.XmlCte.RegistroEventos;
using FusionCore.FusionAdm.CteEletronico.Cancelar;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.Cancelar
{
    public partial class CancelamentoCTe
    {
        private readonly CancelamentoCTeModel _model;

        public CancelamentoCTe(CancelamentoCTeModel model)
        {
            _model = model;
            _model.CancelarRn.FalhouHandler += FalhouCancelar;
            _model.CancelarRn.SucessoHandler += CancelouComSucesso;
            InitializeComponent();
            DataContext = _model;
        }

        private void FalhouCancelar(object sender, Falhou e)
        {
            DialogBox.MostraInformacao(e.Exception.Message);
            Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
        }

        private void CancelouComSucesso(object sender, Sucesso e)
        {
            try
            {
                var cStat = e.RetornoCancelamento.RetornoInformacaoEvento.CodigoStatus;

                if (cStat != 135 && cStat != 136 && cStat != 134)
                {
                    DialogBox.MostraInformacao(e.RetornoCancelamento.RetornoInformacaoEvento.Motivo);
                    Dispatcher.Invoke(Close);
                    return;
                }

                _model.SalvarCancelamento();

                OuveRejeicao(e.RetornoCancelamento);

                DialogBox.MostraInformacao("CT-e cancelado com sucesso");
                Dispatcher.Invoke(Close);
            }
            catch (Exception ex)
            {
                DialogBox.MostraInformacao(ex.Message);
            }
            finally
            {
                Dispatcher.Invoke(ProgressBarAgil4.CloseProgressBar);
            }
        }

        private void OuveRejeicao(FusionRetornoRegistroEventoCTe retornoCancelamento)
        {
            var codigoStatus = retornoCancelamento.RetornoInformacaoEvento.CodigoStatus;

            var autorizou = codigoStatus == 135 || codigoStatus == 136 || codigoStatus == 134;

            if (autorizou) return;

            throw new InvalidOperationException("Ouve uma rejeição da sefaz motivo: " +
                                                retornoCancelamento.RetornoInformacaoEvento.Motivo);
        }

        private void OnClickCancelar(object sender, RoutedEventArgs e)
        {
            ProgressBarAgil4.ShowProgressBar();
            _model.CancelarAsync();
        }
    }
}