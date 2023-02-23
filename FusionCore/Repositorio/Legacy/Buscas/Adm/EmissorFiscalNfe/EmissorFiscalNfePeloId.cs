using FusionCore.FusionAdm.Emissores;
using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using NHibernate;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.EmissorFiscalNfe
{
    public class EmissorFiscalNfePeloId : IBuscaUnico<EmissorFiscalNFE>
    {
        private readonly byte _id;

        public EmissorFiscalNfePeloId(byte id)
        {
            _id = id;
        }

        public EmissorFiscalNFE Busca(ISession sessao)
        {
            return sessao.Get<EmissorFiscalNFE>(_id);
        }
    }
}
