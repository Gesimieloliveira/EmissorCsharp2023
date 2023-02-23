using System;
using FusionCore.FusionAdm.Fiscal.Transparencia;
using FusionCore.FusionNfce.Fiscal.Tributacoes;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioIbptNfce : Repositorio<NfceIbpt, NfceCodigoIbpt>, IRepositorioIbptNfce
    {
        public RepositorioIbptNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(NfceIbpt ibpt)
        {
            Sessao.SaveOrUpdate(ibpt);
        }

        public NfceIbpt GetPeloNcm(string ncm)
        {
            var query = Sessao.QueryOver<NfceIbpt>()
                .Where(i => i.Id.Codigo == ncm && i.Id.Tipo == TipoIbpt.Ncm);

            var resultados = query.List();

            if (resultados == null || resultados.Count == 0)
                return null;

            if (resultados.Count == 1)
                return resultados[0];

            var rand = new Random().Next(0, resultados.Count - 1);
            return resultados[rand];
        }
    }
}