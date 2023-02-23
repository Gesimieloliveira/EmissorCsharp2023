using System;
using Fusion.Sessao;
using Fusion.Visao.Pessoa;
using FusionCore.Excecoes.RegraNegocio;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.Veiculos
{
    public class ProprietarioVeiculoPickerModel : GridPickerModel
    {
        public ProprietarioVeiculoPickerModel()
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            var isPermissaoInserirAlterarPessoa =
                usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PESSOA_INSERIR_ALTERAR);

            HabilitaBotaoNovo = isPermissaoInserirAlterarPessoa;
            HabilitaBotaoEditar = isPermissaoInserirAlterarPessoa;
        }

        protected override void OnNovoRegistro()
        {
            var vm = new PessoaFormModel();
            new PessoaForm(vm).ShowDialog();

            AplicaPesquisa(TextoPesquisado);
        }

        protected override void OnPickItem(IGridPickerItem item)
        {
            try
            {
                var selecionado = item.GetItemReal<ProprietarioVeiculo>();

                using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
                {
                    var repositorio = new RepositorioPessoa(sessao);

                    if (!repositorio.EhUmaTransportadora(selecionado.Id))
                    {
                        throw new InvalidOperationException("Proprietário precisa ser uma Transportadora!");
                    }
                }

                OnPickItemEvent(new GridPickerEventArgs(item));
            }
            catch (RegraNegocioException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        protected override void OnEditarRegistro(IGridPickerItem item)
        {
            if (!(item.ItemReal is ProprietarioVeiculo dono))
            {
                return;
            }

            var vm = new PessoaFormModel(dono.Id);
            vm.RegistroSalvo += (s, e) => { OnPickItem(item); };

            new PessoaForm(vm).ShowDialog();
        }

        protected override void OnInicializar()
        {
            ItensLista.Clear();
            ItemSelecionado = null;
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioVeiculo(sessao);
                var proprietarios = repositorio.BuscaProprietarios(input);

                proprietarios.ForEach(AdicionaNaLista);
            }
        }

        private void AdicionaNaLista(ProprietarioVeiculo proprietario)
        {
            var item = new GridPickerItem
            {
                Titulo = proprietario.Nome,
                Subtitulo = proprietario.IsTransportadora ? "Transportadora" : "",
                Coluna1 = proprietario.DocumentoUnico,
                ItemReal = proprietario
            };

            ItensLista.Add(item);
        }
    }
}