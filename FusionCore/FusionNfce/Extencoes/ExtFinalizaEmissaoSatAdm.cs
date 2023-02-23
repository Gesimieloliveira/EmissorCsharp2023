using System;
using FusionCore.Core.SatFiscal;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Nfce;
using FusionCore.FusionAdm.Nfce.SatFiscal;
using FusionCore.FusionNfce.Fiscal.SatFiscal;
using FusionCore.Helpers.Hidratacao;

namespace FusionCore.FusionNfce.Extencoes
{
    public static class ExtFinalizaEmissaoSatAdm
    {
        public static FinalizaEmissaoSatAdm ToAdm(this FinalizaEmissaoSat emissaoSat, NfceAdm nfceAdm, FusionAdm.Emissores.EmissorFiscal emissorFiscal)
        {
            if (emissaoSat == null) return null;

            var emissaoSatAdm = new FinalizaEmissaoSatAdm
            {
                Nfce = nfceAdm,
                XmlRetorno = emissaoSat.XmlRetorno,
                MensagemRetorno = emissaoSat.MensagemRetorno.TrimOrEmpty(),
                AmbienteSefaz = emissaoSat.AmbienteSefaz,
                Chave = emissaoSat.Chave.TrimOrEmpty(),
                NumeroDocumento =  emissaoSat.NumeroDocumento,
                Empresa = emissaoSat.Empresa.ToAdm(),
                CodigoErro = emissaoSat.CodigoErro,
                CodigoRetorno = emissaoSat.CodigoRetorno,
                CodigoSefaz = emissaoSat.CodigoSefaz,
                MensagemSefaz = emissaoSat.MensagemSefaz.TrimOrEmpty(),
                NumeroCaixa = emissaoSat.NumeroCaixa,
                NumeroSessao = emissaoSat.NumeroSessao.TrimOrEmpty(),
                QrCode = emissaoSat.QrCode,
                NfceId = nfceAdm.Id,
                EmissorFiscal = emissorFiscal
            };

            return emissaoSatAdm;
        }

        public static NfceEmissaoAdm ToAdmConverteFinalizacaoSatParaNfceEmissao(this FinalizaEmissaoSatAdm emissao)
        {
            if (emissao == null) return null;

            var chaveSatFiscal = ChaveSatFiscal.LoadChaveSatFiscal(emissao.Chave);

            var emissaoAdm = new NfceEmissaoAdm
            {
                Nfce = emissao.Nfce,
                EmissorFiscal = emissao.EmissorFiscal,
                Protocolo = string.Empty,
                Chave = emissao.Chave,
                Serie = emissao.NumeroCaixa,
                TipoAmbiente = emissao.AmbienteSefaz,
                VersaoAplicativo = "SAT Fiscal",
                Versao = Versao.V310,
                Autorizado = true,
                ProcessoEmissao = ProcessoEmissao.NFeAplicativoContribuinte,
                TagId = $"CFe{chaveSatFiscal}",
                XmlAutorizado = emissao.XmlRetorno,
                CodigoAutorizacao = 0,
                CodigoNumerico = Convert.ToInt32(chaveSatFiscal.CodigoNumericoAleatorio),
                DigestValue = string.Empty,
                NumeroDocumento = emissao.NumeroDocumento,
                RecebidoEm = emissao.Nfce.EmitidaEm,
                TipoEmissao = TipoEmissao.Normal,
                VersaoAplicativoAutorizacao = "SAT Fiscal",
                EntrouEmContingenciaEm = null,
                JustificativaContingencia = string.Empty,
                NfceId = emissao.NfceId
            };

            return emissaoAdm;
        }
    }
}