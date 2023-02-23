using System;
using FusionCore.Sessao;

namespace Fusion.FastReport.Relatorios.Sistema.Caixa
{
    public class RListagemDeCaixasFechados : RelatorioBase
    {
        private bool _iniciarComPeriodo;
        private DateTime _periodoInicio;
        private DateTime _periodoFim;

        public RListagemDeCaixasFechados(ISessaoManager sessaoManager) : base(sessaoManager)
        {
        }

        protected override byte[] FornecerTemplate()
        {
            return FornecedorTemplate.ObtemBytesFrx("FrListagemDeCaixasFechados.frx");
        }

        public void InformarPeriodo(DateTime inicio, DateTime fim)
        {
            _iniciarComPeriodo = true;
            _periodoInicio = inicio;
            _periodoFim = fim;
        }

        protected override void PrepararDados()
        {
            if (_iniciarComPeriodo)
            {
                RegistraParametro("PIniciarComPeriodo", true);
                RegistraParametro("PDataInicial", _periodoInicio);
                RegistraParametro("PDataFinal", _periodoFim);
            }
        }
    }
}