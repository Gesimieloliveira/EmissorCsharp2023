using System;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.TrocarEmpresa
{
    public class TrocarEmpresaViewModel : ViewModel
    {
        private readonly ISessaoManager _sessaoManager;

        public TrocarEmpresaViewModel(ISessaoManager sessaoManager)
        {
            _sessaoManager = sessaoManager;
        }

        public EmpresaComboBoxDTO EmpresaSelecionada
        {
            get => GetValue<EmpresaComboBoxDTO>();
            set => SetValue(value);
        }

        public event EventHandler<EmpresaDTO> SelecionouEmpresa;

        public void SelecionarEmpresa()
        {
            if (EmpresaSelecionada == null)
            {
                throw new InvalidOperationException("Preciso que selecione uma empresa");
            }

            var empresa = EmpresaSelecionada.CarregaEmpresa(_sessaoManager);

            SelecionouEmpresa?.Invoke(this, empresa);
        }
    }
}