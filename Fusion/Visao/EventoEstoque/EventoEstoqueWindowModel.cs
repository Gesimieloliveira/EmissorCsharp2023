using System.Collections.ObjectModel;
using System.IO;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.Legacy.Ativos;
using FusionCore.Repositorio.Legacy.Buscas.Adm.EstoqueEvento;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace Fusion.Visao.EventoEstoque
{
    public class EventoEstoqueWindowModel : ModelBase
    {
        public ObservableCollection<EstoqueEventoDTO> Eventos { get; }
        private string _nomeProdutoFiltro;

        public EventoEstoqueWindowModel()
        {
            Eventos = new ObservableCollection<EstoqueEventoDTO>();
        }

        public virtual string NomeProdutoFiltro
        {
            get { return _nomeProdutoFiltro; }
            set
            {
                if (value == _nomeProdutoFiltro) return;
                _nomeProdutoFiltro = value;
                PropriedadeAlterada();
            }
        }

        public void PreencherView(ProdutoDTO filtro)
        {
            NomeProdutoFiltro = filtro.Nome;
            Eventos.Clear();

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            {
                var repositorio = new RepositorioComun<EstoqueEventoDTO>(sessao);
                var eventos = repositorio.Busca(new EventosPeloProduto(filtro));

                if (eventos == null)
                    throw new InvalidDataException("Nenhum resultado para o produto");

                eventos.ForEach(Eventos.Add);
            }
        }
    }
}