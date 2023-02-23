using System;
using System.Globalization;
using FusionPdv.Ecf;

namespace FusionPdv.Servicos.ValidacaoInicial
{
    public class EcfVerificaDataEhora
    {
        public void Executar()
        {
            var dataEcf = SessaoEcf.EcfFiscal.DataEHora();

            var novaData = dataEcf.Subtract(DateTime.Now);

            if (novaData.TotalMinutes >= 10.00 || novaData.TotalMinutes <= -10.00)
            {
                throw new ExceptionDataInvalidaEcf("A data da ecf não confere com a do computador.\nPorfavor ajustar o horário do computador." +
                                                   "\nHora na ecf: " + dataEcf.ToString(CultureInfo.CurrentCulture));
            }

        }

        public bool DiferencaDeHoraDoEcfAceita()
        {
            try
            {
                Executar();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
