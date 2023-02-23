using System;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.SelecionarNfce
{
    public class FiltroConversorNfce
    {
        public string FiltroNomeDoClienteContenha { get; set; }

        public DateTime? FiltroDataInicio { get; set; }

        public DateTime? FiltroDataFinal { get; set; }

        public bool FiltroNfceJaConvertidas { get; set; }

        public void DataInicialNaoPodeSerMaiorQueFinal()
        {
            if ((TemDataInicio() && TemDataFinal())
                && FiltroDataInicio >= FiltroDataFinal)
                throw new InvalidOperationException("Data Emitida em incial não deve ser maior ou igual a Data Emitida em final");
        }

        public bool TemDataInicio()
        {
            return FiltroDataInicio != null;
        }

        public bool TemDataFinal()
        {
            return FiltroDataFinal != null;
        }

        public bool TemCliente()
        {
            return FiltroNomeDoClienteContenha.IsNotNullOrEmpty();
        }

        public bool NaoTemDataFinal()
        {
            return !TemDataFinal();
        }

        public bool NaoTemDataInicio()
        {
            return !TemDataInicio();
        }
    }
}