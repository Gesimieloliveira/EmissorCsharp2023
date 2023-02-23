using System;
using System.Collections.Generic;
using System.Windows;
using Fusion.Sessao;
using FusionCore.Core.Flags;
using FusionCore.Helpers.Basico;
using FusionCore.Vendas.Faturamentos;
using FusionWPF.Controles;
using FusionWPF.Parcelamento;
using MahApps.Metro.SimpleChildWindow;

namespace Fusion.Visao.Vendas.FaturamentoCheckout.Dialogs.Finalizacao.MeioPagamento
{
    public class OpcaoPrazo : IOpcaoPagamento
    {
        private readonly FusionWindow _window;
        private readonly IParcelamentoFactory _factory;
        private readonly ChildWindow _parentChild;

        public OpcaoPrazo(FusionWindow window, ChildWindow parentChild, IParcelamentoFactory factory)
        {
            _window = window;
            _factory = factory;
            _parentChild = parentChild;
        }

        public string Descricao { get; } = ETipoPagamento.CreditoLoja.GetDescription();

        public void PagarAsync(decimal valor, Action<Resultado> callback)
        {
            Resultado resultado = null;

            var dialog = _factory.CriaDialog(valor);

            dialog.Contexto.ParceladoComSucesso += (sender, args) =>
            {
                var parcelas = new List<FParcela>();
                var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

                foreach (var p in args.Parcelas)
                {
                    parcelas.Add(new FParcela(p.Numero, p.Vencimento, p.Valor));
                }

                var prazo = FPagamento.CriarNoPrazo(usuarioLogado, args.TipoDocumento, parcelas);

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

        public bool PermiteTroco()
        {
            return false;
        }
    }
}