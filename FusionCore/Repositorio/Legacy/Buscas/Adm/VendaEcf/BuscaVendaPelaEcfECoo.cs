using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.VendaEcf
{
    public class BuscaVendaPelaEcfECoo : IBuscaUnico<PdvVendaDTO>
    {
        private readonly short _ecfId;
        private readonly int _coo;

        public BuscaVendaPelaEcfECoo(short ecfId, int coo)
        {
            _ecfId = ecfId;
            _coo = coo;
        }

        public PdvVendaDTO Busca(ISession sessao)
        {
            var pdvVenda = sessao.QueryOver<PdvVendaDTO>().Where(p => p.PdvEcfDTO.Id == _ecfId && p.Coo == _coo).SingleOrDefault();

            return pdvVenda;
        }
    }
}
