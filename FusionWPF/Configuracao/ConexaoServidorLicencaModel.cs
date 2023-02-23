using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using FusionCore.Helpers.Exe;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;

namespace FusionWPF.Configuracao
{
    public class ConexaoServidorLicencaModel : ViewModel
    {
        public ConexaoServidorLicencaModel()
        {
            if (ArquivoConexaoLicenciamento.Existe == false)
            {
                Servidor = "LOCALHOST";
                Porta = 8561;
            }
            else
            {
                var cfg = ArquivoConexaoLicenciamento.LerArquivo();

                Servidor = cfg.Servidor;
                Porta = cfg.Porta;
            }
        }

        [Required(ErrorMessage = @"Preciso do Nome ou IP do Servidor")]
        public string Servidor
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Preciso da porta do Servidor")]
        public int Porta
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public ICommand CommandSalvar => GetSimpleCommand(Salvar);

        private void Salvar(object obj)
        {
            try
            {
                ThrowExceptionSeExistirErros();

                ArquivoConexaoLicenciamento.UpdateConfigExe(Servidor, Porta.ToString());
                DialogBox.MostraMensagemSalvouComSucesso();

                if (obj is Window form)
                {
                    form.Close();
                }
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }
    }
}