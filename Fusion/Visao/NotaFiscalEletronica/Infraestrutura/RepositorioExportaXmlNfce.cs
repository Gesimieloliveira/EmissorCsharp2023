using System;
using System.Collections.Generic;
using FusionCore.ExportacaoPacote.Empacotadores;
using FusionCore.FusionAdm.Fiscal.Exportacoes.Contratos;
using FusionCore.FusionNfce.Fiscal;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Autorizadores.Infra;
using NHibernate;

namespace Fusion.Visao.NotaFiscalEletronica.Infraestrutura
{
    public class RepositorioExportaXmlNfce : Repositorio<Nfce, int> , IRepositorioExportacaoXml
    {
        public RepositorioExportaXmlNfce(ISession sessao) : base(sessao)
        {
        }


        public IEnumerable<IEnvelope> BuscarXmlExportacao(DateTime inicio, DateTime fim, EmpresaDTO empresa)
        {
            var repositorioAdmNfce = new RepositorioNfceAdm(Sessao);
            var repositorioCupomFiscal = new RepositorioCupomFiscal(Sessao);

            var envelopes = new List<IEnvelope>();

            envelopes.AddRange(repositorioAdmNfce.BuscarXmlExportacao(inicio, fim, empresa));
            envelopes.AddRange(repositorioCupomFiscal.BuscarXmlExportacao(inicio, fim, empresa));

            return envelopes;
        }
    }
}