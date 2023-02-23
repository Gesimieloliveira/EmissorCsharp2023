using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.FusionNfce.Fiscal.SatFiscal;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtHistoricoNfceAdm
    {
        public static HistoricoEnvioSatAdm ToAdm(this HistoricoEnvioSat historicoEnvio, NfceAdm nfceAdm)
        {
            return new HistoricoEnvioSatAdm
            {
                AmbienteSefaz = historicoEnvio.AmbienteSefaz,
                Id = 0,
                CodigoErro = historicoEnvio.CodigoErro,
                CodigoRetorno = historicoEnvio.CodigoRetorno,
                CodigoSefaz = historicoEnvio.CodigoSefaz,
                Empresa = historicoEnvio.Empresa.ToAdm(),
                Finalizou = historicoEnvio.Finalizou,
                MensagemRetorno = historicoEnvio.MensagemRetorno,
                MensagemSefaz = historicoEnvio.MensagemSefaz,
                Nfce = nfceAdm,
                NumeroCaixa = historicoEnvio.NumeroCaixa,
                NumeroSessao = historicoEnvio.NumeroSessao,
                XmlEnvio = historicoEnvio.XmlEnvio
            };
        }

        public static NfceEmissaoHistoricoAdm ToAdm(this NfceEmissaoHistorico historicoEnvio, NfceAdm nfceAdm)
        {
            return new NfceEmissaoHistoricoAdm
            {
                Id = 0,
                AmbienteSefaz = historicoEnvio.AmbienteSefaz,
                XmlEnvio = historicoEnvio.XmlEnvio.Valor,
                Finalizou = historicoEnvio.Finalizou.Valor,
                Nfce = nfceAdm,
                AnoMes = historicoEnvio.Chave.AnoMes,
                ChaveTexto = historicoEnvio.ChaveTexto.Valor,
                CnpjEmitente = historicoEnvio.Chave.Cnpj.Valor,
                CodigoAutorizacao = historicoEnvio.CodigoAutorizacao.Valor,
                CodigoIbgeUf = historicoEnvio.Chave.CodigoIbgeUf,
                CodigoNumerico = historicoEnvio.Chave.CodigoNumerico.Valor,
                DigitoVerificador = (short) historicoEnvio.Chave.DigitoVerificador.Valor,
                EntrouEmContingenciaEm = historicoEnvio.Contingencia?.EntrouEm,
                JustificativaContingencia = historicoEnvio.Contingencia?.Justificativa ?? string.Empty,
                ModeloDocumento = historicoEnvio.Chave.ModeloDocumento,
                Motivo = historicoEnvio.Motivo.Valor,
                NumeroFiscal = historicoEnvio.Chave.NumeroFiscal.Valor,
                Serie = historicoEnvio.Chave.Serie.Valor,
                TentouEm = historicoEnvio.TentouEm.Valor,
                TipoEmissao = historicoEnvio.Chave.FormaEmissao,
                Versao = historicoEnvio.Versao,
                XmlRetorno = historicoEnvio.XmlRetorno?.Valor,
                XmlLote = historicoEnvio.XmlLote,
                FalhaReceberLote = historicoEnvio.FalhaReceberLote
            };
        }
    }
}