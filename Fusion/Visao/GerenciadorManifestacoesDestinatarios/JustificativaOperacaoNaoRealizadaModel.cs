using System;
using FusionCore.Helpers.Hidratacao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.GerenciadorManifestacoesDestinatarios
{
    public class JustificativaOperacaoNaoRealizadaModel : ViewModel
    {
        private string _justificativa;

        public string Justificativa
        {
            get => _justificativa;
            set
            {
                if (value == _justificativa) return;
                _justificativa = value;
                PropriedadeAlterada();
            }
        }

        public void Validar()
        {
            Justificativa = Justificativa.TrimOrEmpty();

            if (Justificativa.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Deve conter uma justificativa com no mínimo 15 caracteres");
            }

            if (Justificativa.Length < 15)
            {
                throw new InvalidOperationException("Justificativa deve ser maior que 15 caracteres");
            }
        }
    }
}