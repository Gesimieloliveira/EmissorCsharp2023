using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionNfce.Fiscal;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtNfceEmissaoAdm
    {
        public static NfceEmissaoAdm ToAdm(this NfceEmissao emissao, NfceAdm nfceAdm, FusionAdm.Emissores.EmissorFiscal emissorFiscal)
        {

            if (emissao == null) return null;

            var emissaoAdm = new NfceEmissaoAdm
            {
                Nfce = nfceAdm,
                EmissorFiscal = emissorFiscal,
                Protocolo = emissao.Protocolo,
                Chave = emissao.Chave,
                Serie = emissao.Serie,
                TipoAmbiente = emissao.TipoAmbiente,
                VersaoAplicativo = emissao.VersaoAplicativo,
                Versao = emissao.Versao,
                Autorizado = emissao.Autorizado,
                ProcessoEmissao = emissao.ProcessoEmissao,
                TagId = emissao.TagId,
                XmlAutorizado = emissao.XmlAutorizado,
                CodigoAutorizacao = emissao.CodigoAutorizacao,
                CodigoNumerico = emissao.CodigoNumerico,
                DigestValue = emissao.DigestValue,
                NumeroDocumento = emissao.NumeroDocumento,
                RecebidoEm = emissao.RecebidoEm,
                TipoEmissao = emissao.TipoEmissao,
                VersaoAplicativoAutorizacao = emissao.VersaoAplicativoAutorizacao,
                EntrouEmContingenciaEm = emissao.EntrouEmContingenciaEm,
                JustificativaContingencia = emissao.JustificativaContingencia,
                NfceId = 0
            };

            return emissaoAdm;
        }  
    }
}