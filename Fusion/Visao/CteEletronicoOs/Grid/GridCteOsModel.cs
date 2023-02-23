using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Fusion.Visao.CteEletronicoOs.Emitir;
using FusionCore.FusionAdm.CteEletronicoOs.Emissao;
using FusionCore.FusionAdm.CteEletronicoOs.Flags;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Helpers.Hidratacao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;
using FusionWPF.Base.Utils.Dialogs;
using NHibernate.Util;

namespace Fusion.Visao.CteEletronicoOs.Grid
{
    public class GridCteOsModel : ViewModel
    {
        private ObservableCollection<GridCteOsDTO> _listaCteOs;
        private GridCteOsDTO _cteOsDTOSelecionado;

        public GridCteOsDTO CteOsDTOSelecionado
        {
            get => _cteOsDTOSelecionado;
            set
            {
                _cteOsDTOSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<GridCteOsDTO> ListaCteOs
        {
            get => _listaCteOs;
            set
            {
                _listaCteOs = value;
                PropriedadeAlterada();
            }
        }


        public ICommand NovoCommand => GetSimpleCommand(NovoAction);

        public void Inicializar()
        {
            MontarObjetosDeGrid();
        }

        private void MontarObjetosDeGrid()
        {
            ListaCteOs = new ObservableCollection<GridCteOsDTO>();
            CteOsDTOSelecionado = null;
            BuscarCteOs().ToList().ForEach(ListaCteOs.Add);
        }

        private IEnumerable<GridCteOsDTO> BuscarCteOs()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioCteOs(sessao).BuscarTodosGridCteOsDTO();
            }
        }

        private void NovoAction(object obj)
        {
            var form = new CteOsEmitirForm(new CteOsEmitirFormModel(new CteOs()));
            form.ShowDialog();
            MontarObjetosDeGrid();
        }

        public void OpcoesCteOs()
        {
            switch (CteOsDTOSelecionado.Status)
            {
                case Status.Autorizada:
                    new CteOsEletronicaOpcoes(new CteOsEletronicaOpcoesModel(BuscarCteOsPorId())).ShowDialog();
                    MontarObjetosDeGrid();
                    break;
                case Status.Pendente:
                    var model = new CteOsEmitirFormModel(BuscarCteOsPorId());
                    var form = new CteOsEmitirForm(model);
                    form.ShowDialog();
                    MontarObjetosDeGrid();
                    break;
                case Status.Cancelada:
                    DialogBox.MostraInformacao("CT-e OS Cancelada!");
                    break;
                case Status.Denegada:
                    DialogBox.MostraInformacao("CT-e OS Denegada!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private CteOs BuscarCteOsPorId()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioCteOs(sessao).GetPeloId(CteOsDTOSelecionado.Id);
            }
        }

        public void AplicarPesquisa(string textoPesquisado)
        {
            ListaCteOs.Clear();

            if (textoPesquisado.IsNullOrEmpty())
            {
                BuscarCteOs().ToList().ForEach(ListaCteOs.Add);
                return;
            }

            BuscarCteOs(textoPesquisado).ForEach(ListaCteOs.Add);
        }

        private IEnumerable<GridCteOsDTO> BuscarCteOs(string filtro)
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                return new RepositorioCteOs(sessao).BuscarTodosGridCteOsDTOFiltrando(filtro.ToUpper());
            }
        }
    }
}