using System;
using FusionCore.FusionAdm.Emissores;
using FusionCore.FusionAdm.Sessao;
using FusionCore.Repositorio.FusionAdm;
using FusionCore.Repositorio.Legacy.Entidades.Adm;
using FusionCore.Vendas.Autorizadores.Infra;
using FusionCore.Vendas.Faturamentos;
using NHibernate;

namespace FusionCore.Vendas.Autorizadores.Nfce
{
    public class CriarCupomFiscal
    {
        private readonly FaturamentoVenda _venda;
        private readonly UsuarioDTO _usuarioLogado;

        public CriarCupomFiscal(FaturamentoVenda venda, UsuarioDTO usuarioLogado)
        {
            _venda = venda;
            _usuarioLogado = usuarioLogado;
        }

        public void Cria()
        {
            using (var sessao = SessaoHelperFactory.AbrirSessaoAdm())
            using (var transacao = sessao.BeginTransaction())
            {
                ValidaCupomJaAutorizado(_venda, sessao);

                var empresa = _venda.Empresa;

                var emissorFiscal = BuscaEmissorFiscal(sessao, empresa);

                CriaCupomSeNaoExistir(sessao, emissorFiscal);

                transacao.Commit();
            }
        }

        private void CriaCupomSeNaoExistir(ISession sessao, EmissorFiscal emissorFiscal)
        {
            var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

            if (repositorioCupomFiscal.ExisteCupomParaEssaVenda(_venda)) return;

            var cupomFiscal = new CupomFiscal(_venda, emissorFiscal.Id, 0, 0, 0
                , emissorFiscal.EmissorFiscalNfce.Ambiente, _usuarioLogado);
            cupomFiscal.EmissaoNormal();

            repositorioCupomFiscal.SalvarOuAlterar(cupomFiscal);
        }

        private EmissorFiscal BuscaEmissorFiscal(ISession sessao, EmpresaDTO empresa)
        {
            var emissorFiscal = new RepositorioEmissorFiscal(sessao).BuscarEmissorFaturamentoNfcePorEmpresa(empresa);

            if (emissorFiscal == null)
                throw new InvalidOperationException("Cadaste um emissor fiscal para faturamento/nfc-e");

            return emissorFiscal;
        }

        private void ValidaCupomJaAutorizado(FaturamentoVenda venda, ISession sessao)
        {
            var repositorioCupomFiscal = new RepositorioCupomFiscal(sessao);

            if (repositorioCupomFiscal.CupomAutorizado(venda))
                throw new InvalidOperationException("Cupom (nfc-e) autorizada.");
        }
    }
}