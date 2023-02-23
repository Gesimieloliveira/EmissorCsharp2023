using FusionCore.FusionAdm.Fiscal.Flags;

namespace FusionCore.Core.SatFiscal
{
    public class ChaveSatFiscal
    {
        private static string _chave;

        private ChaveSatFiscal(byte codigoEstadoUf, int ano, int mes, string cnpj, ModeloDocumento modeloDocumento, string serie, string numeroCupomFiscal, string codigoNumericoAleatorio, byte digitoVerificador)
        {
            CodigoEstadoUf = codigoEstadoUf;
            Ano = ano;
            Mes = mes;
            Cnpj = cnpj;
            ModeloDocumento = modeloDocumento;
            Serie = serie;
            NumeroCupomFiscal = numeroCupomFiscal;
            CodigoNumericoAleatorio = codigoNumericoAleatorio;
            DigitoVerificador = digitoVerificador;
        }

        public byte CodigoEstadoUf { get; }
        public int Ano { get; }
        public int Mes { get; }
        public string Cnpj { get; }
        public ModeloDocumento ModeloDocumento { get; }
        public string Serie { get; }
        public string NumeroCupomFiscal { get; }
        public string CodigoNumericoAleatorio { get; }
        public byte DigitoVerificador { get; }

        public static ChaveSatFiscal LoadChaveSatFiscal(string chave)
        {
            _chave = chave;
            var codigoUf = byte.Parse(chave.Substring(0, 2));
            var anoMes = chave.Substring(2, 4);
            var ano = int.Parse(anoMes.Substring(0, 2));
            var mes = int.Parse(anoMes.Substring(2, 2));
            var cnpj = chave.Substring(6, 14);
            var serieEquipamento = chave.Substring(22, 9);
            var numeroFiscal = chave.Substring(31, 6);
            var codigoAleatorio = chave.Substring(37, 6);
            var digitoVerificador = byte.Parse(chave.Substring(chave.Length-1, 1));

            return new ChaveSatFiscal(codigoUf, ano, mes, cnpj, ModeloDocumento.SAT, serieEquipamento, numeroFiscal, codigoAleatorio, digitoVerificador);
        }

        public override string ToString()
        {
            return _chave;
        }
    }
}