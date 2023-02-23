using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Windows;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;

// ReSharper disable MemberCanBePrivate.Global

namespace FusionNfce.Visao.Cancelamento
{
    public sealed class CancelamentoNfceModel : ViewModel
    {
        private readonly Nfce _documento;
        private readonly CanceladorSefaz _cancelador;

        [Required(ErrorMessage = @"Porfavor digitar uma justificativa com no mínimo 15 caracteres.")]
        [MinLength(15, ErrorMessage = @"Porfavor digitar uma justificativa com no mínimo 15 caracteres.")]
        public string Justificativa
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public int NumeroDocumento
        {
            get => GetValue<int>();
            set => SetValue(value);
        }

        public string Chave
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public CancelamentoNfceModel(Nfce documento, CanceladorSefaz cancelador)
        {
            _documento = documento;
            _cancelador = cancelador;
        }

        public event EventHandler<string> Sucesso;
        public event EventHandler<string> Falha;

        public void Inicializar()
        {
            NumeroDocumento = _documento.NumeroDocumento;
            Chave = _documento.NumeroChave;
        }

        public void CancelarAsync()
        {
            Task.Run(() => Cancelar());
        }

        private void Cancelar()
        {
            try
            {
                var evento = _cancelador.CancelarDocumento(Justificativa.RemoverAcentos(), _documento);

                if (evento.Status.EstaCancelado)
                {
                    OnSucesso(evento.Status.Texto);
                    return;
                }

                OnFalha(evento.Status.Texto);
            }
            catch (Exception e)
            {
                OnFalha(e.Message);
            }
        }

        private void OnSucesso(string e)
        {
            Application.Current.Dispatcher.Invoke(() => Sucesso?.Invoke(this, e));
        }

        private void OnFalha(string e)
        {
            Application.Current.Dispatcher.Invoke(() => Falha?.Invoke(this, e));
        }
    }
}