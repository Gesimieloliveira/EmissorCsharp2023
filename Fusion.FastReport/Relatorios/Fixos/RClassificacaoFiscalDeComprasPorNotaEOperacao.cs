using System;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RClassificacaoFiscalDeComprasPorNotaEOperacao 
        : RelatorioBase
    {
        private int _empresaId;
        private DateTime _dataFinal;
        private DateTime _dataInicial;
        private string _codigoCfop;

        public RClassificacaoFiscalDeComprasPorNotaEOperacao(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrClassificacaoFiscalDeComprasPorNotaEOperacao.frx");
        }

        public void ComEmpresaId(int empresaId)
        {
            _empresaId = empresaId;
            MarcarComoJaConfigurado();
        }

        public void ComPeriodo(DateTime inicio, DateTime fim)
        {
            _dataInicial = inicio;
            _dataFinal = fim;
            MarcarComoJaConfigurado();
        }

        public void ComCodigoCfop(string cfop)
        {
            _codigoCfop = cfop;
        }

        protected override void PrepararDados()
        {
            RegistraParametro(
                "DescricaoRelatorio", 
                "Relatório de classificação fiscal de compras por nota e operação"
            );

            RegistraParametro("EmpresaId", _empresaId);
            RegistraParametro("DataInicial", _dataInicial);
            RegistraParametro("DataFinal", _dataFinal);
            RegistraParametro("CodigoCfop", _codigoCfop);
        }
    }
}