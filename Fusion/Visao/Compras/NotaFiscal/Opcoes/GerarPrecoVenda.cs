using Fusion.Visao.Compras.Precos;
using FusionWPF.Base.Utils.Dialogs;

namespace Fusion.Visao.Compras.NotaFiscal.Opcoes
{
    public class GerarPrecoVenda : IOutraOpcao
    {
        public string Titulo { get; } = "Gerar preço de venda";
        public bool IsVisible { get; } = true;

        public void ExeuctaAcao(NotaFiscalCompraViewModel compraVm)
        {
            if (!compraVm.NotaTemItens)
            {
                DialogBox.MostraAviso("Necessário itens na nota para poder gerar o preço de venda");
                return;
            }

            var vm = new GeradorPrecoVendaViewModel(compraVm.GetNota());
            new GeradorPrecoVendaView(vm).ShowDialog();
        }
    }
}