using System;
using System.Collections.Generic;
using System.Linq;
using Fusion.Visao.Relatorios.Comum;
using Fusion.Visao.Relatorios.Opcoes;
using Fusion.Visao.Relatorios.Opcoes.Sistema;
using FusionCore.Debug;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Relatorios;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Relatorios
{
    public class ListagemRelatoriosContexto : ContextoObservado<ListagemRelatoriosContexto>
    {
        private readonly ISessaoManager _sessaoManager;

        public ListagemRelatoriosContexto(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;

            Relatorios = new List<IOpcaoRelatorio>();
        }

        public IOpcaoRelatorio ItemSelecionado
        {
            get => GetValue<IOpcaoRelatorio>();
            set => SetValue(value);
        }

        public IEnumerable<IOpcaoRelatorio> Relatorios
        {
            get => GetValue<IEnumerable<IOpcaoRelatorio>>();
            set => SetValue(value);
        }

        public bool ModoDesigner
        {
            get => GetValue<bool>();
            private set => SetValue(value);
        }

        public bool ModoDesenvolvimento => BuildMode.IsHomologacao;

        public string StringFiltro
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public void ListarRelatorios()
        {
            var opcoes = ListarOpcoesDeRelatorios();

            if (string.IsNullOrWhiteSpace(StringFiltro))
            {
                Relatorios = opcoes;
                return;
            }

            Relatorios = opcoes.Where(
                i => i.Descricao.ToLower().Contains(StringFiltro.ToLower())
            );
        }

        private IEnumerable<IOpcaoRelatorio> ListarOpcoesDeRelatorios()
        {
            var opcoes = new List<IOpcaoRelatorio>();

            AdicionarRelatoriosFastReport(ref opcoes);
            AdicionarRelatoriosFixos(ref opcoes);
            AdicionarRelatoriosSistemaEditaveis(ref opcoes);
            AdicionarRelatoriosEditaveisEmDesenvolvimento(ref opcoes);

            return opcoes.OrderBy(i => i.Grupo.RemoverAcentos()).ThenBy(d => d.Descricao).ToList();

        }

        private void AdicionarRelatoriosEditaveisEmDesenvolvimento(ref List<IOpcaoRelatorio> opcoes)
        {
            if (!ModoDesenvolvimento || !ModoDesigner) return;
            
            opcoes.Add(new OpcaoImpressaoDeLinhaTef());
            opcoes.Add(new OpcaoListagemDeCaixasFechados());
            opcoes.Add(new OpcaoDanfeNfce());
            opcoes.Add(new OpcaoDanfeNfce58mm());
            opcoes.Add(new OpcaoDanfeNfceA4());
            opcoes.Add(new OpcaoPromissoriaCarne());
            opcoes.Add(new OpcaoPromissoria());
        }

        private void AdicionarRelatoriosSistemaEditaveis(ref List<IOpcaoRelatorio> opcoes)
        {
            if (!ModoDesigner) return;

            opcoes.Add(new OpcaoImpressaoPedidoVenda());
            opcoes.Add(new OpcaoImpressaoFaturamentoA4());
            opcoes.Add(new OpcaoImpressaoFaturamento80());
        }

        private void AdicionarRelatoriosFixos(ref List<IOpcaoRelatorio> opcoes)
        {
            opcoes.Add(new OpcaoAnaliseLucroPorItem());
            opcoes.Add(new OpcaoCompraDetalhada());
            opcoes.Add(new OpcaoContagemDeEstoque());
            opcoes.Add(new OpcaoInutilizacoesNfeNfce());
            opcoes.Add(new OpcaoItensVendidos());
            opcoes.Add(new OpcaoIventarioEstoque());
            opcoes.Add(new OpcaoListagemDeProdutosEstoque());
            opcoes.Add(new OpcaoListagemProdutoTributacao());
            opcoes.Add(new OpacaoVendasTransmitidasNaNfce());
            opcoes.Add(new OpcaoRelatorioAniversariantes());
            opcoes.Add(new OpcaoVendasDoFaturamento());
            opcoes.Add(new OpcaoModeloEtiqueta());
            opcoes.Add(new OpcaoRelatorioFiscalNFeNFce());
            opcoes.Add(new OpcaoListagemDeEstoquePorGrupo());
            opcoes.Add(new OpcaoVendasComItensPorUsuarios());
            opcoes.Add(new OpcaoEstoqueReposicao());
            opcoes.Add(new OpacaoRelatorioFiscalEmissaoCTe());
            opcoes.Add(new OpcaoRelatorioFiscalComprasPorOperacao());
            opcoes.Add(new OpcaoRelatorioFiscalComprasPorNotaEOperacao());
            opcoes.Add(new OpcaoTotalVendidoPorVendedor());
            opcoes.Add(new OpcaoDocumentoPagar());
            opcoes.Add(new OpcaoDocumentoReceber());
            opcoes.Add(new OpcaoRValorPorNcmAgrupadoPorIcms());
            opcoes.Add(new OpcaoRValorPorNcmAgrupadoPorPisCofins());
            opcoes.Add(new OpcaoRelaotorioCompletoVendasPorModelo());
            opcoes.Add(new OpcaoVendasAgrupadoPorCliente());
            opcoes.Add(new OpcaoItensVendidosAgrupadoPorCliente());
            opcoes.Add(new OpcaoTotaisPorFormaPagamento());
            opcoes.Add(new OpcaoClientesPorEnderecoCidadeContato());
            opcoes.Add(new OpcaoProdutosComCodigoDeBarras());
            opcoes.Add(new OpcaoProdutoComNcmVencido());
            opcoes.Add(new OpcaoNcmVencido());
            opcoes.Add(new OpcaoRelatorio("Etiquetas", "Etiqueta 3 colunas 32x17mm", "FrEtiqueta3Colunas32x17mm"));
            opcoes.Add(new OpcaoRelatorio("Etiquetas", "Etiqueta gondola 100x30mm", "FrEtiquetaGondola100x30mm"));
            opcoes.Add(new OpcaoRelatorio("Análises", "Relatório de itens vendidos no faturamento agrupados por cliente", "FrItensVendidosFaturamentoAgrupadosPorCliente"));
            opcoes.Add(new OpcaoRelatorio("Análises", "Relatório de comissão de vendedores", "FrComissaoDeVendedores"));
            opcoes.Add(new OpcaoRelatorio("Análises", "Relatório de tempo da última compra pelos clientes", "FrRelatorioDeTempoDaUltimaCompraPelosClientes"));
            opcoes.Add(new OpcaoRelatorio("Análises", "Relatório de Operações Autorizadas", "FrRelatorioOperacoesAutorizadas"));
            opcoes.Add(new OpcaoRelatorio("Análises", "Relatório de CTE por período", "FrRelatorioCtePorPeriodo"));
            opcoes.Add(new OpcaoRelatorio("Análises", "Relatório análise lucro de item detalhado", "FrAnaliseLucroItemDetalhado"));
        }

        private void AdicionarRelatoriosFastReport(ref List<IOpcaoRelatorio> opcoes)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repo = new RepositorioRelatorio(sessao);
                var relatorios = repo.ObtemTodos();

                opcoes.AddRange(relatorios.Select(r => new OpcaoRelatorioProprio(r, _sessaoManager)));
            }
        }

        public void SalvarTemplate(Guid guid, byte[] template)
        {
            using (var sessao = _sessaoManager.CriaSessao())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioRelatorio(sessao);

                repositorio.SalvarTemplate(new Template(guid, template));
                transacao.Commit();
            }
        }

        public void AtivarModoDesigner()
        {
            ModoDesigner = true;
            ListarRelatorios();
        }

        public void DesativarModoDesigner()
        {
            ModoDesigner = false;
            ListarRelatorios();
        }
    }
}