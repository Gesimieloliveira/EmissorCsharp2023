using System.IO;
using System.Xml;
using System.Xml.Serialization;
using DFe.Utils;
using FusionCore.FusionAdm.Emissores.Flags;
using FusionCore.FusionNfce.EmissorFiscal;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.Ambiente;

namespace FusionNfce.AutorizacaoSatFiscal.Configuracao.Configuracao
{
    public class ConfiguracaoDllSatCriador
    {
        private static readonly string ConfigFile = Path.Combine(DiretorioAssembly.GetPastaConfig(), "ConfiguracaoDllSat.xml");

        public static ConfiguracaoDllSat CriaXmlConfigSeNaoExistir(NfceEmissorFiscalSat emissorSat)
        {

            if (new ManipulaArquivo(ConfigFile).Existe())
            {
                var config = BuscarConfiguracao();

                if (emissorSat.Fabricante != config.FabricanteModelo)
                {
                    config.CaminhoDll = emissorSat.Fabricante.CaminhoDll();
                    config.FabricanteModelo = emissorSat.Fabricante;
                    config.ModeloSat = ModeloSatFusion.Cdecl;

                    CaminhoDllMFe(config);

                    Atualizar(config);
                }

                return config;
            }

            var configuracaoDllSat = new ConfiguracaoDllSat
            {
                FabricanteModelo = emissorSat.Fabricante,
                CaminhoDll = emissorSat.Fabricante.CaminhoDll(),
                ModeloSat = ModeloSatFusion.Cdecl
            };

            CaminhoDllMFe(configuracaoDllSat);

            FuncoesXml.ClasseParaArquivoXml(configuracaoDllSat, ConfigFile);

            return configuracaoDllSat;
        }

        private static void CaminhoDllMFe(ConfiguracaoDllSat config)
        {
            if (SessaoSistemaNfce.IsMFe())
                config.CaminhoDll = @"C:\Program Files (x86)\SEFAZ-CE\Driver MFE\Biblioteca de funções\mfe.dll";
        }

        public static ConfiguracaoDllSat BuscarConfiguracao()
        {
            return FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoDllSat>(ConfigFile);
        }

        public static void Atualizar(ConfiguracaoDllSat configuracao)
        {
            using (var stream = new StreamWriter(ConfigFile))
            {
                var xmlSerializer = new XmlSerializer(typeof(ConfiguracaoDllSat));

                xmlSerializer.Serialize(XmlWriter.Create(stream), configuracao);

                stream.Flush();
            }
        }
    }
}