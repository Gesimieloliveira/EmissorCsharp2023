using System;
using System.Collections.ObjectModel;
using System.Linq;
using FusionCore.FusionNfce.Cliente;
using FusionCore.FusionNfce.Cliente.Consultas;
using FusionCore.FusionNfce.Sessao;
using FusionCore.Repositorio.FusionNfce;
using FusionLibrary.VisaoModel;
using FusionNfce.Visao.Clientes.Model;
using NHibernate.Util;

namespace FusionNfce.Visao.Clientes
{
    public class ClientesFormModel : ModelBase
    {
        public ClientesFormModel()
        {
            InicializaModel();
        }

        private void InicializaModel()
        {
            Itens = new ObservableCollection<ClienteDTO>();

            EfetuaCacheDeClientes();
        }

        private void EfetuaCacheDeClientes()
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

            using (sessao)
            {
                var repositorio = new RepositorioPessoaNfce(sessao);

                _cache = new ObservableCollection<ClienteDTO>(repositorio.BuscarClientesDTO());
            }

            _cache.ForEach(Itens.Add);
        }

        public event EventHandler<ClienteEvent> RetornaItem;
        private string _buscaRapidaTexto;
        private ObservableCollection<ClienteDTO> _itens;
        private ObservableCollection<ClienteDTO> _cache;
        private ClienteDTO _itenSelecionado;

        public ClienteDTO ItenSelecionado
        {
            get { return _itenSelecionado; }
            set
            {
                if (Equals(value, _itenSelecionado)) return;
                _itenSelecionado = value;
                PropriedadeAlterada();
            }
        }


        public ObservableCollection<ClienteDTO> Itens
        {
            get { return _itens; }
            set
            {
                if (Equals(value, _itens)) return;
                _itens = value;
                PropriedadeAlterada();
            }
        }


        public string BuscaRapidaTexto
        {
            get { return _buscaRapidaTexto; }
            set
            {
                if (value == _buscaRapidaTexto) return;
                _buscaRapidaTexto = value;
                PropriedadeAlterada();
            }
        }

        public void BuscaRapida()
        {
            if (BuscaRapidaTexto == null)
            {
                Itens.Clear();
                _cache.ToList().ForEach(Itens.Add);
                return;
            }

            var codigo = default(int);

            try
            {
                codigo = int.Parse(BuscaRapidaTexto.ToUpper());
            }
            catch (Exception)
            {
                // ignored
            }


            var itensBuscado = _cache.Where(c => c.Nome.ToUpper().Contains(BuscaRapidaTexto.ToUpper()) || c.DocumentoUnico.ToUpper() == BuscaRapidaTexto.ToUpper()
            || codigo == c.Id).ToList();

            Itens.Clear();
            itensBuscado.ForEach(Itens.Add);
        }

        public void OnRetornaItem()
        {
            RetornaItem?.Invoke(this, new ClienteEvent(BuscarClientePeloId(ItenSelecionado)));
        }

        private ClienteNfce BuscarClientePeloId(ClienteDTO itenSelecionado)
        {
            var sessao = GerenciaSessaoNfce.ObterSessao(nameof(SessaoNfce)).AbrirSessao();

            using (sessao)
            {
                var repositorio = new RepositorioPessoaNfce(sessao);

                return repositorio.GetPeloId(itenSelecionado.Id);
            }
        }
    }
}