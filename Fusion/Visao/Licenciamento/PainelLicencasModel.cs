using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.Timers;
using System.Windows;
using Fusion.Factories;
using FusionCore.Seguranca.Licenciamento.Dominio;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Mapping.ByCode;
using NHibernate.Util;

namespace Fusion.Visao.Licenciamento
{
    public sealed class PainelLicencasModel : ViewModel
    {
        private readonly Timer _monitor = new Timer(10000);

        public FlyoutAdicionaLicencaModel AdicionaLicencaModel
        {
            get { return GetValue<FlyoutAdicionaLicencaModel>(); }
            set { SetValue(value); }
        }

        public string MachineKey
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        public ObservableCollection<Licenca> Licencas
        {
            get { return GetValue<ObservableCollection<Licenca>>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<AcessoConcedido> Acessos
        {
            get { return GetValue<ObservableCollection<AcessoConcedido>>(); }
            set { SetValue(value); }
        }

        public AcessoConcedido AcessoSelecionado
        {
            get { return GetValue<AcessoConcedido>(); }
            set { SetValue(value); }
        }

        public Licenca LicencaSelecionada
        {
            get { return GetValue<Licenca>(); }
            set { SetValue(value); }
        }

        public string UltimaSyncOnline
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        public PainelLicencasModel()
        {
            Licencas = new ObservableCollection<Licenca>();
            Acessos = new ObservableCollection<AcessoConcedido>();

            _monitor.Elapsed += MonitorLicencasEmUso;
        }

        private void MonitorLicencasEmUso(object sender, ElapsedEventArgs e)
        {
            _monitor.Stop();

            try
            {
                // Application.Current.Dispatcher.Invoke(() => { PreencherListaAcessos(acessos); });
            }
            catch (Exception)
            {
                //ignore
            }
            finally
            {
                _monitor.Start();
            }
        }

        public void Carregar()
        {
            // TODO 1612 - Carrega licenças e acesso para preencher painel

            // PreencherListaLicencas();
            // PreencherListaAcessos();
        }

        private void PreencherListaLicencas(IEnumerable<Licenca> licencas)
        {
            Licencas.Clear();
            licencas.ForEach(Licencas.Add);
        }

        private void PreencherListaAcessos(IEnumerable<AcessoConcedido> lista)
        {
            Acessos.Clear();
            lista.ForEach(Acessos.Add);
        }

        public void IniciarMonitoramento()
        {
            _monitor.Start();
        }

        public void PararMonitoramento()
        {
            _monitor.Stop();
        }

        public void AdicionarLicencaHandler()
        {
            AdicionaLicencaModel = new FlyoutAdicionaLicencaModel(MachineKey);
            AdicionaLicencaModel.ClickAtivar += ClickAtivarHandler;
        }

        private void ClickAtivarHandler(object sender, FlyoutAdicionaLicencaModel e)
        {
            if (string.IsNullOrWhiteSpace(e.ContraChave))
            {
                DialogBox.MostraAviso("Preciso de uma contra-chave pra ativar!");
                return;
            }

            try
            {
            }
            catch (FaultException ex)
            {
                DialogBox.MostraAviso(ex.Message);
            }
        }

        public void DesalocarAcessoSelecionado()
        {
            var acessoDropar = AcessoSelecionado;

            var decisao = DialogBox.MostraConfirmacao(
                "A maquina que utiliza essa licença será desconectada. Posso continuar?");

            if (decisao != MessageBoxResult.Yes)
                return;

            try
            {
            }
            catch (FaultException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        public void DesalocarLicencaSelecionada()
        {
            var licencaDropar = LicencaSelecionada;

            var decisao = DialogBox.MostraConfirmacao("Licença será removida do Sistema. Deseja continuar?");
            if (decisao != MessageBoxResult.Yes)
                return;

            try
            {
                DialogBox.MostraInformacao("Licenca foi removida com sucesso.");
            }
            catch (Exception e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }
    }
}