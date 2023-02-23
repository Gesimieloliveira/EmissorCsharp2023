using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using FusionCore.FusionAdm.Builders;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.GerenciarManifestacoesEletronicas.EstrategiasProcessamento;
using FusionCore.Sessao;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using NHibernate;

namespace FusionCore.GerenciarManifestacoesEletronicas
{
    public class MdeServico
    {
        private const int DOC_LOCALIZADO = 138;
        private readonly EmissorFiscalNFE _emissor;
        private readonly ISessaoManager _sessaoManager;

        public MdeServico(EmissorFiscalNFE emissor, ISessaoManager sessaoManager)
        {
            _emissor = emissor;
            _sessaoManager = sessaoManager;
        }

        public bool ForcarNsuZero { get; set; }

        public void ConsultarDocumentos()
        {
            var cfgBuilder = new ConfiguracaoZeusBuilder(_emissor, TipoEmissao.Normal, new TimeOut(60000));
            var configuracao = cfgBuilder.GetConfiguracao();
            var servicoNFe = new ServicosNFe(configuracao);
            var empresa = _emissor.EmissorFiscal.Empresa;

            while (true)
            {
                using (var sessao = _sessaoManager.CriaSessao(IsolationLevel.ReadCommitted))
                {
                    var uf = empresa.CidadeDTO.SiglaUf;
                    var repositorio = new RepositorioDistribuicaoDFe(sessao);
                    var ultimoNsu = ForcarNsuZero ? 0 : repositorio.BuscarUltimoNsu(empresa.DocumentoUnico, _emissor.Ambiente, uf);

                    var mdeConsulta = new MdeConsulta(empresa.DocumentoUnico, _emissor.Ambiente, uf, ultimoNsu);

                    var restornoDist = servicoNFe.NfeDistDFeInteresse(
                        empresa.CidadeDTO.SiglaUf,
                        empresa.DocumentoUnico,
                        ultimoNsu.ToString()
                    );

                    ThrowExceptionSeRejeicaoNaoConclusiva(restornoDist);

                    mdeConsulta.AddResposta(restornoDist);
                    sessao.Persist(mdeConsulta);
                    sessao.Flush();

                    SalvarDocumentosConsultados(restornoDist, sessao);

                    sessao.Transaction.Commit();

                    if (restornoDist.Retorno.cStat != DOC_LOCALIZADO 
                        || mdeConsulta.UltimoNsu == mdeConsulta.MaiorNsu)
                    {
                        break;
                    }

                    Thread.Sleep(5000);
                }
            }
        }

        private void ThrowExceptionSeRejeicaoNaoConclusiva(RetornoNfeDistDFeInt retorno)
        {
            var statusConclusivos = new[] { 137, 138, 656 };

            if (statusConclusivos.Any(i => i == retorno.Retorno.cStat))
            {
                return;
            }

            throw new InvalidOperationException($"{retorno.Retorno.cStat} - {retorno.Retorno.xMotivo}");
        }

        private void SalvarDocumentosConsultados(RetornoNfeDistDFeInt dist, ISession session)
        {
            if (dist.Retorno.cStat != DOC_LOCALIZADO)
            {
                return;
            }

            var distribuicaoDfe = new DistribuicaoDFe
            {
                DocumentoUnicoInteressado = _emissor.EmissorFiscal.Empresa.DocumentoUnico,
                AmbienteSefaz = _emissor.Ambiente,
                MaiorNsu = dist.Retorno.maxNSU.ToString(),
                UltimoNsuPesquisado = dist.Retorno.ultNSU.ToString(),
                Xml = dist.RetornoCompletoStr ?? dist.RetornoStr
            };

            foreach (var docZip in dist.Retorno.loteDistDFeInt)
            {
                distribuicaoDfe.AddItem(docZip);
            }

            session.Persist(distribuicaoDfe);
            session.Flush();
        }

        public IList<string> ProcessarDocumentos()
        {
            var logs = new List<string>();
            var processador = new DistribuidorItemDfe(logs);

            processador.Processar(new EstrategiaResumoNfe(_emissor));
            processador.Processar(new EstrategiaNfeProc(_emissor));
            processador.Processar(new EstrategiaCancelamento());
            processador.Processar(new EstrategiaCartaCorrecao());

            if (!logs.Any())
                logs.Add("Nenhum documento foi processado.");

            return logs;
        }
    }
}