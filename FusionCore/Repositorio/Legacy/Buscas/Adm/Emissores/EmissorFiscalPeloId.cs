using FusionCore.Repositorio.Legacy.Contratos.Base.Busca;
using NHibernate;
using EmissorClasse = FusionCore.FusionAdm.Emissores.EmissorFiscal;

namespace FusionCore.Repositorio.Legacy.Buscas.Adm.Emissores
{
    public class EmissorFiscalPeloId : IBuscaUnico<EmissorClasse>
    {
        private readonly byte _id;

        public EmissorFiscalPeloId(byte id)
        {
            _id = id;
        }

        public EmissorClasse Busca(ISession sessao)
        {
            return sessao.Get<EmissorClasse>(_id);
        }
    }
}