using System.Collections.ObjectModel;
using FusionCore.FusionAdm.ConverterVendaParaNfe;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Dtos.Consultas;
using FusionCore.Repositorio.FusionAdm;
using FusionLibrary.VisaoModel;

namespace Fusion.Visao.ConverterVendaParaNfe
{
    public class ConverterParaNfeFormModel : ViewModel
    {
        private readonly IConverteVendaParaNFe _converteVendaParaNFe;
        private ObservableCollection<AbaPerfilNfeDTO> _lista;
        private AbaPerfilNfeDTO _itemSelecionado;

        public ConverterParaNfeFormModel(IConverteVendaParaNFe converteVendaParaNFe)
        {
            _converteVendaParaNFe = converteVendaParaNFe;
            InicializaLista();
        }

        private void InicializaLista()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioPerfilNfe(sessao);
                Lista = new ObservableCollection<AbaPerfilNfeDTO>(repositorio.BuscaPerfilDeSaidaNormal());
            }
        }

        public AbaPerfilNfeDTO ItemSelecionado
        {
            get => _itemSelecionado;
            set
            {
                _itemSelecionado = value;
                PropriedadeAlterada();
            }
        }

        public ObservableCollection<AbaPerfilNfeDTO> Lista
        {
            get => _lista;
            set
            {
                _lista = value;
                PropriedadeAlterada();
            }
        }


        public int ConverterPedidoParaNFe(AbaPerfilNfeDTO perfilSelecionado)
        {
            _converteVendaParaNFe.AdicionarPerfilNfe(perfilSelecionado);
            return _converteVendaParaNFe.Executar();
        }
    }
}