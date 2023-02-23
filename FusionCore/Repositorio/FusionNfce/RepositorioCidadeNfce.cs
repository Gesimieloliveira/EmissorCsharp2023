using System.Collections.Generic;
using FusionCore.FusionNfce.Cidade;
using FusionCore.FusionNfce.Uf;
using FusionCore.Repositorio.Contratos.FusionNfceContratos;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace FusionCore.Repositorio.FusionNfce
{
    public class RepositorioCidadeNfce : Repositorio<CidadeNfce, int>, IRepositorioCidadeNfce
    {
        public RepositorioCidadeNfce(ISession sessao) : base(sessao)
        {
        }

        public void Salvar(CidadeNfce cidade)
        {
            Sessao.SaveOrUpdate(cidade);
        }

        public IEnumerable<CidadeNfce> BuscarCidadePorUf(UfNfce uf)
        {
            var cidades = Sessao.QueryOver<CidadeNfce>()
                .Where(c => c.SiglaUf == uf.Sigla).List<CidadeNfce>();

            return cidades;
        }
    }
}