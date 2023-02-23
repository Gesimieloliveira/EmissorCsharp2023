using System;
using System.Linq;
using System.Windows;
using Fusion.Sessao;
using FusionCore.Core.Flags;
using FusionCore.FusionAdm.Fiscal.NF.Pagamentos;
using FusionCore.Helpers.Basico;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionWPF.Controles;
using FusionWPF.Parcelamento;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.NotaFiscalEletronica.Principal.Finalizar.MeioPagamento
{
    public class OpcaoPrazo : IOpcaoPagamento
    {
        private readonly FusionWindow _window;
        private readonly ChildWindow _parentChild;
        private readonly IParcelamentoFactory _factory;
        private readonly UsuarioDTO _usuario = SessaoSistema.ObterUsuarioLogado();

        public OpcaoPrazo(FusionWindow window, ChildWindow parentChild, IParcelamentoFactory factory)
        {
            _window = window;
            _parentChild = parentChild;
            _factory = factory;
        }

        public string Descricao { get; } = ETipoPagamento.CreditoLoja.GetDescription();

        public void PagarAsync(decimal valor, Action<Resultado> callback)
        {
            Resultado resultado = null;
            var dialog = _factory.CriaDialog(valor);

            dialog.Contexto.ParceladoComSucesso += (sender, args) =>
            {
                var parcelas = args.Parcelas
                    .Select(p => new ParcelaNfe(p.Numero, p.Vencimento, p.Valor))
                    .ToList();

                var prazo = new Aprazo(_usuario, args.TipoDocumento, parcelas);

                resultado = Resultado.Sucesso(prazo);
            };

            dialog.Closing += (sender, args) =>
            {
                _parentChild.Visibility = Visibility.Visible;
                callback(resultado);
            };

            _parentChild.Visibility = Visibility.Hidden;
            _window.ShowChildWindowAsync(dialog);
        }
    }
}