using System;
using System.Collections.Generic;
using FusionCore.FusionPdv.Sessao;
using FusionCore.Repositorio.Legacy.Ativos.Pdv;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using FusionLibrary.VisaoModel;
using NHibernate.Util;

namespace FusionPdv.Visao.Cliente
{
    public class ConsultarClienteModel : ModelBase
    {

        private IList<ClienteDt> _listaDeCliente;
        private ClienteDt _cliente;
        private string _filtroPorNome;

        public IList<ClienteDt> ListaDeCliente
        {
            get { return _listaDeCliente; }
            set
            {
                _listaDeCliente = value; 
                PropriedadeAlterada();
            }
        }

        public string FiltroPorNome
        {
            get
            {
                return _filtroPorNome;
            }

            set
            {
                _filtroPorNome = value;
                PropriedadeAlterada();
            }
        }

        public ClienteDt ClienteSelecionado
        {
            get { return _cliente; }
            set
            {
                _cliente = value; 
                PropriedadeAlterada();
            }
        }

        public void ConsultarClientePorNome()
        {
            try
            {
                using (var sessao = GerenciaSessao.ObterSessao("SessaoPdv").AbrirSessao())
                {
                 ListaDeCliente =
                    new ClienteRepositorio(sessao).Busca(new ClienteDt
                    {
                        Nome = _filtroPorNome
                    });   
                }
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException("Falha ao buscar o cliente, tente novamente.", ex);
            }
        }

        public ClienteDt PrimeiroItemDaLista
        {
            get { return (ClienteDt) _listaDeCliente.FirstOrNull(); }
        }
    }
}
