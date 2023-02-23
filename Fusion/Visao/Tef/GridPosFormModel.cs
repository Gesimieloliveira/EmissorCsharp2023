using System.Collections.ObjectModel;
using System.Windows.Input;
using FusionCore.FusionAdm.Sessao;
using FusionCore.FusionAdm.Tef;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.Tef
{
    public class GridPosFormModel : ViewModel
    {
        private ObservableCollection<Pos> _colecaoPos;
        private Pos _posSelecionado;
        private string _textoPesquisado;

        public ObservableCollection<Pos> ColecaoPos
        {
            get => _colecaoPos;
            set
            {
                if (Equals(value, _colecaoPos)) return;
                _colecaoPos = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandNovoRegistro => GetSimpleCommand(NovoRegistroAction);

        public Pos PosSelecionado
        {
            get => _posSelecionado;
            set
            {
                if (Equals(value, _posSelecionado)) return;
                _posSelecionado = value;
                PropriedadeAlterada();
            }
        }

        private void NovoRegistroAction(object obj)
        {
            new TefPosForm(new TefPosFormModel(new Pos())).ShowDialog();
            PesquisarPos();
        }

        public void Editar()
        {
            new TefPosForm(new TefPosFormModel(_posSelecionado)).ShowDialog();
            PesquisarPos();
        }

        public void Pesquisar()
        {
            PesquisarPos();
        }


        public string TextoPesquisado
        {
            get => _textoPesquisado;
            set
            {
                if (value == _textoPesquisado) return;
                _textoPesquisado = value;
                PropriedadeAlterada();
            }
        }

        public ICommand CommandBuscaFiltrada => GetSimpleCommand(BuscaFiltradaAction);

        private void BuscaFiltradaAction(object obj)
        {
            PesquisarPos();
        }

        private void PesquisarPos()
        {
            using (var repositorio = new RepositorioPos(SessaoHelperFactory.AbrirSessaoAdm()))
            {
                ColecaoPos = new ObservableCollection<Pos>(repositorio.BuscaComFiltro(TextoPesquisado));
            }
        }
    }
}