using System.Windows;
using Fusion.Base.Notificacoes;
using Fusion.Sessao;
using Fusion.Visao.DocumentoAPagar;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.Utils.Dialogs;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Compras.NotaFiscal.Opcoes
{
    public class GerarDocumentosApagar : IOutraOpcao
    {
        private readonly Window _currentWindow;

        public GerarDocumentosApagar(Window currentWindow)
        {
            _currentWindow = currentWindow;
        }

        public string Titulo { get; } = "Gerar documentos a pagar";
        public bool IsVisible { get; } = SessaoSistema.Instancia?.AcessoConcedido.PossuiFusionGestor == true;

        public void ExeuctaAcao(NotaFiscalCompraViewModel compraVm)
        {
            if (compraVm.NotaTemItens == false)
            {
                DialogBox.MostraAviso("Necessário lançar itens na nota antes.");
                return;
            }

            if (compraVm.PossuiFinanceiro)
            {
                DialogBox.MostraAviso("Nota já possui documentos a pagar gerados.");
                return;
            }

            var nota = compraVm.GetNota();
            var model = GerarContasPagarModelFactory.Criar(new Notificador(), nota);

            model.AntesComitarDelegate = (sessao, malote) =>
            {
                nota.Malote = malote;
                var repositorio = new RepositorioNotaFiscalCompra(sessao);
                repositorio.Salvar(nota);
            };

            var dialog = new GerarContasPagar(model);
            _currentWindow.ShowChildWindowAsync(dialog);
        }
    }
}