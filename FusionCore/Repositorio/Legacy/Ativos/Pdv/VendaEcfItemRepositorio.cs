using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.Legacy.Base;
using FusionCore.Repositorio.Legacy.Base.Execao;
using FusionCore.Repositorio.Legacy.Entidades.Pdv;
using NHibernate;
using NHibernate.Linq;

namespace FusionCore.Repositorio.Legacy.Ativos.Pdv
{
    public class VendaEcfItemRepositorio : RepositorioBase<VendaEcfItemDt>
    {
        public VendaEcfItemRepositorio(ISession sessao) : base(sessao)
        {
        }

        public IList<VendaEcfItemDt> BuscarVendasEcfItemsDeUmaVenda(VendaEcfDt vendaEcf)
        {
            try
            {
                return Sessao.Query<VendaEcfItemDt>().Where(v => v.VendaEcfDt.Id.Equals(vendaEcf.Id)).ToList();
            }
            catch (Exception ex)
            {
                throw new RepositorioExeption("Erro ao buscar o venda ecf item", ex);
            }
        }
    }
}