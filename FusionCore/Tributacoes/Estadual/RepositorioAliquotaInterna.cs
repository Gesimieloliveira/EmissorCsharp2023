using System;
using System.Collections.Generic;
using System.Linq;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using NHibernate;
using NHibernate.SqlCommand;

namespace FusionCore.Tributacoes.Estadual
{
    public class RepositorioAliquotaInterna : Repositorio<AliquotaInterna, Guid>
    {
        public RepositorioAliquotaInterna(ISession sessao) : base(sessao)
        {
        }

        public AliquotaInterna ObterPorSiglaUfDestino(string siglaUf)
        {
            AliquotaInterna aliquotaInterna = null;
            EstadoDTO estadoDTO = null;

            return Sessao.QueryOver(() => aliquotaInterna)
                .JoinAlias(() => aliquotaInterna.EstadoUf, () => estadoDTO, JoinType.InnerJoin)
                .Where(() => estadoDTO.Sigla == siglaUf).SingleOrDefault<AliquotaInterna>();
        }

        public override IList<AliquotaInterna> BuscaTodos()
        {
            return base.BuscaTodos().OrderBy(x => x.EstadoUf.Nome).ToList();
        }

        public void Alterar(AliquotaInterna aliquotaInterna)
        {
            Sessao.Update(aliquotaInterna);
        }
    }
}