using System.Collections.Generic;
using System.IO;
using Fusion.Sessao;
using Fusion.Visao.Pessoa.SubFormularios;
using FusionCore.FusionAdm.Automoveis;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.FusionAdm;
using FusionWPF.Base.GridPicker;
using FusionWPF.Base.GridPicker.Contrato;
using NHibernate.Util;

namespace Fusion.Visao.Veiculos
{
    public class VeiculoPickerModel : GridPickerModel
    {
        public VeiculoPickerModel()
        {
            var usuarioLogado = SessaoSistema.Instancia.UsuarioLogado;

            var isPermissaoGerenciarVeiculo = usuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_VEICULO);

            HabilitaBotaoNovo = isPermissaoGerenciarVeiculo;
            HabilitaBotaoEditar = isPermissaoGerenciarVeiculo;
        }

        protected override void OnNovoRegistro()
        {
            var vm = new VeiculoFormModel();
            vm.RegistroSalvo += (s, v) => OnPickItem(new GridPickerItem {ItemReal = v});

            new VeiculoForm(vm).ShowDialog();
        }

        protected override void OnEditarRegistro(IGridPickerItem item)
        {
            var vm = new VeiculoFormModel(item.GetItemReal<Veiculo>());
            vm.RegistroSalvo += (s, v) => OnPickItem(new GridPickerItem {ItemReal = v});

            new VeiculoForm(vm).ShowDialog();
        }

        private void PreencherListaCom(IList<Veiculo> veiculos)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            if (veiculos == null)
            {
                throw new InvalidDataException("Nenhum veiculo foi encontrado");
            }

            veiculos.ForEach(AddVeiculoLista);
        }

        private void AddVeiculoLista(Veiculo veiculo)
        {
            //TOOD: Corrige coluna com o proprietario/dono do veiculo na picker

            ItensLista.Add(new GridPickerItem
            {
                Titulo = veiculo.Descricao,
                Coluna1 = veiculo.TipoProprietario.ToString(),
                Coluna2 = $"Placa: {veiculo.Placa}",
                // Coluna3 = $"Dono: {veiculo.GetNomeDono()}",
                ItemReal = veiculo
            });
        }

        public override void AplicaPesquisa(string input)
        {
            ItensLista.Clear();
            ItemSelecionado = null;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioVeiculo(sessao);
                var veiculos = repositorio.BuscaTodosParaGrid(input);

                PreencherListaCom(veiculos);
            }
        }
    }
}