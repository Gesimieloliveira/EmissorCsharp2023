using System;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Fiscal.TyneTypes;

namespace FusionCore.Helpers.ChaveFiscal
{
    public class ChaveSefazHelper
    {
        public static Chave LoadChave(string chaveNfe)
        {
            var codigoEstadoUf = byte.Parse(chaveNfe.Substring(0, 2));
            var anoMes = chaveNfe.Substring(2, 4);
            var ano = int.Parse( $"20{anoMes.Substring(0, 2)}");
            var mes = int.Parse(anoMes.Substring(2, 2));
            var anoEMesData = new DateTime(ano, mes, 1);
            var cnpj = chaveNfe.Substring(6, 14);
            ModeloDocumento modelo;
            Enum.TryParse(chaveNfe.Substring(20, 2), out modelo);
            var serie = short.Parse(chaveNfe.Substring(22, 3));
            var numeroNfe = long.Parse(chaveNfe.Substring(25, 9));
            TipoEmissao formaEmissao;
            Enum.TryParse(chaveNfe.Substring(34, 1), out formaEmissao);
            var codigoNumerico = int.Parse(chaveNfe.Substring(35, 8));
            var digitoVerificador = int.Parse(chaveNfe.Substring(43, 1));

            var chaveBuilder = (Chave) new Chave.Builder()
                .ComCodigoIbgeUf(codigoEstadoUf)
                .ComAnoMes(anoEMesData)
                .ComCnpjEmitente(new CnpjEmitente(cnpj))
                .ComModeloDocumento(modelo)
                .ComSerie(new Serie(serie))
                .ComNumeroFiscal(new NumeroFiscal(numeroNfe))
                .ComFormaEmissao(formaEmissao)
                .ComCodigoNumerico(new CodigoNumerico(codigoNumerico))
                .ComDigitoVerificador(new DigitoVerificador(digitoVerificador));

            return chaveBuilder;
        }
    }
}