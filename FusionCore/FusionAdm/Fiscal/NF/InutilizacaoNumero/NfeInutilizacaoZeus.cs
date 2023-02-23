using System;
using System.Net;
using FusionCore.Excecoes;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionNfce.Sessao.Sistema;
using FusionCore.Helpers.DocumentoXml;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using ModeloDocumentoZeus = DFe.Classes.Flags.ModeloDocumento;

namespace FusionCore.FusionAdm.Fiscal.NF.InutilizacaoNumero
{
    public class InutilizacaoSucesso
    {
        public string Protocolo { get; set; }
        public string Xml { get; set; }
        public NfeInutilizacaoZeus Inutilizacao { get; set; }
    }

    public class NfeInutilizacaoZeus
    {
        public byte Ano { get; }
        public short Serie { get; }
        public int NumeroInicial { get; }
        public int NumeroFinal { get; }
        public string Justificativa { get; }
        public string CnpjEmitente { get; }

        public NfeInutilizacaoZeus(
            byte ano,
            short serie,
            int numeroInicial,
            int numeroFinal,
            string justificativa,
            string cnpjEmitente)
        {
            Ano = ano;
            Serie = serie;
            NumeroInicial = numeroInicial;
            NumeroFinal = numeroFinal;
            Justificativa = justificativa;
            CnpjEmitente = cnpjEmitente;
        }

        public IDadosServicoSefaz DadosServicoSefaz { private get; set; }
        public ModeloDocumento Modelo => DadosServicoSefaz.Modelo;

        public InutilizacaoSucesso EnviarParaSefaz()
        {
            try
            {
                var cfg = new ConfiguracaoZeusBuilder(DadosServicoSefaz, TipoEmissao.Normal).GetConfiguracao();

                RetornoNfeInutilizacao retornoConsulta = null;

                if (SessaoSistemaNfce.IsEmissorNFce())
                {
                    var servicoNFe = new ServicosNFe(cfg);

                    retornoConsulta = servicoNFe.NfeInutilizacao(
                        CnpjEmitente,
                        Ano,
                        (ModeloDocumentoZeus)DadosServicoSefaz.Modelo,
                        Serie,
                        NumeroInicial,
                        NumeroFinal,
                        Justificativa
                    );
                }

                return CheckRetorno(retornoConsulta);
            }
            catch (WebException ex)
            {
                throw new InvalidOperationException(
                    "Falha de conexão. Não foi possível inutilizar o número! Detalhes : " + ex.Message);
            }
        }

        private InutilizacaoSucesso CheckRetorno(RetornoNfeInutilizacao wsRetorno)
        {
            var xmlHelper = new XmlHelper(wsRetorno.RetornoCompletoStr);
            var cstat = xmlHelper.PegaElemento("cStat");
            var xMotiv = xmlHelper.PegaElemento("xMotivo");
            var protocolo = xmlHelper.PegaElemento("nProt") ?? "000000000000000";

            if (cstat == "102")
            {
                return new InutilizacaoSucesso
                {
                    Protocolo = protocolo,
                    Xml = wsRetorno.RetornoCompletoStr,
                    Inutilizacao = this
                };
            }

            if (cstat == "563" || cstat == "256")
            {
                throw new JaInutilizadoException($"Retorno Sefaz: {cstat} - {xMotiv} | Protocolo: {protocolo}");
            }

            throw new InvalidOperationException($"Retorno Sefaz: {cstat} - {xMotiv} | Protocolo: {protocolo}");
        }
    }
}