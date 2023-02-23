using System;
using System.Diagnostics;
using System.IO;
using FusionCore.Helpers.Ambiente;
using FusionLibrary.Helper.Criptografia;
using FusionLibrary.VisaoModel;

namespace FusionWPF.Base.Utils.Dialogs
{
    public enum BoxType
    {
        Error,
        Warning,
        Info
    }

    public class MetroDialogBoxModel : ViewModel
    {
        private readonly Exception _exception;
        private string _mensagemConfirmacao;
        private string _mensagemConfirmadaDigitada;
        private bool _habilitarBotaoConfirmar;
        private bool _habilitaConfirmacao;
        private int _alturaMensagem = 110;
        private int _larguraMaxima = 560;

        public BoxType Tipo
        {
            get { return GetValue<BoxType>(); }
            set { SetValue(value); }
        }

        public string Titulo
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Mensagem
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public bool TemException
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }

        public MetroDialogBoxModel(string titulo, string mensagem, BoxType boxType, string mensagemConfirmacao = null)
        {
            Titulo = titulo;
            Mensagem = mensagem;
            Tipo = boxType;
            MensagemConfirmacao = mensagemConfirmacao;
            HabilitaConfirmacao = mensagemConfirmacao != null;
            AlturaMensagem = HabilitaConfirmacao ? 150 : 108;
            LarguraMaxima = HabilitaConfirmacao ? 700 : 560;
        }

        public bool HabilitaConfirmacao
        {
            get => _habilitaConfirmacao;
            set
            {
                _habilitaConfirmacao = value;
                PropriedadeAlterada();
            }
        }

        public string MensagemConfirmacao
        {
            get => _mensagemConfirmacao;
            set
            {
                _mensagemConfirmacao = value;
                PropriedadeAlterada();
            }
        }

        public MetroDialogBoxModel(string titulo, string mensagem, Exception ex, BoxType boxType = BoxType.Error)
            : this(titulo, mensagem, boxType)
        {
            _exception = ex;
            TemException = true;
        }

        public void MostrarInformacoesErro()
        {
            var errorInfo = GetExceptionDetails(_exception);
            var assemblyDir = DiretorioAssembly.GetPastaTemp();
            var tempFile = Path.Combine(assemblyDir, Md5Helper.ComputaUnique() + ".txt");

            File.WriteAllText(tempFile, errorInfo);
            Process.Start(tempFile);
        }

        private static string GetExceptionDetails(Exception exception)
        {
            var message = exception.Message + Environment.NewLine + exception.StackTrace;

            if (exception.InnerException == null)
            {
                return message;
            }

            message += Environment.NewLine + GetExceptionDetails(exception.InnerException);

            return message;
        }

        public void ConfirmaMensagem()
        {
            HabilitarBotaoConfirmar = MensagemConfirmacao == MensagemConfirmadaDigitada;
        }

        public bool HabilitarBotaoConfirmar
        {
            get => _habilitarBotaoConfirmar;
            set
            {
                _habilitarBotaoConfirmar = value;
                PropriedadeAlterada();
            }
        }

        public string MensagemConfirmadaDigitada
        {
            get => _mensagemConfirmadaDigitada;
            set
            {
                _mensagemConfirmadaDigitada = value;
                PropriedadeAlterada();
                ConfirmaMensagem();
            }
        }

        public int AlturaMensagem
        {
            get => _alturaMensagem;
            set
            {
                _alturaMensagem = value;
                PropriedadeAlterada();
            }
        }

        public int LarguraMaxima
        {
            get => _larguraMaxima;
            set
            {
                _larguraMaxima = value;
                PropriedadeAlterada();
            }
        }
    }
}