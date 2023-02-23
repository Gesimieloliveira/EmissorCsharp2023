using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FusionCore.Helpers.ConsultaCnpj;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local

namespace Fusion.Visao.Empresa.ConsultaNaSefaz
{
    public sealed class BuscarEmpresaNaSefazModel : ViewModel
    {
        private string _cnpjEmpresa;
        private bool _isOpen = true;

        public BuscarEmpresaNaSefazModel()
        {
            IsEnable = true;
        }

        public string CnpjEmpresa
        {
            get => _cnpjEmpresa;
            set
            {
                if (value == _cnpjEmpresa) return;
                _cnpjEmpresa = value;
                PropriedadeAlterada();
            }
        }

        public bool IsOpen
        {
            get => _isOpen;
            set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                PropriedadeAlterada();
            }
        }

        public bool IsEnable
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool EmProcessamento
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public ICommand CommandProcuraEmpresa => GetSimpleCommand(ProcuraEmpresaAction);
        public ICommand CommandFechar => GetSimpleCommand(FecharAction);
        public event EventHandler<EmpresaReceitaWs> EmpresaEncontrada;

        private void FecharAction(object obj)
        {
            IsOpen = false;
        }

        private void ProcuraEmpresaAction(object obj)
        {
            EmProcessamento = true;
            IsEnable = false;

            Task.Run(() =>
            {
                var cnpj = Regex.Replace(CnpjEmpresa, "[.-/]", "");
                var receitaws = new ReceitaWs();

                receitaws.RequestSuccess += RequestSuccessHandler;
                receitaws.RequestError += RequestErrorHandler;

                receitaws.FazRequest(cnpj);
            });
        }

        private void RequestSuccessHandler(object sender, EmpresaReceitaWs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                EnableView();
                EmpresaEncontrada?.Invoke(this, e);

                IsOpen = false;
            });
        }

        private void EnableView()
        {
            EmProcessamento = false;
            IsEnable = true;
        }

        private void RequestErrorHandler(object sender, Exception e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                EnableView();
                DialogBox.MostraAviso(e.Message);
            });
        }
    }
}