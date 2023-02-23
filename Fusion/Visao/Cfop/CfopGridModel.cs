using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Fusion.Visao.Base.Grid;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;

namespace Fusion.Visao.Cfop
{
    public class CfopGridModel : GridPadraoModel<CfopDTO>
    {
        public CfopGridModel()
        {
            MostraBotaoNovo = false;
        }

        public override ObservableCollection<DataGridColumn> ColunasDaGrid()
        {
            return new ObservableCollection<DataGridColumn>
            {
                CriaColuna("CFOP", nameof(CfopDTO.Id), 120),
                CriaColuna("Descrição", nameof(CfopDTO.Descricao))
            };
        }

        public override void AplicarPesquisa(string texto)
        {
            Lista?.Clear();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioCfop(sessao);

                if (string.IsNullOrWhiteSpace(texto))
                {
                    Lista = repositorio.BuscaTodos();
                    return;
                }

                Lista = repositorio.BuscaRapida(texto);
            }
        }

        public override Window JanelaNovo()
        {
            throw new ArgumentException("Não é possivel criar novos cfops");
        }

        public override Window JanelaAlterar()
        {
            var viewModel = new CfopFormModel(Selecionado);
            return viewModel.GetView();
        }

        public override void PopularLista()
        {
            AplicarPesquisa(string.Empty);
        }
    }
}