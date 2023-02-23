using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.PedidoVenda.Preferencias;
using FusionCore.Helpers.Maquina;
using FusionCore.Impressoras;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.PedidoDeVenda.Preferencias
{
    public class PedidoVendaPreferenciaFormModel : ViewModel
    {
        private bool _imprimeAposFinalizar;
        private PedidoVendaPreferencia _preferencia;
        private readonly PedidoVendaPreferenciaFacade _facade;
        private bool _visualizarAposFinalizar;
        private bool _imprimeDuasVias;

        public PedidoVendaPreferenciaFormModel()
        {
            ImpressorasDisponiveis = new List<string>();
            _facade = new PedidoVendaPreferenciaFacade();
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

        public bool ImprimeAposFinalizar
        {
            get => _imprimeAposFinalizar;
            set
            {
                if (value == _imprimeAposFinalizar) return;
                _imprimeAposFinalizar = value;
                PropriedadeAlterada();
            }
        }


        public void Inicializa()
        {
            ImpressorasDisponiveis.Clear();

            foreach (var impressoraNome in Impressora.ObterImpressorasDoComputador())
            {
                ImpressorasDisponiveis.Add(impressoraNome);
            }

            _preferencia = _facade.GetPreferenciaDaMaquina() ?? new PedidoVendaPreferencia(IdMaquinaProvider.Computa());

            ImpressoraSelecionada = _preferencia.Impressora;
            ImprimeAposFinalizar = _preferencia.ImprimeAposFinalizar;
            VisualizarAposFinalizar = _preferencia.VisualizarAposFinalizar;
            ImprimeDuasVias = _preferencia.ImprimeDuasVias;
        }

        public bool ImprimeDuasVias
        {
            get => _imprimeDuasVias;
            set
            {
                if (value == _imprimeDuasVias) return;
                _imprimeDuasVias = value;
                PropriedadeAlterada();
            }
        }

        public bool VisualizarAposFinalizar
        {
            get => _visualizarAposFinalizar;
            set
            {
                if (value == _visualizarAposFinalizar) return;
                _visualizarAposFinalizar = value;
                PropriedadeAlterada();
            }
        }

        public void Salvar()
        {
            if (ImpressoraSelecionada == null)
            {
                throw new InvalidOperationException("Preciso que informe uma impressora");
            }

            _preferencia.Impressora = ImpressoraSelecionada;
            _preferencia.ImprimeAposFinalizar = ImprimeAposFinalizar;
            _preferencia.ImprimeDuasVias = ImprimeDuasVias;
            _preferencia.VisualizarAposFinalizar = VisualizarAposFinalizar;

            _facade.Salvar(_preferencia);

            OnFechar();
        }
    }
}