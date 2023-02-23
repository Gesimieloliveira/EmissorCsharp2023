using System;
using FusionLibrary.Helper.Diversos;
using FusionLibrary.VisaoModel;

namespace FusionWPF.Parcelamento
{
    public class ParcelaContexto : ViewModel
    {
        internal ParcelaContexto(byte numero, DateTime vencimento, decimal valor)
        {
            Numero = numero;
            Vencimento = vencimento;
            Valor = valor;
        }

        public byte Numero
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        public DateTime? Vencimento
        {
            get => GetValue<DateTime?>();
            set
            {
                SetValue(value);
                CalcularDiasParaVencimento();
            }
        }

        public decimal Valor
        {
            get => GetValue<decimal>();
            set => SetValue(value);
        }

        public int? Dias
        {
            get => GetValue<int?>();
            set
            {
                SetValue(value);
                CalcularVencimentoPeloDia();
            }
        }

        private void CalcularVencimentoPeloDia()
        {
            if (Dias == null)
            {
                SetValue<DateTime?>(null, nameof(Vencimento));
                return;
            }

            var now = DateTime.Now.FixaEmZeroHoras();
            var novoVencimento = now.AddDays(Dias.Value);

            SetValue(novoVencimento, nameof(Vencimento));
        }

        private void CalcularDiasParaVencimento()
        {
            if (Vencimento == null)
            {
                SetValue<int?>(null, nameof(Dias));
                return;
            }

            var now = DateTime.Now.FixaEmZeroHoras();
            var dataFinal = Vencimento.Value.FixaEmZeroHoras();
            var diferenca = (decimal) dataFinal.Subtract(now).TotalDays;
            var diferencaInt = (int) decimal.Round(diferenca, MidpointRounding.AwayFromZero);

            SetValue(diferencaInt, nameof(Dias));
        }

        public ParcelaGerada CriaParcela()
        {
            if (Vencimento == null)
            {
                throw new InvalidOperationException($"Parcela {Numero} com vencimento inválido!");
            }

            return new ParcelaGerada(Numero, Vencimento.Value, Valor);
        }
    }
}