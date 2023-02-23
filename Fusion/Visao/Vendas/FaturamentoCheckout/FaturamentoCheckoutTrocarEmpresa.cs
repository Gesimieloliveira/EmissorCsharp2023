using Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.TrocarEmpresa;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Vendas.FaturamentoCheckout
{
    public partial class FaturamentoCheckout
    {
        public void AcaoTrocarEmpresa()
        {
            if (ViewModel.PossuiFaturamento)
            {
                DialogBox.MostraAviso("Não é possível trocar a empresa, faturamento já está em andamento.");
                return;
            }

            //TODO: Atenção - Foi utilizado o mesmo no pedido (oragnizar faturamento)
            var childModel = new TrocarEmpresaViewModel(_sessaoSistema.SessaoManager);
            var childView = new TrocarEmpresaView(childModel);

            childView.ViewModel.SelecionouEmpresa += OnEmpresaSelecionada;

            void OnEmpresaSelecionada(object so, EmpresaDTO empresa)
            {
                ViewModel.AlterarEmpresaInicial(empresa);
            }

            AbrirChildWindow(childView);
        }
    }
}