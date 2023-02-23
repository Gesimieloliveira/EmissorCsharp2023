using System;
using DFe.Utils;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.ContingenciaSefaz;
using FusionCore.FusionAdm.Fiscal.ChaveEletronica;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionAdm.Fiscal.Extensoes.ZeusConversores;
using FusionCore.FusionAdm.Fiscal.Fabricas;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Integridade;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionCore.Xml;
using NFe.Classes.Servicos.Autorizacao;
using NFe.Classes.Servicos.Tipos;
using NFe.Utils;
using NFe.Utils.Validacao;
using Shared.NFe.Utils.InfRespTec;

namespace FusionCore.FusionAdm.Fiscal.NF.Autorizacao
{
    public class EmissorNfe
    {
        private readonly ISessaoManager _sessaoManager;
        private readonly ReservadorNumeroNfe _reservador;
        private ContingenciaNfe _contingenciaAtiva;

        public EmissorNfe(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
            _reservador = new ReservadorNumeroNfe(sessaoManager);
        }

        public void AtivarContingencia(ContingenciaNfe contingencia)
        {
            _contingenciaAtiva = contingencia;
        }

        public EmissaoNfe Emitir(Nfeletronica nfe)
        {
            if (PossuiEmissaoPendente(nfe))
            {
                throw new InvalidOperationException("Não foi possivel emitir a NF-e, possui emissão pendente");
            }

            var emissor = nfe.Emitente.CarregarDadosEmissor(_sessaoManager);

            var tipoEmissao = _contingenciaAtiva?.TipoEmissao ?? TipoEmissao.Normal;
            var certificado = CertificadoDigitalFactory.Cria(emissor, true);
            var cfgZeus = new ConfiguracaoZeusBuilder(emissor.EmissorFiscalNfe, tipoEmissao).GetConfiguracao();

            if (!nfe.PossuiNumeroAlocado())
            {
                _reservador.ReservarNumero(nfe, emissor);
            }

            nfe.PrepararParaEmissao(_sessaoManager);

            var componentesChave = new ComponentesChaveNfe(nfe, tipoEmissao);
            var chave = ChaveSefazHelper.GerarChave(componentesChave);
            var ambiente = emissor.EmissorFiscalNfe.Ambiente;

            var emissao = new EmissaoNfe(nfe, emissor, ambiente, chave);

            if (_contingenciaAtiva != null)
            {
                emissao.DefinirContingencia(_contingenciaAtiva);
            }

            var zeusNfe = nfe.ToZeus(emissao, _sessaoManager);

            GerarHashCsrt(nfe, zeusNfe, chave);

            var xmlString = FuncoesXml.ClasseParaXmlString(zeusNfe).RemoverAcentos();

            var assinador = new AssinadorSefaz(certificado);
            var docAssinado = assinador.Assina(xmlString, zeusNfe.infNFe.Id);

            var versao = ServicoNFe.NFeAutorizacao.VersaoServicoParaString(cfgZeus.VersaoNFeAutorizacao);
            var envio = new enviNFe4(versao, 1, IndicadorSincronizacao.Assincrono, null);

            var xmlEnvio = XmlFactory.Cria(FuncoesXml.ClasseParaXmlString(envio));
            var nfeElement = XmlFactory.CriaFragment(docAssinado);

            xmlEnvio.FirstChild.AppendChild(xmlEnvio.ImportNode(nfeElement, true));

            ThrowExceptionSeXmlInvalido(xmlEnvio.OuterXml, cfgZeus);

            using (var sessao = _sessaoManager.CriaSessao())
            {
                emissao.XmlEnvio = xmlEnvio.OuterXml;

                var repositorio = new RepositorioNfe(sessao);
                repositorio.Persistir(emissao);
            }

            return emissao;
        }

        private void GerarHashCsrt(Nfeletronica nfe, NFe.Classes.NFe zeusNfe, ChaveSefaz chave)
        {
            if (zeusNfe.infNFe.infRespTec == null) return;

            var ufId = nfe.Emitente.Empresa.EstadoDTO.Id;

            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorioResponsavelTecnico = new RepositorioResponsavelTecnico(sessao);

                if (repositorioResponsavelTecnico.ExisteCsrt(ufId, TipoDocumentoFiscalEletronico.NFe) == false) return;

                var reponsavelTecnico =
                    repositorioResponsavelTecnico.BuscarPorUf(ufId);

                

                zeusNfe.infNFe.infRespTec.hashCSRT = GerarHashCSRT.HashCSRT(reponsavelTecnico.Csrt, chave.Chave);
                zeusNfe.infNFe.infRespTec.idCSRT = reponsavelTecnico.CsrtId;
            }
        }

        private bool PossuiEmissaoPendente(Nfeletronica nfe)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioNfe(sessao);
                return repositorio.PossuiEmissaoPendente(nfe);
            }
        }

        private static void ThrowExceptionSeXmlInvalido(string xmlEnvio, ConfiguracaoServico cfg)
        {
            ConfiguracaoServico.Instancia.DiretorioSchemas = cfg.DiretorioSchemas;
            //Validador.Valida(ServicoNFe.NFeAutorizacao, cfg.VersaoNFeAutorizacao, xmlEnvio);
            Validador.Valida(ServicoNFe.NFeAutorizacao, cfg.VersaoNFeAutorizacao, xmlEnvio, true, ConfiguracaoServico.Instancia);
        }
    }
}