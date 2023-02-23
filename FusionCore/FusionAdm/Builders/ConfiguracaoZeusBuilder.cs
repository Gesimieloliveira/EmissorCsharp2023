using System;
using System.IO;
using System.Linq;
using System.Reflection;
using DFe.Classes.Entidades;
using DFe.Classes.Flags;
using FusionCore.Configuracoes;
using FusionCore.Core.Net;
using FusionCore.FusionAdm.Fiscal.Extensoes.Flags;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Sessao;
using NFe.Utils;
using Shared.NFe.Utils.Enderecos.NovasUrlsCeara;
using ModeloDocumento = DFe.Classes.Flags.ModeloDocumento;
using TipoAmbiente = DFe.Classes.Flags.TipoAmbiente;

namespace FusionCore.FusionAdm.Builders
{
    public class ConfiguracaoZeusBuilder
    {
        private readonly IDadosServicoSefaz _emissor;
        private readonly TipoEmissao _tipoEmissao;
        private readonly TimeOut _timeOut;

        public ConfiguracaoZeusBuilder(IDadosServicoSefaz emissor, 
                                       TipoEmissao tipoEmissao, 
                                       TimeOut timeOut = null)
        {
            _emissor = emissor;
            _tipoEmissao = tipoEmissao;
            _timeOut = timeOut;
        }

        public ConfiguracaoServico GetConfiguracao(Estado? estado = null)
        {
            if (_emissor == null)
            {
                throw new InvalidOperationException("Nenhum Emissor Fiscal setado no builder de configuração");
            }

            var estadoEmitente = (Estado)_emissor.IbgeEstadoEmissao;

            var assembyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var zCfg = new ConfiguracaoServico
            {
                cUF = estadoEmitente == Estado.RS && estado != null ? estado.Value : estadoEmitente,
                VersaoLayout = VersaoServico.Versao400,
                TimeOut = _timeOut == null ? 30000 : _timeOut.Milessegundos,
                tpEmis = _tipoEmissao.ToZeus(),
                ModeloDocumento = (ModeloDocumento) _emissor.Modelo,
                Certificado = _emissor.Certificado,
                tpAmb = (TipoAmbiente) _emissor.Ambiente,
                SalvarXmlServicos = true,
                DiretorioSchemas = Path.Combine(assembyPath, "Assets", "Schemas.Nfe"),
                DiretorioSalvarXml = Path.Combine(assembyPath, "XmlServicos"),
                ProtocoloDeSeguranca = _emissor.ProtocoloSeguranca.ToSecurityProtocol(),
                RemoverAcentos = true
            };


            if (!Directory.Exists(zCfg.DiretorioSalvarXml))
            {
                Directory.CreateDirectory(zCfg.DiretorioSalvarXml);
            }

            ConfiguraUrlsCeara(zCfg);

            return zCfg;
        }

        private static void ConfiguraUrlsCeara(ConfiguracaoServico zCfg)
        {
            if (zCfg.ModeloDocumento == ModeloDocumento.NFe)
            {
                ConfiguracaoUrls.FactoryUrl = FactoryUrlCearaMudanca.CriaFactoryUrl();
            }
        }
    }
}