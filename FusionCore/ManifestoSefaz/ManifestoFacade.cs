using System;
using System.Linq;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;

namespace FusionCore.ManifestoSefaz
{
    public static class ManifestoFacade
    {
        public static void FazCienciaOperacao(EmissorFiscal emissor, string chave)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioManifestoDfe(sessao);

                if (repositorio.GetManifesto(chave, TipoManifesto.CienciaOpercao) != null)
                {
                    throw new JaManifestadoException("Já fiz um manifesto para Ciência da Operação com essa Chave");
                }

                var zeusBuilder = new ConfiguracaoZeusBuilder(emissor.EmissorFiscalNfe, TipoEmissao.Normal);
                var cfg = zeusBuilder.GetConfiguracao();
                var servico = new ServicosNFe(cfg);

                var tipo = NFeTipoEvento.TeMdCienciaDaOperacao;
                var lote = 1;
                var sequencia = 1;

                var resposta =
                    servico.RecepcaoEventoManifestacaoDestinatario(lote, sequencia, chave, tipo, emissor.Cnpj);

                var cStatLote = resposta.Retorno.cStat;
                var xMotivLote = resposta.Retorno.xMotivo;
                var temEventos = resposta.Retorno.retEvento?.Any() == true;

                if (cStatLote != 128 || !temEventos)
                {
                    throw new InvalidOperationException($"Falha ao Manifestar: {cStatLote} - {xMotivLote}");
                }

                var cStat = resposta.Retorno.retEvento[0].infEvento.cStat;
                var xMotiv = resposta.Retorno.retEvento[0].infEvento.xMotivo;

                if (cStat != 573 && cStat != 135)
                {
                    throw new InvalidOperationException($"Falha ao Manifestar: {cStat} - {xMotiv}");
                }

                var manifesto = new ManifestoDfe
                {
                    Chave = chave,
                    Tipo = TipoManifesto.CienciaOpercao,
                    XmlEnvio = resposta.EnvioStr,
                    XmlResposta = resposta.RetornoCompletoStr
                };

                repositorio.Salva(manifesto);
            }
        }
    }
}