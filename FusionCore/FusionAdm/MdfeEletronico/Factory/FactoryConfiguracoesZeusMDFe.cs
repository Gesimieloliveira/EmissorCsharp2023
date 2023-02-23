using System.IO;
using DFe.Utils;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.MdfeEletronico.Extencoes;
using FusionCore.FusionNfce.Extencoes;
using FusionCore.Helpers.Ambiente;
using FusionLibrary.Helper.Criptografia;
using MDFe.Utils.Configuracoes;
using MDFe.Utils.Flags;
using TpAmbienteFusion = DFe.Classes.Flags.TipoAmbiente;

namespace FusionCore.FusionAdm.MdfeEletronico.Factory
{
    public static class FactoryConfiguracoesZeusMdfe
    {
        public static void CarregarConfiguracao(EmissorFiscalMDFE emissorFiscal)
        {
            var manipulaPasta = new ManipulaPasta(ManipulaPasta.LocalSistema() + "\\XmlMDFe");
            manipulaPasta.CriaPastaSeNaoExistir();

            MDFeConfiguracao.ConfiguracaoCertificado = new ConfiguracaoCertificado
            {
                TipoCertificado = emissorFiscal.EmissorFiscal.TipoCertificadoDigital.ToZeus(),
                Senha = SimetricaCrip.Descomputar(emissorFiscal.EmissorFiscal.SenhaCertificado),
                Arquivo = emissorFiscal.EmissorFiscal.ArquivoCertificado,
                ManterDadosEmCache = true,
                Serial = SimetricaCrip.Descomputar(emissorFiscal.EmissorFiscal.SerialNumberCertificado),
                CacheId = emissorFiscal.EmissorFiscalId.ToString()
            };

            MDFeConfiguracao.CaminhoSalvarXml = ManipulaPasta.LocalSistema() + "\\XmlMDFe";
            MDFeConfiguracao.CaminhoSchemas = Path.Combine(ManipulaPasta.LocalSistema(), "Assets", "Schemas.Mdfe");
            MDFeConfiguracao.IsSalvarXml = true;


            MDFeConfiguracao.VersaoWebService = new MDFeVersaoWebService();
            MDFeConfiguracao.VersaoWebService.TimeOut = 60000;
            MDFeConfiguracao.VersaoWebService.TipoAmbiente = emissorFiscal.Ambiente == TipoAmbiente.Homologacao ? TpAmbienteFusion.Homologacao : TpAmbienteFusion.Producao;
            MDFeConfiguracao.VersaoWebService.UfEmitente = emissorFiscal.EmissorFiscal.Empresa.EstadoDTO.ToZeusMdfe();
            MDFeConfiguracao.VersaoWebService.VersaoLayout = VersaoServico.Versao300;

        }

    }
}