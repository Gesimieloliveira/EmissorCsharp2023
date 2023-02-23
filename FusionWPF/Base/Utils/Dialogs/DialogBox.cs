using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using FluentValidation.Results;

namespace FusionWPF.Base.Utils.Dialogs
{
    public static class DialogBox
    {
        public static void MostraInformacao(string mensagem)
        {
            ShowDialogBox(new MetroDialogBoxModel("Informação", mensagem, BoxType.Info));
        }

        public static void MostraMensagemSalvouComSucesso()
        {
            ShowDialogBox(new MetroDialogBoxModel("Tudo OK", "Registro foi salvo com sucesso", BoxType.Info));
        }

        private static void ShowDialogBox(MetroDialogBoxModel model)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = new MetroDialogBox(model);
                view.ShowDialog();
            });
        }

        public static void MostraMensagemDeletouComSucesso()
        {
            ShowDialogBox(new MetroDialogBoxModel("Tudo OK", "Registro foi deletado", BoxType.Info));
        }

        public static void MostraAviso(string mensagem)
        {
            ShowDialogBox(new MetroDialogBoxModel("Aviso", mensagem, BoxType.Warning));
        }

        public static void MostraAviso(string mensagem, IList<string> avisos)
        {
            if (!avisos.Any())
            {
                ShowDialogBox(new MetroDialogBoxModel("Aviso", mensagem, BoxType.Warning));
            }

            var aviso = string.Join(Environment.NewLine, avisos.Select(i => i));
            ShowDialogBox(new MetroDialogBoxModel(mensagem, aviso, BoxType.Warning));
        }

        public static void MostraAviso(string mensagem, ValidationResult validationResult)
        {
            MostraAviso(mensagem, validationResult.Errors.Select(e => e.ErrorMessage).ToList());
        }

        public static void MostraAviso(string titulo, string mensagem)
        {
            ShowDialogBox(new MetroDialogBoxModel(titulo, mensagem, BoxType.Warning));
        }

        public static bool MostraDialogoDeConfirmacao(string mensagem)
        {
            var resposta = MessageBox.Show(
                mensagem,
                "Confirmação",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            return resposta == MessageBoxResult.Yes;
        }

        public static void MostraDialogoDeConfirmacao(string mensagem, Action acaoOk)
        {
            if (MostraDialogoDeConfirmacao(mensagem) == false)
            {
                return;
            }

            acaoOk?.Invoke();
        }

        public static MessageBoxResult MostraConfirmacao(string mensagem)
        {
            return MessageBox.Show(mensagem, @"Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
        }

        public static bool MostraConfirmacao(string mensagem, MessageBoxImage imagem)
        {
            return MessageBox.Show(mensagem, @"Confirmação", MessageBoxButton.YesNo, imagem) == MessageBoxResult.Yes;
        }

        public static void MostraErro(string mensagem, Exception ex = null, BoxType boxType = BoxType.Error)
        {
            ShowDialogBox(new MetroDialogBoxModel("Erro", mensagem, ex, boxType));
        }

        public static bool MostraConfirmacaoComMensagemDeConfirmacao(string titulo,
            string mensagem,
            string mensagemConfirmacao)
        {
            var retorno = false;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var view = new MetroDialogBox(new MetroDialogBoxModel(titulo, mensagem, BoxType.Info, mensagemConfirmacao));
                var result = view.ShowDialog();
                retorno = result ?? false;
            });

            return retorno;
        }
    }
}