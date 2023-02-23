using System;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Aplicacao;
using FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Dominio;
using NHibernate;

namespace FusionCore.Vendas.Autorizadores.Nfce.Contingencia.Infraestrutura
{
    public class TodasContingencias : Repositorio<ContingenciaNfce, int>, ITodasContingenciasNfce
    {
        public TodasContingencias(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(ContingenciaNfce contingencia)
        {
            Sessao.Persist(contingencia);
        }

        public bool ExisteContingenciaEmAberto()
        {

            return Sessao.QueryOver<ContingenciaNfce>().Where(x => x.FinalizaEm >= DateTime.Now).RowCount() != 0;
        }

        public ContingenciaNfce BuscarContingenciaAberta()
        {
            return Sessao.QueryOver<ContingenciaNfce>().Where(x => x.FinalizaEm > DateTime.Now)
                .SingleOrDefault<ContingenciaNfce>();
        }
    } 
}