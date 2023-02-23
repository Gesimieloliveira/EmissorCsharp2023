using System;
using System.ComponentModel.DataAnnotations;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Flyouts.Models
{
    public class DocumentoPagarModelNfe : ModelValidation
    {
        public byte Parcela
        {
            get { return GetValue<byte>(); }
            set
            {
                SetValue(value);
            }
        }

        public DateTime? Vencimento
        {
            get { return GetValue<DateTime?>(); }
            set
            {
                SetValue(value);
            }
        }

        [Required(ErrorMessage = @"Porfavor adicionar um valor")]
        public string ValorAjustado
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public bool EditarDataVencimento
        {
            get { return GetValue<bool>(); }
            set
            {
                SetValue(value);
            }
        }
    }
}