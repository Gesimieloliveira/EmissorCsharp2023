using System;
using FusionCore.FusionNfce.VendasPendentesMensais;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioVendaPendenteMensal : Repositorio<VendaPendenteMensal, int>, IRepositorioVendaPendenteMensal
    {
        public RepositorioVendaPendenteMensal(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(VendaPendenteMensal vendaPendenteMensal)
        {
            Sessao.SaveOrUpdate(vendaPendenteMensal);
        }

        public VendaPendenteMensal ObterVendaPendenteNaoResolvida()
        {
            return Sessao.QueryOver<VendaPendenteMensal>().Where(v => v.IsResolvido == false).SingleOrDefault();
        }

        public VendaPendenteMensal ObterVendaPendentePeloAnoEMes(DateTime anoMes)
        {
            return Sessao.QueryOver<VendaPendenteMensal>().Where(v => v.Ano == anoMes.Year && v.Mes == anoMes.Month).SingleOrDefault();
        }
    }
}