using System.Collections.Generic;
using FusionCore.FusionNfce.Cliente;
using FusionCore.FusionNfce.Cliente.Consultas;
using FusionCore.FusionNfce.Vendedores;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;
using NHibernate.Transform;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioPessoaNfce : Repositorio<ClienteNfce, int>, IRepositorioClienteNfce
    {
        public RepositorioPessoaNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ClienteNfce cliente)
        {
            Sessao.Merge(cliente);
        }

        public ClienteNfce BuscarClientePorDocumento(string documentoUnicoCliente)
        {
            var queryOver = Sessao.QueryOver<ClienteNfce>().Where(c => c.DocumentoUnico == documentoUnicoCliente)
                .Where(c => c.Ativo == true);

            var cliente = queryOver.OrderBy(x => x.Id).Asc.Take(1).SingleOrDefault<ClienteNfce>();

            return cliente;
        }

        public IList<ClienteDTO> BuscarClientesDTO()
        {
            ClienteNfce cliente = null;
            ClienteDTO resultado = null;

            var queryOver = Sessao.QueryOver(() => cliente).SelectList(list => list.Select(() => cliente.Id).WithAlias(() => resultado.Id)
                .Select(() => cliente.DocumentoUnico).WithAlias(() => resultado.DocumentoUnico)
                .Select(() => cliente.Nome).WithAlias(() => resultado.Nome));

            queryOver.Where(() => cliente.Ativo == true);

            queryOver.TransformUsing(Transformers.AliasToBean<ClienteDTO>());

            var lista = queryOver.List<ClienteDTO>();

            return lista;
        }

        public void Salvar(VendedorNfce vendedorNfce)
        {
            Sessao.Merge(vendedorNfce);
        }

        public IList<VendedorNfce> BuscarTodosVendedoresAtivos()
        {
            return Sessao.QueryOver<VendedorNfce>().List();
        }

        public bool NaoExisteVendedorCadastrado()
        {
            return Sessao.QueryOver<VendedorNfce>().RowCount() == 0;
        }

        public VendedorNfce BuscarVendedorPorId(int vendedorId)
        {
            return Sessao.QueryOver<VendedorNfce>().Where(x => x.Id == vendedorId).SingleOrDefault<VendedorNfce>();
        }
    }
}