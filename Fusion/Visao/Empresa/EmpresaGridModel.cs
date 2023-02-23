using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Sessao;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Papeis.Enums;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.Empresa;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.Wpf.Conversores;

namespace Fusion.Visao.Empresa
{
    public class EmpresaGridModel : GridPadraoModel<EmpresaDTO>
    {
        private readonly SessaoSistema _sessaoSistema = SessaoSistema.Instancia;

        public EmpresaGridModel()
        {
            MostraPesquisaRapida = false;
        }

        protected override void OnLoaded()
        {
            MostraBotaoNovo = _sessaoSistema.UsuarioLogado.VerificaPermissao.IsTemPermissao(Permissao.GERENCIAR_EMPRESA);
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>();

            var colunaRazao = new DataGridTextColumn
            {
                Header = "Razão social",
                Binding = new Binding("RazaoSocial")
            };

            var colunaFantasia = new DataGridTextColumn
            {
                Header = "Nome fantasia",
                Binding = new Binding("NomeFantasia")
            };

            var documentoUnico = new DataGridTextColumn
            {
                Header = "CNPJ/CPF",
                Binding = new Binding("DocumentoUnico") {Converter = new CpfCnpjMaskConverter()},
                Width = 150
            };

            colunas.Add(colunaRazao);
            colunas.Add(colunaFantasia);
            colunas.Add(documentoUnico);

            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {
            
        }

        public override Window JanelaNovo()
        {
            var formModel = new EmpresaFormModel(new EmpresaDTO());
            return new EmpresaForm(formModel);
        }

        public override Window JanelaAlterar()
        {
            var formModel = new EmpresaFormModel(Selecionado);
            return new EmpresaForm(formModel);
        }

        public override void PopularLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<EmpresaDTO>(sessao);
                Lista = repositorio.Busca(new TodasEmpresas());
            }
        }
    }
}