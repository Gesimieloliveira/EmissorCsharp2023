using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Mail;
using System.Windows.Input;
using Fusion.Sessao;
using Fusion.Visao.ConverterVendaParaNfe;
using Fusion.Visao.PedidoDeVenda.Servicos;
using Fusion.Visao.Pessoa.Picker;
using FusionCore.ControleCaixa.Facades;
using FusionCore.FusionAdm.Componentes;
using FusionCore.FusionAdm.ConverterVendaParaNfe;
using FusionCore.FusionAdm.PedidoVenda;
using FusionCore.FusionAdm.PedidoVenda.Fabricas;
using FusionCore.FusionAdm.PedidoVenda.Servicos.Converter;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Servico.Estoque;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Basico;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas.PedidoDeVenda;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.ConfiguracaoEmail;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Sessao;
using FusionCore.Vendas.Faturamentos;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.Utils.Dialogs;
using FusionWPF.SendMail;
using NHibernate;
using NHibernate.Util;

namespace Fusion.Visao.PedidoDeVenda.Lista
{
    public sealed class OpcoesPedidoVendaModel : ViewModel
    {
        private readonly UsuarioDTO _usuario = SessaoSistema.Instancia.UsuarioLogado;

        public event EventHandler<FaturamentoVenda> OnConverteu; 

        public OpcoesPedidoVendaModel(PedidoVendaDTO pedidoVenda)
        {
            PedidoSelecionado = pedidoVenda;
        }

        public PedidoVendaDTO PedidoSelecionado { get; }

        public ICommand CommandConverterParaNFe => GetSimpleCommand(ConverterParaNFeAction);
        public ICommand CommandConverterParaFaturamento => GetSimpleCommand(ConverterParaFaturamentoAction);

        private void ConverterParaFaturamentoAction(object obj)
        {
            try
            {
                _usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.CONVERTER_PEDIDO_PARA_FATURAMENTO);
                ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_usuario);

                const string msg = "Deseja realmente converter esse pedido de venda/orçamento em faturamento e faturar o mesmo já?";

                if (!DialogBox.MostraDialogoDeConfirmacao(msg))
                {
                    return;
                }

                var pedidoVenda = FetchPedidoVenda();
                pedidoVenda.ValidarParaFinalizacao();

                ChecaEstoqueNegativo(pedidoVenda);

                var faturamento = new ConvertePedidoDeVendaParaFaturamento(pedidoVenda, _usuario).Executar();

                OnOnConverteu(faturamento);

                OnFechar();
            }
            catch (Exception e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private static void ChecaEstoqueNegativo(PedidoVenda pedidoVenda)
        {
            foreach (var pedidoVendaProduto in pedidoVenda.ItensPedidoVenda)
            {
                BoqueioEstoqueHelper.ChecaEstoqueNegativoAdm(pedidoVendaProduto.Produto, pedidoVendaProduto.Quantidade);
            }
        }

        private void ConverterParaNFeAction(object obj)
        {
            try
            {
                _usuario.VerificaPermissao.IsTemPermissaoThrow(Permissao.CONVERTER_PEDIDO_PARA_NFE);

                ControleCaixaGestorFacade.ThrowExcetpionSeNaoExistirCaixaAberto(_usuario);

                var pedido = FetchPedidoVenda();

                pedido.ValidarParaFinalizacao();

                Cliente cliente = null;

                if (pedido.NaoTemCliente)
                {
                    var modelClientePicker = new PessoaPickerModel(new ClienteEngine());
                    modelClientePicker.PickItemEvent += delegate(object sender, GridPickerEventArgs args)
                    {
                        cliente = args.GetItem<Cliente>();
                    };

                    modelClientePicker.GetPickerView().ShowDialog();

                    cliente?.ThrowNaoPossuiEndereco();

                    if (cliente == null)
                        throw new InvalidOperationException("Selecione um cliente para converter o pedido em NF-e");
                }

                ChecaEstoqueNegativo(pedido);

                var conversor = new ConvertePedidoDeVendaParaNFe(pedido,
                    SessaoSistema.Instancia.SessaoManager,
                    SessaoSistema.Instancia.UsuarioLogado,
                    cliente);

                var viewModel = new ConverterParaNfeFormModel(conversor);
                var view = new ConverterParaNfeForm(viewModel);

                view.ShowDialog();

                OnFechar();
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        private PedidoVenda FetchPedidoVenda()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = FactoryRepositorioPedidoVenda.CriaRepositorio(sessao);
                return repositorio.GetPeloIdLazy(PedidoSelecionado.Id);
            }
        }

        public void EnviarPorEmail()
        {
            var tipoPedido = PedidoSelecionado.TipoPedido == TipoPedido.Orcamento ? "Orçamento" : "Pedido de Venda";

            var behavior = new EnvioEmailBehavior();

            behavior.DespacharEmails += DespacharEmailsHandler;
            behavior.Assunto = tipoPedido;
            behavior.CorpoMensagem = $"Segue em anexo o {tipoPedido}";

            if (PedidoSelecionado.IdCliente != 0)
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioPessoa(sessao);
                    var emails = repositorio.BuscarEmailsPelaPessoaId(PedidoSelecionado.IdCliente);

                    behavior.Emails = new ObservableCollection<Email>(emails);
                }
            }
            

            new EnvioEmailView(behavior).ShowDialog();
        }

        private void DespacharEmailsHandler(object sender, IEnumerable<Email> emails)
        {
            if (!(sender is EnvioEmailBehavior behavior))
            {
                return;
            }

            var tipoPedido = PedidoSelecionado.TipoPedido == TipoPedido.Orcamento ? "Orçamento" : "Pedido de Venda";


            try
            {
                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var documento = GerarDocumento();

                    var emailBuilder = new EmailBuilder(GetConfigEmail(sessao));

                    emailBuilder.Assunto(behavior.Assunto)
                        .AddAnexo(documento, $"{tipoPedido} - Número: {PedidoSelecionado.Id:0000}.pdf")
                        .Mensagem(behavior.CorpoMensagem);

                    emails.ForEach(e => emailBuilder.AddEmail(e));

                    emailBuilder.Enviar();
                }
            }
            catch (SmtpException)
            {
                throw new InvalidOperationException("Não foi possível conectar no SMTP de email");
            }
        }

        private MemoryStream GerarDocumento()
        {
            var impressor = new ImpressorPedidoVenda(new SessaoManagerAdm());
            var pdf = impressor.GerarPdf(PedidoSelecionado.Id);

            return pdf;
        }

        private static ConfiguracaoEmailDTO GetConfigEmail(ISession sessao)
        {
            var repositorio = new RepositorioComun<ConfiguracaoEmailDTO>(sessao);
            return repositorio.Busca(new UnicaConfiguracaoEmail());
        }

        private void OnOnConverteu(FaturamentoVenda e)
        {
            OnConverteu?.Invoke(this, e);
        }
    }
}