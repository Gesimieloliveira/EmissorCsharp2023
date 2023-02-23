using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.FusionNfce.Empresa;

namespace FusionCore.NfceSincronizador.Sync.EmissoresFiscais
{
    public static class EmissorFiscalExt
    {
        public static NfceEmissorFiscal ToNfce(this EmissorFiscal emissorFiscal)
        {
            var nfceEmissorFiscal = new NfceEmissorFiscal
            {
                AlteradoEm = emissorFiscal.AlteradoEm,
                EmissorFiscalNfce = null,
                EmissorFiscalSat = null,
                Descricao = emissorFiscal.Descricao,
                Id = emissorFiscal.Id,
                FlagNfe = emissorFiscal.FlagNfe,
                ArquivoCertificado = emissorFiscal.ArquivoCertificado,
                SenhaCertificado = emissorFiscal.SenhaCertificado,
                FlagNfce = emissorFiscal.FlagNfce,
                FlagSat = emissorFiscal.FlagSat,
                SerialNumberCertificado = emissorFiscal.SerialNumberCertificado,
                TipoCertificadoDigital = emissorFiscal.TipoCertificadoDigital,
                ProtocoloSeguranca = emissorFiscal.ProtocoloSeguranca,
                TerminalOfflineId = emissorFiscal.TerminalOffline == null ? (int?) null : emissorFiscal.TerminalOffline.Id,
                Empresa = new EmpresaNfce
                {
                    Id = emissorFiscal.Empresa.Id
                }
            };

            var emissorFiscalNfce = emissorFiscal.EmissorFiscalNfce;
            var emissorFiscalSat = emissorFiscal.EmissorFiscalSat;
            var autorizadorBaixarXml = emissorFiscal.AutorizadoBaixarXml;

            CriaEmissorFiscalNFCeSeExistir(emissorFiscalNfce, nfceEmissorFiscal);
            CriaEmissorFiscalSatSeExistir(emissorFiscalSat, nfceEmissorFiscal);
            CriaAutorizadorBaixarXmlSeExistir(autorizadorBaixarXml, nfceEmissorFiscal);

            return nfceEmissorFiscal;
        }

        private static void CriaAutorizadorBaixarXmlSeExistir(AutorizadoBaixarXml autorizadorBaixarXml, NfceEmissorFiscal nfceEmissorFiscal)
        {
            if (autorizadorBaixarXml == null) return;

            var nfceAutorizadorBaixaXml = new NfceAutorizadoBaixarXml(autorizadorBaixarXml, nfceEmissorFiscal);

            nfceEmissorFiscal.AutorizadoBaixarXml = nfceAutorizadorBaixaXml;
        }

        private static void CriaEmissorFiscalSatSeExistir(EmissorFiscalSAT emissorFiscalSat,
            NfceEmissorFiscal nfceEmissorFiscal)
        {
            if (emissorFiscalSat == null) return;

            var satEmissorFiscalNfce = new NfceEmissorFiscalSat
            {
                NumeroCaixa = emissorFiscalSat.NumeroCaixa,
                CodigoAtivacao = emissorFiscalSat.CodigoAtivacao,
                ArquivoLogo = emissorFiscalSat.ArquivoLogo,
                Ambiente = emissorFiscalSat.Ambiente,
                CodificacaoArquivoXml = emissorFiscalSat.CodificacaoArquivoXml,
                VersaoLayoutSat = emissorFiscalSat.VersaoLayoutSat,
                CodigoAcossiacao = emissorFiscalSat.CodigoAcossiacao,
                EmissorFiscal = nfceEmissorFiscal,
                Fabricante = emissorFiscalSat.Fabricante,
                ModeloDocumento = emissorFiscalSat.ModeloDocumento,
                EmissorFiscalId = nfceEmissorFiscal.Id,
                IsMFe = emissorFiscalSat.IsMFe,
                ChaveAcessoValidador = emissorFiscalSat.ChaveAcessoValidador
            };

            nfceEmissorFiscal.EmissorFiscalSat = satEmissorFiscalNfce;
        }

        private static void CriaEmissorFiscalNFCeSeExistir(EmissorFiscalNFCE emissorReceber,
            NfceEmissorFiscal emissorNfce)
        {
            if (emissorReceber == null)
            {
                return;
            }

            var nfceEmissorFiscalNfce = new NfceEmissorFiscalNfce
            {
                EmissorFiscal = emissorNfce,
                Csc = emissorReceber.Csc,
                Ambiente = emissorReceber.Ambiente,
                ArquivoLogo = emissorReceber.ArquivoLogo,
                IdToken = emissorReceber.IdToken,
                AlteradoEm = emissorReceber.AlteradoEm,
                EmissorFiscalId = emissorNfce.Id,
                Modelo = emissorReceber.Modelo,
                UsaNumeracaoDiferenteContigencia = emissorReceber.UsaNumeracaoDiferenteContigencia,
                IsIntegradorCeara = emissorReceber.IsIntegradorCeara
            };

            nfceEmissorFiscalNfce.SetSequenciaNormal(emissorReceber.Serie, emissorReceber.NumeroAtual);
            nfceEmissorFiscalNfce.SetSequenciaOffline(emissorReceber.SerieContingencia, emissorReceber.NumeroAtualContingencia);

            emissorNfce.EmissorFiscalNfce = nfceEmissorFiscalNfce;
        }
    }
}