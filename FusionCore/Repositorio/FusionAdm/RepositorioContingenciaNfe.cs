using System.Linq;
using FusionCore.FusionAdm.ContingenciaSefaz;
using FusionCore.FusionAdm.Emissores;
using NHibernate;

namespace FusionCore.Repositorio.FusionAdm
{
    public class RepositorioContingenciaNfe : Repositorio<ContingenciaNfe, int>
    {
        public RepositorioContingenciaNfe(ISession sessao) : base(sessao)
        {
        }

        public void Persistir(ContingenciaNfe contingencia)
        {
            Sessao.Persist(contingencia);
            Sessao.Flush();
        }

        public void Alterar(ContingenciaNfe contigencia)
        {
            Sessao.Update(contigencia);
            Sessao.Flush();
        }

        public ContingenciaNfe ContingenciaAberta(int emissorId)
        {
            var query = Sessao.QueryOver<ContingenciaNfe>()
                .Where(c => c.FinalizadaEm == null);

            var contingencias = query.List();
            var ativa = contingencias.FirstOrDefault(c => c.EmissorFiscal.Id == emissorId);

            return ativa;
        }

        public ContingenciaNfe ContingenciaAberta(EmissorFiscal emissor)
        {
            return ContingenciaAberta(emissor.Id);
        }
    }
}