using System;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Fixos
{
    public class RClassificacaoFiscalDeComprasPorOperacao 
        : RelatorioBase
    {
        private int _empresaId;
        private DateTime _dataInicial;
        private DateTime _dataFinal;

        public RClassificacaoFiscalDeComprasPorOperacao(ISessaoManager sessaoManager) 
            : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrClassificacaoFiscalDeComprasPorOperacao.frx");
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

        protected override void PrepararDados()
        {
            RegistraParametro(
                "DescricaoRelatorio", 
                "Relatório de classificação fiscal de compras por operação"
            );

            RegistraParametro("EmpresaId", _empresaId);
            RegistraParametro("DataInicial", _dataInicial);
            RegistraParametro("DataFinal", _dataFinal);
        }
    }
}