using System;
using System.ComponentModel.DataAnnotations;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.DocumentoAPagar.Entidade
{
    public class DocumentoPagarModel : ModelValidation
    {

        public byte Parcela
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        public DateTime? Vencimento
        {
            get => GetValue<DateTime?>();
            set => SetValue(value);
        }

        [Required(ErrorMessage = @"Porfavor adicionar um valor")]
        public decimal ValorAjustado
        {
            get => GetValue<decimal>().Arredonda(2);
            set => SetValue(value.Arredonda(2));
        }
    }
}