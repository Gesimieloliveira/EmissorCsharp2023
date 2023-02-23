using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.FusionAdm;
using PerfilNfeModel = FusionCore.FusionAdm.Fiscal.NF.Perfil.PerfilNfe;

namespace Fusion.Visao.PerfilNfe
{
    public class PerfilNfeGridModel : GridPadraoModel<PerfilNfeModel>
    {
        private IList<PerfilNfeModel> _cacheLista;

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            var colunas = new ObservableCollection<DataGridColumn>
            {
                CriaColuna("Codigo/ID", new Binding("Id") {StringFormat = "D11"}, 100),
                CriaColuna("Descrição do Perfil", "Descricao", DataGridLength.Auto),
                CriaColuna("Tipo da Operação", "TipoOperacao", DataGridLength.SizeToHeader),
                CriaColuna("Natureza da Operação", "NaturezaOperacao", DataGridLength.Auto)
            };


            return colunas;
        }

        public override void AplicarPesquisa(string texto)
        {

            if (texto.IsNullOrEmpty())
            {
                PopularLista();
                return;
            }

            Lista = _cacheLista.Where(perfilNfe => perfilNfe.Id.ToString().Equals(texto)
                                                   || perfilNfe.Descricao.ToUpper().Contains(texto.ToUpper())).ToList();
        }

        public override Window JanelaNovo()
        {
            return new PerfilNfeForm(new PerfilNfeFormModel());
        }

        public override Window JanelaAlterar()
        {
            return new PerfilNfeForm(new PerfilNfeFormModel(Selecionado));
        }

        public override void PopularLista()
        {
            using (var repositorio = new RepositorioPerfilNfe(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                Lista = repositorio.BuscaTodos();
                _cacheLista = Lista;
            }
        }
    }
}