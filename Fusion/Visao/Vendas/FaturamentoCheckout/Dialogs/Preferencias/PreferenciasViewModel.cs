using System;
using System.Collections.Generic;
using FusionCore.Helpers.Maquina;
using FusionCore.Impressoras;
using FusionCore.Vendas.Faturamentos;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Preferencias
{
    public class PreferenciasViewModel : ViewModel
    {
        private readonly PreferenciasFaturamentoFacade _preferenciaFacade;
        private FaturamentoPreferencia _preferencia;
        private LayoutImpressaoPormissoria _layoutImpressaoPromissoria = LayoutImpressaoPormissoria.NaoImprimir;

        public PreferenciasViewModel(PreferenciasFaturamentoFacade preferenciaFacade)
        {
            ImpressorasDisponiveis = new List<string>();
            _preferenciaFacade = preferenciaFacade;
        }

        public IList<string> ImpressorasDisponiveis
        {
            get => GetValue<IList<string>>();
            private set => SetValue(value);
        }

        public string ImpressoraSelecionada
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public LayoutImpressao LayoutImpressao
        {
            get => GetValue<LayoutImpressao>();
            set => SetValue(value);
        }

        public bool ImprimirFinalizacao
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool ImprimirCupom
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool PreVisualizar
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool VisualizarCupom
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool DesativarTelaOpcoes
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool ImprimeDuasVias
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public LayoutImpressaoPormissoria LayoutImpressaoPromissoria
        {
            get => _layoutImpressaoPromissoria;
            set
            {
                if (value == _layoutImpressaoPromissoria) return;
                _layoutImpressaoPromissoria = value;
                PropriedadeAlterada();
            }
        }

        public void Inicializar()
        {
            ImpressorasDisponiveis.Clear();
            _preferencia = _preferenciaFacade.GetPreferenciaDaMaquina();

            foreach (var printer in Impressora.ObterImpressorasDoComputador())
            {
                ImpressorasDisponiveis.Add(printer);
            }

            ImpressoraSelecionada = _preferencia?.Impressora;
            ImprimirFinalizacao = _preferencia?.ImprimirFinalizacao ?? true;
            ImprimirCupom = _preferencia?.ImprimirCupom ?? true;
            PreVisualizar = _preferencia?.PreVisualizar ?? true;
            VisualizarCupom = _preferencia?.VisualizarCupom ?? true;
            DesativarTelaOpcoes = _preferencia?.DesabilitarTelaOpcoes ?? false;
            ImprimeDuasVias = _preferencia?.ImprimeDuasVias ?? false;
            LayoutImpressao = _preferencia?.LayoutImpressao ?? LayoutImpressao.Impressao80M;
            LayoutImpressaoPromissoria = _preferencia?.LayoutImpressaoPromissoria ??
                                         LayoutImpressaoPormissoria.NaoImprimir;
        }

        public void SalvaPreferencias()
        {
            if (ImpressoraSelecionada == null)
            {
                throw new InvalidOperationException("Preciso que informe uma impressora");
            }

            if (_preferencia == null)
            {
                _preferencia = new FaturamentoPreferencia(IdMaquinaProvider.Computa());
            }

            _preferencia.Impressora = ImpressoraSelecionada;
            _preferencia.LayoutImpressao = LayoutImpressao;
            _preferencia.PreVisualizar = PreVisualizar;
            _preferencia.ImprimeDuasVias = ImprimeDuasVias;
            _preferencia.LayoutImpressaoPromissoria = LayoutImpressaoPromissoria;
            _preferencia.DesabilitarTelaOpcoes = DesativarTelaOpcoes;
            _preferencia.ImprimirFinalizacao = ImprimirFinalizacao;
            _preferencia.ImprimirCupom = ImprimirCupom;
            _preferencia.VisualizarCupom = VisualizarCupom;

            _preferenciaFacade.Salva(_preferencia);
        }
    }
}