using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FusionCore.DFe.XmlCte.XmlCte.RegistroEventos;
using FusionCore.FusionAdm.CteEletronico.CCe;
using FusionCore.Repositorio.FusionAdm.FabricaRepositorio;
using FusionWPF.Base.Utils;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.CteEletronico.CCe
{
    public partial class CteCartaCorrecao
    {
        private readonly CteCartaCorrecaoModel _model;

        public CteCartaCorrecao(ICartaCorrecaoCte cte, IFabricaRepositorioCte fabricaRepositorio)
        {
            InitializeComponent();
            _model = new CteCartaCorrecaoModel(cte, fabricaRepositorio);
            _model.EnvioFalhou += EnvioFalhou;
            _model.EnvioSucesso += EnvioSucesso;
            DataContext = _model;
        }

        private void EnvioSucesso(object sender, SucessoCCe e)
        {
            try
            {
                OuveRejeicao(e.Retorno);
                _model.SalvarCCe(e.Retorno, e.Evento);


                DialogBox.MostraInformacao("CT-e Carta de Correção enviada com sucesso");
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

        private void EnvioFalhou(object sender, FalhouCCe e)
        {
            Dispatcher.Invoke(() =>
            {
                ProgressBarAgil4.CloseProgressBar();
                DialogBox.MostraInformacao(e.Exception.Message);
            });
        }

        private void OnClickImprimirCCe(object sender, MouseButtonEventArgs e)
        {
            _model.ImprimirCorrecao();
        }

        private void CartaCorrecao_OnClick(object sender, RoutedEventArgs e)
        {
            _model.DeletaCorrecaoSelecionada();
        }

        private async void Enviar_OnClick(object sender, RoutedEventArgs e)
        {
            ProgressBarAgil4.ShowProgressBar();
            await Task.Run(() => _model.EnviaCorrecao());
        }
    }
}