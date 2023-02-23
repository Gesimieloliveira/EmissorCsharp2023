using System;
using System.Collections.Generic;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Fiscal.NF.ConstularStatus;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.NotaFiscalEletronica.Status
{
    public class ConsultaStatusSefazFormModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;

        public ConsultaStatusSefazFormModel(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public EmissorFiscal EmissorSelecionado
        {
            get => GetValue<EmissorFiscal>();
            set => SetValue(value);
        }

        public IEnumerable<EmissorFiscal> ListaDeEmissores
        {
            get => GetValue<IEnumerable<EmissorFiscal>>();
            set => SetValue(value);
        }

        public void CarregarDadosIniciais()
        {
            using (var sessao = _sessaoManager.CriaSessao())
            {
                var repositorio = new RepositorioEmissorFiscal(sessao);
                var emissores = repositorio.BuscaTodosNfe();

                ListaDeEmissores = emissores;
            }
        }

        public RetornoConsultaStatus VerificarStatus()
        {
            ThrowExceptionSeModeloInvalido();

            var consultador = new ConsultarStatusZeus(EmissorSelecionado.EmissorFiscalNfe);
            var consultaStatus = consultador.ConsultarStatus();

            return consultaStatus;
        }

        private void ThrowExceptionSeModeloInvalido()
        {
            if (EmissorSelecionado == null)
            {
                throw new InvalidOperationException("Preciso de um Emissor Fiscal");
            }
        }
    }
}