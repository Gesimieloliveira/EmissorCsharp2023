using System;
using System.Xml;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using NFe.Servicos;

namespace FusionCore.FusionAdm.Fiscal.NF.Autorizacao
{
    public class AutorizadorNfe
    {
        private readonly ISessaoManager _sessaoManager;

        public AutorizadorNfe(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public void AutorizaNaSefaz(EmissaoNfe emissao)
        {
            var emissor = emissao.EmissorFiscal;
            var cfg = new ConfiguracaoZeusBuilder(emissor.EmissorFiscalNfe, emissao.TipoEmissao).GetConfiguracao();
            var certificado = CertificadoDigitalFactory.Cria(emissor, true);

            var ws = ServicoNfeFactory.CriaWsdlAutorizacao(cfg, certificado, false);

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioNfe(sessao);
                emissao.EnviadoEm = DateTime.Now;

                try
                {
                    var xmlRequest = new XmlDocument();
                    xmlRequest.LoadXml(emissao.XmlEnvio);

                    var wsResultado = ws.Execute(xmlRequest);

                    if (wsResultado == null)
                    {
                        throw new InvalidOperationException("Não foi possível obter resposta de autorização da SEFAZ");
                    }

                    emissao.XmlLote = wsResultado.OuterXml;
                    repositorio.Alterar(emissao);
                }
                catch
                {
                    emissao.FalhaReceberLote = true;
                    repositorio.Alterar(emissao);
                    throw;
                }
            }
        }
    }
}