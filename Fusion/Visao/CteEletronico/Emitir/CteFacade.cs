using Fusion.Visao.CteEletronico.Builder;
using FusionCore.FusionAdm.CteEletronico.Emissao;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using NHibernate;

namespace Fusion.Visao.CteEletronico.Emitir
{
    public class CteFacade
    {
        private readonly CteBuilder _builder;

        public CteFacade(CteBuilder builder)
        {
            _builder = builder;
        }

        public void SalvarCabecalho(ISession sessao)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.Salvar(_builder.Construir());
            repositorio.SalvarCteRodoviario(_builder.Construir().CteRodoviario);
            repositorio.SalvarCteEmitente(_builder.Construir().CteEmitente);
        }

        public void DeletaCteSubstituto()
        {
            var cte = _builder.Construir();
            if (cte.Id == 0) return;

            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                var repositorio = new RepositorioCte(sessao);
                repositorio.DeletarCteSubstituto(cte.Id);

                transacao.Commit();
            }
        }

        public void SalvarDocumentoNfe(ISession sessao, CteDocumentoNfe documentoNfe)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.SalvarDocumentoNfe(documentoNfe);
        }

        public void SalvarDocumentoImpresso(ISession sessao, CteDocumentoImpresso documentoImpresso)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.SalvarDocumentoImpresso(documentoImpresso);
        }

        public void SalvarDocumentoOutro(ISession sessao, CteDocumentoOutro documentoOutro)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.SalvarDocumentoOutro(documentoOutro);
        }

        public void SalvarInfoCarga(ISession sessao, CteInfoQuantidadeCarga infoQuantidadeCarga)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.SalvarInfoCarga(infoQuantidadeCarga);
        }

        public void SalvarCte(ISession sessao, Cte cte)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.Salvar(cte);
        }

        public void SalvarVeiculoTransportado(ISession sessao, CteVeiculoTransportado veiculo)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.SalvarVeiculoTransportado(veiculo);
        }

        public void SalvarComponente(ISession sessao, CteComponenteValorPrestacao componente)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.SalvarComponente(componente);
        }

        public void DeletarDocumentoNfe(ISession sessao, CteDocumentoNfe documentoNfe)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.DeletarDocumentoNfe(documentoNfe);
        }

        public void DeletarDocumentoImpresso(ISession sessao, CteDocumentoImpresso documentoImpresso)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.DeletarDocumentoImpresso(documentoImpresso);
        }

        public void DeletarDocumentoOutro(ISession sessao, CteDocumentoOutro documentoOutro)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.DeletarDocumentoOutro(documentoOutro);
        }

        public void DeletaInfoQuantidadeCarga(ISession sessao, CteInfoQuantidadeCarga infoQuantidadeCarga)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.DeletarInfoQuantidadeCarga(infoQuantidadeCarga);
        }

        public void DeletaVeiculoTransportado(ISession sessao, CteVeiculoTransportado veiculoTransportado)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.DeletaVeiculoTransportado(veiculoTransportado);
        }

        public void DeletarDocumentoAnterior(ISession sessao, CteDocumentoAnterior documentoAnterior)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.DeletarAnterior(documentoAnterior);
        }

        public void DeletarComponenteValorPrestacao(ISession sessao, CteComponenteValorPrestacao componente)
        {
            var repositorio = new RepositorioCte(sessao);
            repositorio.DeletarComponente(componente);
        }
    }
}