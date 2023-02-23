using System;
using System.Collections.Generic;
using System.Linq;
using Fusion.Sessao;
using Fusion.Visao.Pessoa.Picker.OpcoesBusca;
using FusionCore.FusionAdm.Pessoas;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionLibrary.Helper.Diversos;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;
using FusionWPF.Base.GridPicker.OpcoesBuscas;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;
using static System.String;

namespace Fusion.Visao.Pessoa.Picker
{
    public class PessoaPickerModel : GridPickerModel
    {
        private readonly IPessoaPickerEngine _engine;

        public PessoaPickerModel(IPessoaPickerEngine engine)
        {
            _engine = engine;
            TipoBuscas.Add(new BuscaPessoaPickerPadrao());
            TipoBuscas.Add(new BuscaPessoaPickerPorEmail());
            TipoBuscas.Add(new BuscaPessoaPickerPorTelefone());
            TipoBuscas.Add(new BuscaPessoaPickerPorDocumento());
            TipoBuscas.Add(new BuscaPessoaPickerPorNome());
            TipoBuscas.Add(new BuscaPessoaSelecionaPorEndereco());
            TipoBuscas.Add(new BuscaPessoaPickerVendedor());

            BuscaSelecionada = TipoBuscas[0];
            IsTemTipoBuscas = true;

            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;
            var isPessoaInserirOuAlterar = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.CADASTRO_PESSOA_INSERIR_ALTERAR);
            HabilitaBotaoNovo = isPessoaInserirOuAlterar;
            HabilitaBotaoEditar = isPessoaInserirOuAlterar;
        }

        protected override void OnNovoRegistro()
        {
            var viewModel = new PessoaFormModel();
            viewModel.RegistroSalvo += PessoaSalvaHandler;

            new PessoaForm(viewModel).ShowDialog();
        }

        private void PessoaSalvaHandler(object sender, PessoaEntidade pessoa)
        {
            try
            {
                pessoa.ThrowInvalidOperationInativa();

                OnPickItemEvent(_engine.ConverterItem(pessoa));
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        protected override void OnEditarRegistro(IGridPickerItem item)
        {
            var pessoaGrid = item.GetItemReal<PessoaGrid>();
            var viewModel = new PessoaFormModel(pessoaGrid.Id);
            viewModel.RegistroSalvo += PessoaSalvaHandler;

            new PessoaForm(viewModel).ShowDialog();
        }

        protected override void OnPickItem(IGridPickerItem item)
        {
            try
            {
                OnPickItemEvent(_engine.ConverterItemPicked(item));
            }
            catch (InvalidOperationException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
            catch (InvalidCastException e)
            {
                DialogBox.MostraAviso(e.Message);
            }
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();
            ItemSelecionado = null;
            IList<PessoaGrid> pessoas;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                pessoas = BuscaSelecionada.Listar<PessoaGrid>(input, sessao);
            }

            pessoas?.ForEach(AddPessoaLista);
        }

        private void AddPessoaLista(PessoaGrid pessoa)
        {
            var item = new GridPickerItem
            {
                Titulo = pessoa.Nome,
                Subtitulo = GetExtensao(pessoa),
                Coluna1 = $"#Id: {pessoa.Id}",
                Coluna2 = ExtraiDocumentoUnico(pessoa),
                Coluna3 = ExtraiDocumentoAuxiliar(pessoa),
                ItemReal = pessoa
            };

            ItensLista.Add(item);
        }

        private static string GetExtensao(PessoaGrid pessoa)
        {
            var extensao = "Pessoa";

            if (pessoa.IsCliente)
            {
                extensao = Concat(extensao, ", Cliente");
            }

            if (pessoa.IsTransportadora)
            {
                extensao = Concat(extensao, ", Transportadora");
            }

            if (pessoa.IsFornecedor)
            {
                extensao = Concat(extensao, ", Fornecedor");
            }

            if (pessoa.IsVendedor)
            {
                extensao = Concat(extensao, ", Vendedor");
            }

            return extensao;
        }

        private static string ExtraiDocumentoUnico(PessoaGrid pessoa)
        {
            switch (pessoa.Tipo)
            {
                case PessoaTipo.Fisica:
                    return IsNullOrWhiteSpace(pessoa.Cpf)
                        ? Empty
                        : $"CPF: {pessoa.Cpf.FormataParaCpf()}";
                case PessoaTipo.Juridica:
                    return IsNullOrWhiteSpace(pessoa.Cnpj)
                        ? Empty
                        : $"CNPJ: {pessoa.Cnpj?.FormataParaCnpj()}";
                case PessoaTipo.Extrangeiro:
                    return IsNullOrWhiteSpace(pessoa.DocumentoExtrangeiro)
                        ? Empty
                        : $"Doc. Extrangeiro: {pessoa.DocumentoExtrangeiro}";
            }

            return Empty;
        }

        private static string ExtraiDocumentoAuxiliar(PessoaGrid pessoa)
        {
            var documentos = new List<string>();

            if (pessoa.IsFisica && !IsNullOrWhiteSpace(pessoa.Rg))
            {
                documentos.Add($"RG: {pessoa.Rg}");
            }

            if (!IsNullOrWhiteSpace(pessoa.InscricaoEstadual))
            {
                documentos.Add($"IE: {pessoa.InscricaoEstadual}");
            }

            return Join(" / ", documentos).Trim();
        }

        public void UsarBusca<T>() where T : IOpcaoBusca
        {
            BuscaSelecionada = TipoBuscas.FirstOrDefault(i => i.GetType() == typeof(T));
        }
    }
}