using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionNfce.EmissorFiscal;
using EmissorClasse = FusionCore.FusionAdm.Emissores.EmissorFiscal;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtNfceEmissorFiscal
    {
        public static EmissorClasse ToAdm(this NfceEmissorFiscal dadosServico)
        {
            return new EmissorClasse
            {
                Id = dadosServico.Id
            };
        }

        public static EmissorFiscalSAT ToAdm(this NfceEmissorFiscalSat emissorFiscalSat)
        {
            var emissorSat = new EmissorFiscalSAT
            {
                Fabricante = emissorFiscalSat.Fabricante,
                EmissorFiscal = emissorFiscalSat.EmissorFiscal.ToAdm(),
                Ambiente = emissorFiscalSat.Ambiente,
                CodigoAtivacao = emissorFiscalSat.CodigoAtivacao,
                NumeroCaixa = emissorFiscalSat.NumeroCaixa,
                CodificacaoArquivoXml = emissorFiscalSat.CodificacaoArquivoXml,
                CodigoAcossiacao = emissorFiscalSat.CodigoAcossiacao,
                ArquivoLogo = emissorFiscalSat.ArquivoLogo,
                EmissorFiscalId = emissorFiscalSat.EmissorFiscalId,
                ModeloDocumento = emissorFiscalSat.ModeloDocumento,
                VersaoLayoutSat = emissorFiscalSat.VersaoLayoutSat,
                IsMFe = emissorFiscalSat.IsMFe,
                ChaveAcessoValidador = emissorFiscalSat.ChaveAcessoValidador
            };

            return emissorSat;
        }

    }
}